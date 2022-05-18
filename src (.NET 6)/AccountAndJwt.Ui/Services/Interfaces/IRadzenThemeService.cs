using AccountAndJwt.Ui.Models.Radzen;

namespace AccountAndJwt.Ui.Services.Interfaces
{
    public interface IRadzenThemeService
    {
        Theme CurrentTheme { get; }
        Theme[] Themes { get; set; }

        void Initialize();
        void Change(String themeValue);
    }
}