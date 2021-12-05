using System;
using System.Collections.Generic;
using System.Linq;
using mao.backend;
using mao.backend.Controllers;
using mao.backend.Structures;
using Microsoft.AspNetCore.Components;

namespace mao.frontend.Pages
{
    public class MainBase : ComponentBase
    {
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        protected override void OnInitialized() => OnChange += StateHasChanged;
        
        
        public IEnumerable<GroupControls> Groups  => CoreController.GroupControls.Values.ToArray();

        public void AddNewGroup(string name = "New")
        {
            var safeName = string.IsNullOrEmpty(name) ? "New" : name;
            var groupId = CoreController.CreateGroup(0, safeName);
            Utils.Log($"Created group '{safeName}' (Group ID: '{groupId}')", LogLevel.Success);
            
            NotifyStateChanged();
        }
    }
}