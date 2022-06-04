using Blazorise;
using Blazorise.Markdown;
using Microsoft.AspNetCore.Components;

namespace AccountAndJwt.Ui.Pages
{
    [Route("blazoriseMarkdown")]
    public partial class BlazoriseMarkdown
    {
        private String _markdownValue = "## Custom Toolbar\nCustom functions, icons and buttons can be defined for the toolbar.";


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        private INotificationService Notification { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private void OnCustomButtonClicked(MarkdownButtonEventArgs eventArgs)
        {
            Notification.Info($"Name: {eventArgs.Name} Value: {eventArgs.Value}", "Title");
        }
    }
}