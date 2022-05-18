using AccountAndJwt.Ui.Models.Radzen;
using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.JSInterop;
using System.Globalization;

namespace AccountAndJwt.Ui.Services
{
    public class RadzenThemeService : IRadzenThemeService
    {
        private const String _themeKey = "radzenTheme";
        private const String _defaultTheme = "default";

        private readonly IJSRuntime _jsRuntime;
        private readonly ILocalStorageService _localStorageService;


        public RadzenThemeService(IJSRuntime jsRuntime, ILocalStorageService localStorageService)
        {
            _jsRuntime = jsRuntime;
            _localStorageService = localStorageService;

            Themes = new[]
            {
                new Theme { Text = "Default Theme", Value = _defaultTheme},
                new Theme { Text = "Dark Theme", Value="dark" },
                new Theme { Text = "Software Theme", Value = "software"},
                new Theme { Text = "Humanistic Theme", Value = "humanistic" },
                new Theme { Text = "Standard Theme", Value = "standard" }
            };
        }


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        public Theme CurrentTheme { get; set; }
        public Theme[] Themes { get; set; }


        // IRadzenThemeService ////////////////////////////////////////////////////////////////////
        public void Initialize()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            SetTheme();
        }
        public async void Change(String themeValue)
        {
            if (!Themes.Select(p => p.Value).Contains(themeValue))
                return;

            CurrentTheme = FindTheme(themeValue);
            await _localStorageService.SetItemAsync(_themeKey, themeValue);
            await _jsRuntime.InvokeAsync<String>("setTheme", themeValue);
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private async void SetTheme()
        {
            var themeValue = await _localStorageService.GetItemAsync<String>(_themeKey);
            CurrentTheme = FindTheme(themeValue ?? _defaultTheme);

            await _jsRuntime.InvokeAsync<String>("setTheme", CurrentTheme.Value);
        }
        private Theme FindTheme(String themeValue)
        {
            var theme = Themes.FirstOrDefault(p => p.Value.Equals(themeValue));
            if (theme == null)
                return Themes.FirstOrDefault(p => p.Value.Equals(_defaultTheme));

            return theme;
        }
    }
}