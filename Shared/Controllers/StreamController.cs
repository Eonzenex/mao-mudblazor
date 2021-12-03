using System.Collections.Generic;
using mao_mudblazor_server.Shared.Structures;

namespace mao_mudblazor_server.Shared.Controllers;

public class StreamController
{
    #region Getters

    public static bool DoesStreamIdExists(int streamId) => CoreController.StreamControls.ContainsKey(streamId);
    public static IEnumerable<NewStreamControls> GetAllStreamControls() => CoreController.StreamControls.Values;
    public static NewStreamControls GetStreamControls(int streamId) => CoreController.StreamControls[streamId];
    
    #endregion

    
    #region Controls

    public static bool SetPaused(int streamId, bool newValue)
    {
        if (!CoreController.StreamControls.ContainsKey(streamId)) return false;

        var controls = GetStreamControls(streamId);
        lock (controls)
        {
            controls.Paused = newValue;
        }

        return true;
    }
    
    public static bool SetLoop(int streamId, bool newValue)
    {
        if (!CoreController.StreamControls.ContainsKey(streamId)) return false;

        var controls = GetStreamControls(streamId);
        lock (controls)
        {
            controls.Loop = newValue;
        }

        return true;
    }
    
    public static bool SetProgress(int streamId, float newValue)
    {
        if (!CoreController.StreamControls.ContainsKey(streamId)) return false;

        var controls = GetStreamControls(streamId);
        lock (controls)
        {
            controls.Progress = newValue;
            controls.ChangeProgress = true;
        }

        return true;
    }
    
    public static bool SetLength(int streamId, float newValue)
    {
        if (!CoreController.StreamControls.ContainsKey(streamId)) return false;

        var controls = GetStreamControls(streamId);
        lock (controls)
        {
            controls.Length = newValue;
        }

        return true;
    }
    
    public static bool SetVolume(int streamId, float newValue)
    {
        if (!CoreController.StreamControls.ContainsKey(streamId)) return false;

        var controls = GetStreamControls(streamId);
        lock (controls)
        {
            controls.Volume = newValue;
        }

        return true;
    }
    
    
    // Actions
    public static bool SetKill(int streamId, bool newValue)
    {
        if (!CoreController.StreamControls.ContainsKey(streamId)) return false;

        var controls = GetStreamControls(streamId);
        lock (controls)
        {
            controls.Kill = newValue;
        }

        return true;
    }
    
    public static bool SetRestart(int streamId, bool newValue)
    {
        if (!CoreController.StreamControls.ContainsKey(streamId)) return false;

        var controls = GetStreamControls(streamId);
        lock (controls)
        {
            controls.Restart = newValue;
        }

        return true;
    }
    
    public static bool SetStop(int streamId, bool newValue)
    {
        if (!CoreController.StreamControls.ContainsKey(streamId)) return false;

        var controls = GetStreamControls(streamId);
        lock (controls)
        {
            controls.Stop = newValue;
        }

        return true;
    }
    
    public static bool SetChangeProgress(int streamId, bool newValue)
    {
        if (!CoreController.StreamControls.ContainsKey(streamId)) return false;

        var controls = GetStreamControls(streamId);
        lock (controls)
        {
            controls.ChangeProgress = newValue;
        }

        return true;
    }

    #endregion
}