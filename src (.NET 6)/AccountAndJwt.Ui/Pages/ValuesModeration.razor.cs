using AccountAndJwt.Contracts.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace AccountAndJwt.Ui.Pages
{
    [Route("valuesModeration")]
    [Authorize(Roles = Role.Moderator)]
    public partial class ValuesModeration
    {

    }
}