using System;
using Microsoft.AspNetCore.Components;

namespace mao_mudblazor_server.Shared.Components.Small;

public class MuiSliderBase : ComponentBase
{
    protected const string SliderWrap = "border-radius: 12px;" + 
                                        "box-sizing: content-box;" + 
                                        "display: inline-block;" + 
                                        "position: relative;" + 
                                        "cursor: pointer;" + 
                                        "touch-action: none;" + 
                                        "color: rgb(144, 202, 249);" + 
                                        "width: 100%;" + 
                                        "padding: 0px;";
    
    protected const string SliderTrack = "display: block;" + 
                                         "position: absolute;" +
                                         "top: 40%;" + 
                                         "transform: translateY(-50%);" +
                                         "border-radius: inherit;" + 
                                         "border: 1px solid var(--mud-palette-primary);" + 
                                         "background-color: var(--mud-palette-primary);";

    public void SliderValueChanged(double value)
    {
        Value = (float) value;
        ValueChanged?.Invoke((float) value);
    }
    
    public float Percent() => (Value / Max) * 100;
    
    
    [Parameter]
    public float Min { get; set; } = 0;

    [Parameter]
    public float Max { get; set; } = 1;

    [Parameter]
    public double Step { get; set; } = 0.1;
    
    [Parameter]
    public Action<double> ValueChanged { get; set; }

    [Parameter]
    public float Value { get; set; } = 0.5f;

    [Parameter]
    public string Style { get; set; } = "";
}