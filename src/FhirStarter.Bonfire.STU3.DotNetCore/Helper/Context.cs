using System;
using Microsoft.AspNetCore.Http;

namespace FhirStarter.Bonfire.STU3.DotNetCore.Helper
{
    // https://stackoverflow.com/questions/42547653/how-to-access-current-absolute-uri-from-any-asp-net-core-class
    public static class Context
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private static Uri GetAbsoluteUri()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };
            return uriBuilder.Uri;
        }
        public static string GetAcceptHeaders()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var headers = request.Headers;
            return headers["Accept"];
        }
    }
}
