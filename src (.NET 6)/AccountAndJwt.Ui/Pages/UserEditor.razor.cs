using AccountAndJwt.ApiClients.Http.Authorization.Interfaces;
using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Contracts.Models.Api;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using IAuthorizationService = AccountAndJwt.Ui.Services.Interfaces.IAuthorizationService;

namespace AccountAndJwt.Ui.Pages
{
    [Route("userEditor")]
    [Authorize(Roles = Role.Admin)]
    public partial class UserEditor
    {
        private const Int32 _numberValuesToAdd = 10;
        private const Int32 _pageSize = 15;

        private PageProgress _pageProgress;
        private Boolean _inProgress = true;
        private Boolean _addValuesInProgress;
        private String _errorMessage;

        private List<ValueAm> _valueList;
        private ValueAm _selectedValue;
        private Int32 _totalValues;


        private Modal _modalRef;
        private RoleAm[] _availableRoles;


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IAuthorizationHttpClient AuthorizationHttpClient { get; set; }

        [Inject]
        public IAuthorizationService Authorization { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override async Task OnInitializedAsync()
        {
            var accessToken = Authorization.User.ServerTokens.AccessToken;
            _availableRoles = await AuthorizationHttpClient.GetAvailableRolesAsync(accessToken);

        }
        private Task ShowModal()
        {
            return _modalRef.Show();
        }
        private Task OnCloseClicked()
        {
            return _modalRef.Hide();
        }
        private Task OnSaveChangesClicked()
        {
            return _modalRef.Hide();
        }
    }
}