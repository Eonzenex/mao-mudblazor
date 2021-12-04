using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace mao.frontend.Shared.Components;

public class AddNewAudioSlotBase: ComponentBase
{
    [Parameter] public Action<InputFileChangeEventArgs> CreateAudioSlot { get; set; }
}