using Microsoft.AspNetCore.Http;

namespace GameStore.Web.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string PathAndQuery(this HttpRequest request)
        {
            var pathAndQuery = request.QueryString.HasValue
                ? $"{request.Path}{request.QueryString}"
                : request.Path.ToString();

            return pathAndQuery;
        }
        
    }
}