using LanguageExt;
using Serilog;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Congress;
using Us.Proxy.Common;

namespace Us.Congress.Proxy.Congress
{
    public class CongressProxy(string apiKey, HttpClient httpClient, IHttpCallLogger httpCallLogger, ILogger logger) :
        ProxyBase(httpClient, httpClient.BaseAddress?.ToString() ?? string.Empty, ErrorSource.CongressAPI, httpCallLogger, logger), ICongressProxy
    {
        private readonly string apiKey = apiKey;

        public Task<Either<Error, CongressResponse>> GetCongress(int congress, CancellationToken cancellationToken = default) =>
            GetAsync<CongressResponse>($"/congress/{congress}?api_key={apiKey}", cancellationToken: cancellationToken);

        public Task<Either<Error, CongressPagedResponse>> GetPagedCongress(int offset, int limit, CancellationToken cancellationToken = default) =>
            GetAsync<CongressPagedResponse>($"/congress?api_key={apiKey}&offset={offset}&limit={limit}", cancellationToken: cancellationToken);
    }
}
