using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace AccountAndJwt.Ui.Pages
{
    [Authorize]
    [Route("profile")]
    public partial class Profile
    {

    }
}