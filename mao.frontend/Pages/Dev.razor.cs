using System;
using System.Collections.Generic;
using mao.backend.Controllers;
using mao.backend.Structures;
using Microsoft.AspNetCore.Components;

namespace mao.frontend.Pages;

public class DataBase : ComponentBase
{
    public event Action OnChange;
    protected void NotifyStateChanged() => OnChange?.Invoke();
    protected override void OnInitialized() => OnChange += StateHasChanged;


    public Dictionary<int, NewGroupControls> GroupDictionary => CoreController.GroupControls;
    public Dictionary<int, NewStreamControls> StreamDictionary => CoreController.StreamControls;

    public int CountStreamsInGroup(int groupId) => GroupDictionary[groupId].StreamIds.Count;
}