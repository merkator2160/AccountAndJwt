using AccountAndJwt.Contracts.Models.Exceptions;
using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.JSInterop;
using System.Text;

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
        public void Alert(Exception ex)
        {
            var sb = new StringBuilder();
            ParseException(sb, ex);

            Alert(sb.ToString());
        }
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


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private void ParseException(StringBuilder sb, Exception ex)
        {
            if (ex is HttpServerException exception)
                sb.Append($"[{exception.Verb}] {exception.Uri}\n");

            sb.Append($"Message: {ex.Message}");

            if (!String.IsNullOrEmpty(ex.StackTrace))
                sb.Append($"\nStackTrace:\n{ex.StackTrace}");

            if (ex.InnerException == null)
                return;

            sb.Append("\n\nInnerException:\n");
            ParseException(sb, ex.InnerException);
        }
    }
}