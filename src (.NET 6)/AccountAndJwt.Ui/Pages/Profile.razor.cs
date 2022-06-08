using AccountAndJwt.ApiClients.Http.Authorization.Interfaces;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Ui.Services.Interfaces;
using AccountAndJwt.Ui.Shared;
using AutoMapper;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using IAuthorizationService = AccountAndJwt.Ui.Services.Interfaces.IAuthorizationService;

namespace AccountAndJwt.Ui.Pages
{
    [Authorize]
    [Route("profile")]
    public partial class Profile
    {
        private Validations _passwordValidations;
        private Validations _nameValidations;
        private Validations _emailValidations;
        private ServerValidationAlert _serverValidationAlert;
        private Boolean _inProgress;

        private ChangeNameRequestAm _nameModel;
        private ChangeEmailRequestAm _emailModel;
        private ResetPasswordRequestAm _passwordModel;


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IAuthorizationHttpClient AuthorizationHttpClient { get; set; }

        [Inject]
        public IAuthorizationService Authorization { get; set; }

        [Inject]
        public IBrowserPopupService BrowserPopupService { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }


        // HANDLERS ///////////////////////////////////////////////////////////////////////////////
        protected override void OnInitialized()
        {
            _nameModel = new ChangeNameRequestAm()
            {
                FirstName = Authorization.User.FirstName,
                LastName = Authorization.User.LastName,
            };
            _emailModel = new ChangeEmailRequestAm()
            {
                NewEmail = Authorization.User.Email,
            };
            _passwordModel = new ResetPasswordRequestAm()
            {
                OldPassword = String.Empty,
                NewPassword = String.Empty,
                ConfirmPassword = String.Empty
            };
        }
        private async Task ChangeNameAsync()
        {
            if (!await _nameValidations.ValidateAll())
                return;

            _inProgress = true;

            try
            {
                await AuthorizationHttpClient.ChangeNameAsync(_nameModel, Authorization.User.ServerTokens.AccessToken);
            }
            catch (Exception ex)
            {
                _serverValidationAlert.HandleException(ex);
            }
            finally
            {
                _inProgress = false;
            }
        }
        private async Task ChangeEmailAsync()
        {
            if (!await _emailValidations.ValidateAll())
                return;

            _inProgress = true;

            try
            {
                await AuthorizationHttpClient.ChangeEmailAsync(_emailModel, Authorization.User.ServerTokens.AccessToken);
            }
            catch (Exception ex)
            {
                _serverValidationAlert.HandleException(ex);
            }
            finally
            {
                _inProgress = false;
            }
        }
        private async Task ResetPasswordAsync()
        {
            if (!await _passwordValidations.ValidateAll())
                return;

            _inProgress = true;

            try
            {
                await AuthorizationHttpClient.ResetPasswordAsync(_passwordModel, Authorization.User.ServerTokens.AccessToken);
            }
            catch (Exception ex)
            {
                _serverValidationAlert.HandleException(ex);
            }
            finally
            {
                _inProgress = false;
            }
        }
    }
}