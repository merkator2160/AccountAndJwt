using AccountAndJwt.Common.Exceptions;
using AccountAndJwt.Contracts.Models.Api.Errors;
using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace AccountAndJwt.Ui.Shared
{
    public partial class ServerValidationAlert
    {
        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IBrowserPopupService BrowserPopupService { get; set; }

        public ModelStateAm ModelState { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Hide()
        {
            if (ModelState == null)
                return;

            ModelState = null;
            StateHasChanged();
        }
        public void HandleException(Exception ex)
        {
            if (ex is HttpServerException httpServerException)
            {
                switch ((Int32)httpServerException.StatusCode)
                {
                    case 460:
                        HandleServerValidationError(ex.Message);
                        StateHasChanged();
                        return;
                    case 400:
                        HandleInvalidModelState(ex.Message);
                        StateHasChanged();
                        return;
                    default:
                        BrowserPopupService.Alert(httpServerException.ToString());
                        return;
                }
            }

            BrowserPopupService.Alert($"{ex.Message}\r\n{ex.StackTrace}");
        }
        private void HandleInvalidModelState(String message)
        {
            ModelState = JsonConvert.DeserializeObject<ModelStateAm>(message);
        }
        private void HandleServerValidationError(String message)
        {
            ModelState = new ModelStateAm()
            {
                Errors = new Dictionary<String, String[]>()
                {
                    {
                        "Server validation error:", new []
                        {
                            message
                        }
                    }
                }
            };
        }
    }
}