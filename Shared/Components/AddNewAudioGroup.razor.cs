using System;
using Microsoft.AspNetCore.Components;

namespace mao_mudblazor_server.Shared.Components;

public class AddNewAudioGroupBase : ComponentBase
{
    [Parameter] public Action<string> Callback { get; set; }
    [Parameter] public string Class { get; set; } = "";
    public string GroupName { get; set; }
    
    
    
    public void CallbackThen()
    {
        Callback(GroupName);
        GroupName = "";
    }
}