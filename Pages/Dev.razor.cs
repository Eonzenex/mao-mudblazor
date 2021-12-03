using System;
using System.Collections.Generic;
using mao_mudblazor_server.Shared.Controllers;
using mao_mudblazor_server.Shared.Structures;
using Microsoft.AspNetCore.Components;

namespace mao_mudblazor_server.Pages;

public class DataBase : ComponentBase
{
    public event Action OnChange;
    protected void NotifyStateChanged() => OnChange?.Invoke();
    protected override void OnInitialized() => OnChange += StateHasChanged;


    public Dictionary<int, NewGroupControls> GroupDictionary => CoreController.GroupControls;
    public Dictionary<int, NewStreamControls> StreamDictionary => CoreController.StreamControls;

    public int CountStreamsInGroup(int groupId) => GroupDictionary[groupId].StreamIds.Count;
}