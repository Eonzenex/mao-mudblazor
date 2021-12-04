using System;
using System.IO;
using System.Threading.Tasks;
using mao.backend;
using mao.backend.Controllers;
using mao.backend.Structures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace mao.frontend.Shared.Components;

public class AudioSlotBase : ComponentBase
{
    public event Action OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
    protected override Task OnInitializedAsync()
    {
        Interval();
        return base.OnInitializedAsync();
    }

    protected override void OnInitialized() => OnChange += StateHasChanged;


    public bool KillInterval => StreamControls.Kill;
    public async Task Interval()
    {
        while (!KillInterval)
        {
            await Task.Delay(100);
            NotifyStateChanged();
        }
    }


    [CascadingParameter(Name = "UsingDarkMode")] protected bool? UsingDarkMode { get; set; } = true;
    [Parameter] public int StreamId { get; set; } = -1;
    [Parameter] public Action OnKill { get; set; }

    
    public bool TryingToKill { get; set; } = false;
    protected NewStreamControls StreamControls => TryingToKill ? null : StreamController.GetStreamControls(StreamId);
    protected Variant KillBtnVariant => UsingDarkMode ?? false ? Variant.Filled : Variant.Outlined;

    
    public float Volume => StreamControls?.Volume ?? 1;

    public float Progress => StreamControls?.Progress ?? 0;
    public string FileName => StreamControls is not null ? Path.GetFileName(StreamControls.FilePath) : "Unknown";

    public void ChangeVolume(double value)
    {
        if (Utils.SafeFloatEquality((float) value, Volume)) return;
        
        StreamController.SetVolume(StreamId, (float) value);
        Utils.Log($"Stream '{StreamId}' changed volume to '{value:F1}'");
    }

    public void Kill()
    {
        StreamController.SetKill(StreamId, true);
        Utils.Log($"Stream '{StreamId}' killed");

        TryingToKill = true;
        OnKill?.Invoke();
    }

    public void ChangeProgress(double value)
    {
        if (Utils.SafeFloatEquality((float) value, Progress)) return;
        
        StreamController.SetProgress(StreamId, (float) value);
        Utils.Log($"Stream '{StreamId}' changed progress to '{value:F1}'");
    }
    public void Restart()
    {
        StreamController.SetRestart(StreamId, true);
        Utils.Log($"Stream '{StreamId}' restarted");
    }
    public void TogglePlayPause()
    {
        StreamController.SetPaused(StreamId, !StreamControls.Paused);
        var verb = StreamControls.Paused ? "paused" : "resumed";
        Utils.Log($"Stream '{StreamId}' {verb}");
    }
    public void Stop()
    {
        StreamController.SetStop(StreamId, true);
        Utils.Log($"Stream '{StreamId}' stopped");
    }
    public void ToggleLoop()
    {
        StreamController.SetLoop(StreamId, !StreamControls.Loop);
        Utils.Log($"Stream '{StreamId}' toggled loop to {StreamControls.Loop}");
    }

    public string FormatProgress(float value)
    {
        var mutableProgress = value;
        const int minuteDivisor = 60;
        const int hoursDivisor = minuteDivisor * 60;
        
        var hours = (int) Math.Floor(mutableProgress / hoursDivisor);
        if (hours != 0) mutableProgress %= hoursDivisor;
        
        var minutes = (int) Math.Floor(mutableProgress / minuteDivisor);
        if (minutes != 0) mutableProgress %= minuteDivisor;

        var seconds = mutableProgress;

        return hours != 0 
            ? $"{hours:00}:{minutes:00}:{seconds:00}" 
            : $"{minutes:00}:{seconds:00}";
    }
}