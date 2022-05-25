namespace AccountAndJwt.Ui.Services.Interfaces
{
    public interface IBrowserPopupService
    {
        void Alert(Exception ex);
        void Alert(String message);
        Task<String> Prompt(String title, String initialValue);
        Task<Boolean> Confirm(String question);
    }
}