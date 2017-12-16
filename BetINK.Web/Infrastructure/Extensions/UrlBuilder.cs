using System.Collections.Generic;

namespace BetINK.Web.Infrastructure.Extensions
{
    public static class UrlBuilder
    {
        public static string GetUrl(string controller, string action, string id = null, string area = null, Dictionary<string, string> additionalParameters = null)
        {
            var url = $"/{controller}/{action}";

            if (area != null)
                url = $"/{area}" + url;

            if (id != null)
                url = url + $"/{id}";

            if (additionalParameters != null)
            {
                foreach (var keyValue in additionalParameters)
                {
                    url = url + "?" + keyValue.Key + "=" + keyValue.Value;
                }

            }

            return url;
        }
    }
}
