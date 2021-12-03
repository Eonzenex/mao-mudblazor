using System;
using System.Collections.Generic;
using System.Linq;
using mao_mudblazor_server.Shared.Controllers;
using mao_mudblazor_server.Shared.Structures;
using Microsoft.AspNetCore.Components;

namespace mao_mudblazor_server.Pages
{
    public class MainBase : ComponentBase
    {
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        protected override void OnInitialized() => OnChange += StateHasChanged;
        
        
        public IEnumerable<NewGroupControls> Groups  => CoreController.GroupControls.Values.ToArray();

        public void AddNewGroup(string name = "New")
        {
            var safeName = string.IsNullOrEmpty(name) ? "New" : name;
            var groupId = CoreController.CreateGroup(0, safeName);
            Utils.Log($"Created group '{safeName}' (Group ID: '{groupId}')", LogLevel.Success);
            
            NotifyStateChanged();
        }
    }
}