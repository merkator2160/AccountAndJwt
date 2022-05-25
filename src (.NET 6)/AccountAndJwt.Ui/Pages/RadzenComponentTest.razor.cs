using AccountAndJwt.Contracts.Models.Exceptions;
using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace AccountAndJwt.Ui.Pages
{
    [Route("radzenTest")]
    public partial class RadzenComponentTest
    {
        private String _text = "Hi";


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IRadzenThemeService RadzenThemeService { get; set; }

        [Inject]
        public IBrowserPopupService BrowserPopupService { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override void OnInitialized()
        {

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
            BrowserPopupService.Alert(new HttpServerException(HttpMethod.Post, HttpStatusCode.MovedPermanently, "https://qna.habr.com/q/381656", "Message!"));
            //BrowserPopupService.Alert(new ApplicationException("Message!", new Exception("Inner exception message!")));
        }
    }
}