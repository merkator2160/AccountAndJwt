using AccountAndJwt.Common.Exceptions;
using AccountAndJwt.Ui.Services.Interfaces;
using Blazorise;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace AccountAndJwt.Ui.Pages
{
    [Route("")]
    public partial class Index
    {
        private String _text = "Show browser alert";



        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IBrowserPopupService BrowserPopupService { get; set; }

        [Inject]
        private INotificationService NotificationService { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }



        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override void OnInitialized()
        {
#if DEBUG
            Navigation.NavigateTo("userEditor");
#endif
        }
        private async void OnButtonClicked()
        {
            //_text = "Hello world!";

            //BrowserPopupService.Alert("Hello world!");

            //var result = await BrowserPopupService.Prompt("Title", "qwerty");
            //BrowserPopupService.Alert(result);

            //var result = await BrowserPopupService.Confirm("Question?");
            //BrowserPopupService.Alert(result.ToString());

            //BrowserPopupService.Alert(new Exception("Test"));
            BrowserPopupService.Alert(new HttpServerException(HttpMethod.Post, HttpStatusCode.MovedPermanently, "https://qna.habr.com/q/381656", "Message!").ToString());
            //BrowserPopupService.Alert(new ApplicationException("Message!", new Exception("Inner exception message!")));
        }
    }
}