using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.JSInterop;

namespace AccountAndJwt.Ui.Services
{
    public class BrowserPopupService : IBrowserPopupService
    {
        private readonly IJSRuntime _jsRuntime;


        public BrowserPopupService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }


        // IBrowserPopupService ///////////////////////////////////////////////////////////////////
        public async void Alert(String message)
        {
            await _jsRuntime.InvokeVoidAsync("alert", message);
        }
        public async Task<String> Prompt(String title, String initialValue)
        {
            return await _jsRuntime.InvokeAsync<String>("prompt", title, initialValue);
        }
        public async Task<Boolean> Confirm(String question)
        {
            return await _jsRuntime.InvokeAsync<Boolean>("confirm", question);
        }
    }
}