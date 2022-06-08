using AccountAndJwt.Common.Exceptions;
using AccountAndJwt.Contracts.Models.Api.Errors;
using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace AccountAndJwt.Ui.Shared
{
    public partial class ServerValidationAlert
    {
        private ModelStateAm _modelState;


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IBrowserPopupService BrowserPopupService { get; set; }

        public Boolean IsActive => _modelState != null;


        // HANDLERS ///////////////////////////////////////////////////////////////////////////////
        private void OnCloseButtonClicked()
        {
            _modelState = null;
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Hide()
        {
            if (_modelState == null)
                return;

            _modelState = null;
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
            _modelState = JsonConvert.DeserializeObject<ModelStateAm>(message);
        }
        private void HandleServerValidationError(String message)
        {
            _modelState = new ModelStateAm()
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