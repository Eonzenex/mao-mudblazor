using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;

namespace mao_mudblazor_server.Shared;

public class MainLayoutBase : LayoutComponentBase
{
    public event Action OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
    protected override void OnInitialized() => OnChange += StateHasChanged;

    public bool UsingDarkMode { get; set; } = true;
    public MudTheme CurrentTheme { get; set; } = DarkMode;
    public void ToggleDarkMode()
    {
        UsingDarkMode = !UsingDarkMode;
        CurrentTheme = UsingDarkMode ? DarkMode : LightMode;
        NotifyStateChanged();
    }


    public readonly string LightModeIconStyle = "margin-left: auto;" +
                                                $"color: {DarkMode.Palette.ActionDefault}";
    
    
    public static MudTheme LightMode = new ()
    {
        Palette = new Palette
        {
            Primary = Colors.DeepOrange.Default,
            AppbarBackground = Colors.DeepOrange.Default
        }
    };
    
    public static MudTheme DarkMode = new()
    {
        Palette = new Palette
        {
            Background = Colors.Grey.Darken4,
            Surface = Colors.Grey.Darken3,
            
            TextPrimary = Colors.Grey.Lighten5,
            TextSecondary = new MudColor(Colors.Shades.White).SetAlpha(0.54).ToString(MudColorOutputFormats.RGBA),
            ActionDefault = Colors.Grey.Lighten5,
            
            Primary = Colors.DeepOrange.Default,
                
            AppbarBackground = Colors.DeepOrange.Default
        }
    };
}