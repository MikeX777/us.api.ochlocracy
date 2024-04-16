using System.Net;
using System.Net.Http.Headers;
using Us.Ochlocracy.Model.Extensions;

namespace Us.Proxy.Common.Extensions
{
    public static class HttpExtensions
    {
        public static HttpRequestHeaders RemoveHeader(this HttpRequestHeaders headers, string key)
        {
            headers.Remove(key);
            return headers;
        }

        public static string ReadRequestHeader(this HttpResponseMessage r, string key) =>
            r.RequestMessage != null
                ? r.RequestMessage.Headers.TryGetValues(key, out var values)
                    ? values.Any()
                        ? values.First()
                        : ""
                    : ""
                : "";

        public static async Task<string> ReadRequestContent(this HttpResponseMessage r) =>
            r.RequestMessage != null
                ? !(r.RequestMessage.Method == HttpMethod.Get || r.RequestMessage.Method == HttpMethod.Delete)
                    ? r.RequestMessage.Content != null
                        ? await r.RequestMessage.Content.ReadAsStringAsync()
                        : ""
                    : ""
                : "";

        public static HttpClient ReplaceHeader(this HttpClient client, string key, string? value)
        {
            client.DefaultRequestHeaders.Remove(key);
            if (value.HasValue())
                client.DefaultRequestHeaders.Add(key, value);

            return client;
        }

        /// <summary>
        /// Indicates whether <paramref name="statusCode"/> is in 200 range.
        /// </summary>
        /// <param name="statusCode">Status code to check.</param>
        public static bool IsSuccessStatusCode(this HttpStatusCode statusCode) =>
            (int)statusCode >= 200 && (int)statusCode <= 299;

        /// <summary>
        /// Indicates whether <paramref name="statusCode"/> is in 400 range.
        /// </summary>
        /// <param name="statusCode">Status code to check.</param>
        public static bool IsBadRequest(this HttpStatusCode statusCode) =>
            (int)statusCode >= 400 && (int)statusCode <= 499;

        /// <summary>
        /// Indicates whether <paramref name="statusCode"/> is in 500 range.
        /// </summary>
        /// <param name="statusCode">Status code to check.</param>
        public static bool IsServerError(this HttpStatusCode statusCode) =>
            (int)statusCode >= 500 && (int)statusCode <= 599;
    }
}
