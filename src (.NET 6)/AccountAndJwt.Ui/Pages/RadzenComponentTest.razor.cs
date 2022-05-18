﻿using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace AccountAndJwt.Ui.Pages
{
    [Route("radzenTest")]
    public partial class RadzenComponentTest
    {
        private String _text = "Hi";



        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IRadzenThemeService RadzenThemeService { get; set; }



        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override void OnInitialized()
        {

        }
        private void ButtonClicked()
        {
            _text = "Hello world!";
        }
    }
}