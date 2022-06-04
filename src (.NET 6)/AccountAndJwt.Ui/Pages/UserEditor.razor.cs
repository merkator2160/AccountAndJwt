using AccountAndJwt.ApiClients.Http.Authorization.Interfaces;
using AccountAndJwt.Common.Exceptions;
using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Ui.Models.ViewModels;
using AccountAndJwt.Ui.Services.Interfaces;
using AutoMapper;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using IAuthorizationService = AccountAndJwt.Ui.Services.Interfaces.IAuthorizationService;

namespace AccountAndJwt.Ui.Pages
{
    [Route("userEditor")]
    [Authorize(Roles = Role.Admin)]
    public partial class UserEditor
    {
        private const Int32 _pageSize = 15;

        private PageProgress _pageProgress;
        private Boolean _inProgress = true;
        private String _errorMessage;

        private List<GridUserVm> _userList;
        private GridUserVm _selectedUser;
        private Int32 _totalUsers;

        private Modal _modalRef;
        private RoleAm[] _availableRoles;


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IAuthorizationHttpClient AuthorizationHttpClient { get; set; }

        [Inject]
        public IAuthorizationService Authorization { get; set; }

        [Inject]
        public IBrowserPopupService BrowserPopupService { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
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
        private async Task OnRowUpdatedAsync(SavedRowItem<GridUserVm, Dictionary<String, Object>> updatedRow)
        {
            _inProgress = true;

            try
            {
                //await AuthorizationHttpClient.UpdateValueAsync(Mapper.Map<UpdateValueRequestAm>(updatedRow.Item), Authorization.User.ServerTokens.AccessToken);
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
        private async Task OnRowRemovedAsync(GridUserVm value)
        {
            _inProgress = true;

            try
            {
                //await AuthorizationHttpClient.DeleteValueAsync(value.Id, Authorization.User.ServerTokens.AccessToken);
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
                //await AuthorizationHttpClient.AddValueAsync(Mapper.Map<AddValueRequestAm>(newRow.Item), Authorization.User.ServerTokens.AccessToken);
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
            user.FirstName = String.Empty;
            user.LastName = String.Empty;
            user.Email = String.Empty;
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



        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private void HandleException(Exception ex)
        {
            if (ex is HttpServerException httpServerException)
            {
                if ((Int32)httpServerException.StatusCode == 460)
                {
                    _errorMessage = ex.Message;
                    StateHasChanged();
                }
                else
                {
                    BrowserPopupService.Alert(httpServerException.ToString());
                }

                return;
            }

            BrowserPopupService.Alert($"{ex.Message}\r\n{ex.StackTrace}");
        }
        private void OnShowErrorClicked()
        {
            _errorMessage = String.IsNullOrEmpty(_errorMessage) ? "Error message." : String.Empty;
        }
    }
}