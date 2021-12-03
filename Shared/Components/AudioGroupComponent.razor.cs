using System;
using System.Collections.Generic;
using System.Linq;
using mao_mudblazor_server.Shared.Controllers;
using mao_mudblazor_server.Shared.Structures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace mao_mudblazor_server.Shared.Components;

public class AudioGroupComponentBase : ComponentBase
{
    public event Action OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
    protected override void OnInitialized() => OnChange += StateHasChanged;
    
    
    [Parameter] public int GroupId { get; set; }
    public NewGroupControls GroupControls => GroupController.GetGroupControls(GroupId);

    public IEnumerable<int> Streams => CoreController.GetStreamIdsInGroup(GroupId)
        .Where(StreamController.DoesStreamIdExists)
        .Where(streamId => !StreamController.GetStreamControls(streamId).Kill)
        .ToList();

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
            var msg = $"Filepath does not exist: '{filePath}'";
            Utils.Log(msg, LogLevel.Error);
            // throw new FileNotFoundException(msg);
            return;
        }

        var newStreamId = CoreController.CreateStream(filePath, GroupId);
        Utils.Log($"Group '{GroupId}' created stream '{newStreamId}' (Device ID: '{GroupControls.DeviceId}')", LogLevel.Success);
        
        NotifyStateChanged();
    }
}