using System;
using System.Collections.Generic;
using System.Linq;
using mao.backend;
using mao.backend.Controllers;
using mao.backend.Structures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace mao.frontend.Shared.Components;

public class AudioGroupComponentBase : ComponentBase
{
    public event Action OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
    protected override void OnInitialized() => OnChange += StateHasChanged;
    
    
    [Parameter] public int GroupId { get; set; }
    [Parameter] public Action OnGroupKill { get; set; }
    public GroupControls GroupControls => GroupController.GetGroupControls(GroupId);

    public IEnumerable<int> Streams => CoreController.GetStreamIdsInGroup(GroupId)
        .Where(StreamController.DoesStreamIdExists)
        .Where(streamId => !StreamController.GetStreamControls(streamId).Kill)
        .ToList();

    public void KillGroup()
    {
        var numStreams = Streams.Count();
        GroupController.Kill(GroupId);
        
        Utils.Log($"Killed group '{GroupId}', {numStreams} streams killed", LogLevel.Success);
        
        OnGroupKill();
    }

    public void ChildOnKill()
    {
        NotifyStateChanged();
    }

    public void CreateNewAudioSlot(InputFileChangeEventArgs e)
    {
        if (string.IsNullOrEmpty(e.File.Name))
        {
            var msg = $"FileName not found in scanned audio sources: '{e.File.Name}'";
            Utils.Log(msg, LogLevel.Error);
            // throw new FileNotFoundException(msg);
            return;
        }

        var filePath = "";
        if (!CoreController.ValidateFileName(e.File.Name, ref filePath))
        {
            var msg = $"Filepath does not exist: '{e.File.Name}'";
            Utils.Log(msg, LogLevel.Error);
            // throw new FileNotFoundException(msg);
            return;
        }

        var newStreamId = CoreController.CreateStream(filePath, GroupId);
        Utils.Log($"Group '{GroupId}' created stream '{newStreamId}' (Device ID: '{GroupControls.DeviceId}')", LogLevel.Success);
        
        NotifyStateChanged();
    }
}