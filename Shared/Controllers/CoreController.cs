using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using mao_mudblazor_server.Shared.Structures;
using NAudio.Core.Wave.WaveOutputs;
using NAudio.Wave;

namespace mao_mudblazor_server.Shared.Controllers;

public static class CoreController
{
    public static readonly HashSet<string> AudioSources = new();
    
    public static readonly Dictionary<int, NewGroupControls> GroupControls = new ();
    public static readonly Dictionary<int, NewStreamControls> StreamControls = new ();


    public static void Init()
    {
        var audioSrcLocations = Utils.GetAllAudioSources("*.wav", "*.mp3", "*.ogg");
        foreach (var audioPath in audioSrcLocations)
        {
            AudioSources.Add(audioPath);
        }

        if (AudioSources.Count != 0)
        {
            Utils.Log($"Loaded '{AudioSources.Count}' audio sources from '{Utils.AudioSourcePaths.Count}' locations", LogLevel.Success);
        }
        else
        {
            Utils.Log("No audio sources found. Make sure `AudioSourcePaths` in `appsettings.json` is set to an array", LogLevel.Warn);
        }
    }

    public static bool ValidateFileName(string fileName, ref string filePath)
    {
        foreach (var audioSource in AudioSources.Where(audioSource => audioSource.Contains(fileName)))
        {
            filePath = audioSource;
            return true;
        }

        return false;
    }

    
    #region Getters

    public static IEnumerable<int> GetStreamIdsInGroup(int groupId)
    {
        if (!GroupControls.ContainsKey(groupId))
            throw new KeyNotFoundException($"Group ID not found in GroupControls: '{groupId}'");

        return GroupControls[groupId].StreamIds;
    }
    
    public static IEnumerable<NewStreamControls> GetStreamControlsInGroup(int groupId)
    {
        if (!GroupControls.ContainsKey(groupId))
            throw new KeyNotFoundException($"Group ID not found in GroupControls: '{groupId}'");

        return GetStreamIdsInGroup(groupId).Select(sid => StreamControls[sid]);
    }

    #endregion

    #region Setters

    public static void SafeDeleteStream(int streamId)
    {
        foreach (var groupControl in GroupControls.Values.Where(x => x.StreamIds.Contains(streamId)))
        {
            groupControl.StreamIds.Remove(streamId);
        }
        
        StreamControls.Remove(streamId);
    }

    #endregion

    
    private static void ControlStream(int groupId, int streamId, long position = 0)
    {
        if (!GroupControls.ContainsKey(groupId))
        {
            var msg = $"Group ID not found in GroupControls: '{groupId}'";
            Utils.Log(msg, LogLevel.Error);
            throw new KeyNotFoundException(msg);
        }
        
        if (!StreamControls.ContainsKey(streamId))
        {
            var msg = $"Stream ID not found in StreamControls: '{streamId}'";
            Utils.Log(msg, LogLevel.Error);
            throw new KeyNotFoundException(msg);
        }

        var groupControls = GroupControls[groupId];
        var streamControls = StreamControls[streamId];
        
        string filePath;
        lock (streamControls) { filePath = streamControls.FilePath; }
        
        if (!File.Exists(filePath)) 
        {
            var msg = $"Filepath not found: '{filePath}'";
            Utils.Log(msg, LogLevel.Error);
            throw new FileNotFoundException(msg);
        }

        using var audioFile = new AudioFileReader(filePath);
        var loopStream = new LoopStream(audioFile);
        using var outputDevice = new WaveOutEvent {DeviceNumber = groupControls.DeviceId};
        
        lock (streamControls) { streamControls.Length = (float) audioFile.TotalTime.TotalSeconds; }
        outputDevice.Init(loopStream);
        audioFile.Position = position;
        
        if (position != 0) outputDevice.Play();
        else outputDevice.Pause();
        
        while (true)
        {
            groupControls = GroupControls[groupId];
            streamControls = StreamControls[streamId];
            
            lock (streamControls)
            {
                streamControls.Volume = Math.Clamp(streamControls.Volume, 0, 1);
                audioFile.Volume = streamControls.Volume;
                
                // If group device has changed, change device
                if (groupControls.DeviceId != outputDevice.DeviceNumber)
                {
                    var continuePosition = audioFile.Position;
                    Task.Run(() => ControlStream(groupId, streamId, continuePosition));
                    
                    audioFile.Dispose();
                    outputDevice.Dispose();
                    return;
                }
                
                // If shut down the stream, safely dispose then exit
                if (streamControls.Kill)
                {
                    streamControls.Kill = false;
                    audioFile.Dispose();
                    outputDevice.Dispose();
                    break;
                }
                
                // If stopping, pause the stream then set the position to the start
                if (streamControls.Stop)
                {
                    streamControls.Stop = false;
                    streamControls.Paused = true;
                    outputDevice.Pause();
                    audioFile.Position = 0;
                }

                // If restart, set position to start
                if (streamControls.Restart)
                {
                    streamControls.Restart = false;
                    audioFile.Position = 0;
                }

                // If trying to change the audio position
                if (streamControls.ChangeProgress)
                {
                    streamControls.ChangeProgress = false;
                    audioFile.CurrentTime = TimeSpan.FromSeconds(streamControls.Progress);
                }

                // If playing and stream is not playing, resume
                if (!streamControls.Paused && outputDevice.PlaybackState != PlaybackState.Playing)
                {
                    outputDevice.Play();
                }
                
                // If paused and not paused, being pausing
                if (streamControls.Paused && outputDevice.PlaybackState != PlaybackState.Paused)
                {
                    outputDevice.Pause();
                }

                // Toggle looping
                if (streamControls.Loop && !loopStream.EnableLooping || !streamControls.Loop && loopStream.EnableLooping)
                {
                    loopStream.EnableLooping = streamControls.Loop;
                }
                
                // Update progress
                streamControls.Progress = (float) audioFile.CurrentTime.TotalSeconds;
            }
            
            Thread.Sleep(Utils.ThreadSleep);
        }

        SafeDeleteStream(streamId);
    }
    
    
    public static int CreateNewStreamControls(int groupId)
    {
        if (!GroupControls.ContainsKey(groupId))
        {
            var msg = $"Group ID not found in GroupControls: '{groupId}'";
            Utils.Log(msg, LogLevel.Error);
            throw new KeyNotFoundException(msg);
        }

        var streamId = 0;
        if (StreamControls.Count != 0) streamId = StreamControls.Keys.OrderBy(x => x).Last() + 1;

        if (StreamControls.ContainsKey(streamId))
        {
            var msg = $"Key WAS found: Stream ID already exists in StreamControls: '{streamId}'";
            Utils.Log(msg, LogLevel.Error);
            throw new KeyNotFoundException(msg);
        }
        
        if (!GroupControls[groupId].AddNewStreamId(streamId))
        {
            var msg = $"Key WAS found: Stream ID already exists in Group: '{streamId}'";
            Utils.Log(msg, LogLevel.Error);
            throw new KeyNotFoundException(msg);
        }
        
        StreamControls.Add(streamId, new NewStreamControls());

        return streamId;
    }
    
    public static int CreateGroup(int deviceId = -1, string name = "New")
    {
        var keysSorted = GroupControls.Keys.OrderBy(k => k).ToArray();
        var groupId = keysSorted.Length == 0 ? 0 : keysSorted[^1] + 1;
        GroupControls[groupId] = new NewGroupControls{GroupId = groupId, DeviceId = deviceId, Name = name};

        return groupId;
    }

    public static int CreateStream(string filePath, int groupId = 0, long continuePosition = 0)
    {
        if (!File.Exists(filePath))
        {
            var msg = $"File not found '{filePath}'";
            Utils.Log(msg, LogLevel.Error);
            throw new KeyNotFoundException(msg);
        }
        
        if (!GroupControls.ContainsKey(groupId))
        {
            var msg = $"Group ID not found in GroupControls: '{groupId}'";
            Utils.Log(msg, LogLevel.Error);
            throw new KeyNotFoundException(msg);
        }

        var streamId = CreateNewStreamControls(groupId);
        if (streamId < 0) return streamId;
        
        var streamControls = StreamControls[streamId];
        lock (streamControls) { streamControls.FilePath = filePath; }
        
        Task.Run(() => ControlStream(groupId, streamId, continuePosition));

        return streamId;
    }
}