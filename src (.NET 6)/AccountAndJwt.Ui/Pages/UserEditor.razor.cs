using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace AccountAndJwt.Ui.Pages
{
    [Route("userEditor")]
    [Authorize(Roles = "Admin")]
    public partial class UserEditor
    {

    }
}