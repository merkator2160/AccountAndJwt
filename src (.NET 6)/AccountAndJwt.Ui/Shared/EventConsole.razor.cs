using AccountAndJwt.Ui.Models.Radzen;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace AccountAndJwt.Ui.Shared
{
    public partial class EventConsole
    {
        private IList<Message> _messages = new List<Message>();



        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IJSRuntime JsRuntime { get; set; }


        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<String, Object> Attributes { get; set; }



        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await JsRuntime.InvokeVoidAsync("eval", "document.getElementById('event-console').scrollTop = document.getElementById('event-console').scrollHeight");
            }
        }
        private void OnClearClick()
        {
            Clear();
        }
        public void Clear()
        {
            _messages.Clear();

            InvokeAsync(StateHasChanged);
        }
        public void Log(String message)
        {
            _messages.Add(new Message { Date = DateTime.Now, Text = message });

            InvokeAsync(StateHasChanged);
        }
        public void Log(object value)
        {
            Log(JsonSerializer.Serialize(value));
        }
    }
}