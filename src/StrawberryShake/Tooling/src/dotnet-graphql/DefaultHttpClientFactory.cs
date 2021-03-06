using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace StrawberryShake.Tools
{
    public class DefaultHttpClientFactory
        : IHttpClientFactory
    {
        public HttpClient Create(Uri uri, string? token, string? scheme)
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress = uri;
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(
                    new ProductHeaderValue(
                        "StrawberryShake",
                        typeof(InitCommand).Assembly!.GetName()!.Version!.ToString())));

            if (token is not null)
            {
                if (string.IsNullOrWhiteSpace(scheme))
                {
                    httpClient.DefaultRequestHeaders
                        .TryAddWithoutValidation("Authorization", token);
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(scheme, token);
                }
            }

            return httpClient;
        }
    }
}
