using AccountAndJwt.ApiClients.Http.Authorization.Interfaces;
using AccountAndJwt.Common.Exceptions;
using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Errors;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Ui.Models.ViewModels;
using AccountAndJwt.Ui.Services.Interfaces;
using AutoMapper;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Text;
using IAuthorizationService = AccountAndJwt.Ui.Services.Interfaces.IAuthorizationService;

namespace AccountAndJwt.Ui.Pages
{
    [Route("userEditor")]
    [Authorize(Roles = Role.Admin)]
    public partial class UserEditor
    {
        private DataGrid<GridUserVm> _dataGrid;
        private PageProgress _pageProgress;
        private Boolean _inProgress = true;
        private String _errorMessage;

        private List<GridUserVm> _userList;
        private GridUserVm _selectedUser;
        private Int32 _totalUsers;

        private Modal _editPermissionsModal;
        private RoleAm[] _availableRoles;
        private RoleAm _selectedRole;
        private RoleAm _selectedUserRole;


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
        protected override async Task OnInitializedAsync()
        {
            var accessToken = Authorization.User.ServerTokens.AccessToken;
            _availableRoles = await AuthorizationHttpClient.GetAvailableRolesAsync(accessToken);
        }

        // Grid //
        private async Task OnReadDataAsync(DataGridReadDataEventArgs<GridUserVm> e)
        {
            _inProgress = true;

            try
            {
                if (!e.CancellationToken.IsCancellationRequested)
                {
                    var valuesPage = await AuthorizationHttpClient.GetUsersPagedAsync(new GetUsersPagedRequestAm()
                    {
                        PageSize = e.PageSize,
                        PageNumber = e.Page
                    }, Authorization.User.ServerTokens.AccessToken);
                    if (!e.CancellationToken.IsCancellationRequested)
                    {
                        _totalUsers = valuesPage.TotalItemCount;
                        _userList = new List<GridUserVm>(Mapper.Map<GridUserVm[]>(valuesPage.Users));
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                _inProgress = false;
            }
        }
        private async Task OnRowRemovedAsync(GridUserVm user)
        {
            _inProgress = true;

            try
            {
                await AuthorizationHttpClient.DeleteAccountAsync(user.Id, Authorization.User.ServerTokens.AccessToken);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                _inProgress = false;
            }
        }
        private async Task OnRowInsertedAsync(SavedRowItem<GridUserVm, Dictionary<String, Object>> newRow)
        {
            _inProgress = true;

            try
            {
                await AuthorizationHttpClient.RegisterAsync(Mapper.Map<RegisterUserRequestAm>(newRow.Item));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                _inProgress = false;
            }
        }
        private void OnValueNewItemDefaultSetter(GridUserVm user)
        {
            user.Login = String.Empty;
            user.Password = String.Empty;
            user.FirstName = String.Empty;
            user.LastName = String.Empty;
            user.Email = String.Empty;
        }

        // Change Email modal //
        private Task ShowEditPermissionsModal()
        {
            return _editPermissionsModal.Show();
        }
        private Task HideEditPermissionsModal()
        {
            return _editPermissionsModal.Hide();
        }
        private async Task AddRole()
        {
            _inProgress = true;

            try
            {
                await AuthorizationHttpClient.AddUserRoleAsync(new AddRemoveUserRoleRequestAm()
                {
                    RoleId = _selectedRole.Id,
                    UserId = _selectedUser.Id
                }, Authorization.User.ServerTokens.AccessToken);
                _selectedUser.RoleList.Add(_selectedRole);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                _inProgress = false;
            }
        }
        private async Task RemoveRole()
        {
            _inProgress = true;

            try
            {
                await AuthorizationHttpClient.RemoveUserRoleAsync(new AddRemoveUserRoleRequestAm()
                {
                    RoleId = _selectedUserRole.Id,
                    UserId = _selectedUser.Id
                }, Authorization.User.ServerTokens.AccessToken);
                _selectedUser.RoleList.Remove(_selectedUserRole);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                _inProgress = false;
            }
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private void HandleException(Exception ex)
        {
            if (ex is HttpServerException httpServerException)
            {
                switch ((Int32)httpServerException.StatusCode)
                {
                    case 460:
                        _errorMessage = ex.Message;
                        StateHasChanged();
                        return;
                    case 400:
                        _errorMessage = HandleInvalidModelState(ex.Message);
                        StateHasChanged();
                        return;
                    default:
                        BrowserPopupService.Alert(httpServerException.ToString());
                        return;
                }
            }

            BrowserPopupService.Alert($"{ex.Message}\r\n{ex.StackTrace}");
        }
        private String HandleInvalidModelState(String message)
        {
            // TODO: Error formatting improvements required

            var response = JsonConvert.DeserializeObject<ModelStateAm>(message);
            var sb = new StringBuilder();

            foreach (var x in response.Errors)
            {
                foreach (String str in x.Value)
                {
                    sb.Append($"{x.Key}: {str}\r\n");
                }
            }

            return sb.ToString();
        }
    }
}