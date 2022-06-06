using AccountAndJwt.ApiClients.Http.Authorization.Interfaces;
using AccountAndJwt.Common.Exceptions;
using AccountAndJwt.Contracts.Const;
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
    [Route("valuesModeration")]
    [Authorize(Roles = Role.Moderator)]
    public partial class ValuesModeration
    {
        private const Int32 _numberValuesToAdd = 10;

        private PageProgress _pageProgress;
        private Boolean _inProgress = true;
        private Boolean _addValuesInProgress;
        private String _errorMessage;

        private List<GridValueVm> _valueList;
        private GridValueVm _selectedValue;
        private Int32 _totalValues;


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
        private async Task OnCreateSomeRandomValuesButtonClickedAsync()
        {
            _inProgress = true;
            _addValuesInProgress = true;

            try
            {
                for (Int32 i = 0; i < _numberValuesToAdd; i++)
                {
                    await _pageProgress.SetValueAsync(i * 10);
                    await AuthorizationHttpClient.AddValueAsync(new AddValueRequestAm()
                    {
                        Value = Random.Shared.Next(),
                        Commentary = "Randomly generated value on UI"
                    }, Authorization.User.ServerTokens.AccessToken);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                _inProgress = false;
                _addValuesInProgress = false;
            }
        }

        // Grid //
        private async Task OnReadDataAsync(DataGridReadDataEventArgs<GridValueVm> e)
        {
            _inProgress = true;

            try
            {
                if (!e.CancellationToken.IsCancellationRequested)
                {
                    var valuesPage = await AuthorizationHttpClient.GetValuesPagedAsync(e.PageSize, e.Page, Authorization.User.ServerTokens.AccessToken);
                    if (!e.CancellationToken.IsCancellationRequested)
                    {
                        _totalValues = valuesPage.TotalItemCount;
                        _valueList = new List<GridValueVm>(Mapper.Map<GridValueVm[]>(valuesPage.Values));
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
        private async Task OnRowUpdatedAsync(SavedRowItem<GridValueVm, Dictionary<String, Object>> updatedRow)
        {
            _inProgress = true;

            try
            {
                await AuthorizationHttpClient.UpdateValueAsync(Mapper.Map<UpdateValueRequestAm>(updatedRow.Item), Authorization.User.ServerTokens.AccessToken);
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
        private async Task OnRowRemovedAsync(GridValueVm value)
        {
            _inProgress = true;

            try
            {
                await AuthorizationHttpClient.DeleteValueAsync(value.Id, Authorization.User.ServerTokens.AccessToken);
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
        private async Task OnRowInsertedAsync(SavedRowItem<GridValueVm, Dictionary<String, Object>> newRow)
        {
            _inProgress = true;

            try
            {
                await AuthorizationHttpClient.AddValueAsync(Mapper.Map<AddValueRequestAm>(newRow.Item), Authorization.User.ServerTokens.AccessToken);
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
        private void OnValueNewItemDefaultSetter(GridValueVm value)
        {
            value.Value = Random.Shared.Next();
            value.Commentary = "New value";
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
        private void ShowError()
        {
            _errorMessage = String.IsNullOrEmpty(_errorMessage) ? "Error message." : String.Empty;
        }
    }
}