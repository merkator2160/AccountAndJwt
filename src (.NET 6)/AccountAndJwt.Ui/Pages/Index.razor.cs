using AccountAndJwt.Common.Exceptions;
using AccountAndJwt.Ui.Services.Interfaces;
using Blazorise;
using Blazorise.Markdown;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace AccountAndJwt.Ui.Pages
{
    [Route("")]
    public partial class Index
    {
        private String _text = "Hi";
        private String _markdownValue = "## Custom Toolbar\nCustom functions, icons and buttons can be defined for the toolbar.";


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
            Navigation.NavigateTo("userEditor");
        }
        private async void ButtonClicked()
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
        private void OnCustomButtonClicked(MarkdownButtonEventArgs eventArgs)
        {
            // TODO: Permission issue, investigate
            //NotificationService.Info($"Name: {eventArgs.Name} Value: {eventArgs.Value}");
        }
    }
}