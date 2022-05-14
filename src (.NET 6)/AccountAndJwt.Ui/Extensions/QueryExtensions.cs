using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using System.Web;

namespace AccountAndJwt.Ui.Extensions
{
    /// <summary>
    /// https://jasonwatmore.com/post/2020/08/09/blazor-webassembly-get-query-string-parameters-with-navigation-manager
    /// </summary>
    public static class QueryExtensions
    {
        public static String QueryString(this NavigationManager navigationManager, String key)
        {
            return navigationManager.QueryString()[key];
        }
        public static NameValueCollection QueryString(this NavigationManager navigationManager)
        {
            return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
        }
    }
}