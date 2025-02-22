﻿using System;
using System.Linq;
using mao.backend;
using mao.backend.Controllers;
using mao.backend.Structures;
using Microsoft.AspNetCore.Components;

namespace mao.frontend.Shared.Components;

public class SelectAudioDeviceBase : ComponentBase
{
    public event Action OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
    protected override void OnInitialized()
    {
        OnChange += StateHasChanged;
        GetAudioDevices();
    }


    [Parameter] public int GroupId { get; set; }
    public OutputDevice[] OutputDevices { get; set; }
    public int SelectedOutputDevice => GroupController.GetGroupControls(GroupId).DeviceId;

    public void GetAudioDevices()
    {
        OutputDevices = DeviceController.GetOutputDevices().ToArray();
    }

    public void ChangeAudioDevice(int value)
    {
        GroupController.SetDeviceId(GroupId, value);

        var outputDevice = DeviceController.GetOutputDevice(value);
        Utils.Log($"Group ID '{GroupId}' moved to '{outputDevice.Name}' (Device ID: '{outputDevice.Id}')");
        NotifyStateChanged();
    }
}