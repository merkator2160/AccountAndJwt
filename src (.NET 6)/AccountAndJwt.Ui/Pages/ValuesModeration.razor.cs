using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace AccountAndJwt.Ui.Pages
{
    [Route("valuesModeration")]
    [Authorize(Roles = "Moderator")]
    public partial class ValuesModeration
    {

    }
}