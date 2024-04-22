using LanguageExt;
using Serilog;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Members;
using Us.Proxy.Common;

namespace Us.Congress.Proxy.Members
{
    public class MemberProxy(string apiKey, HttpClient httpClient, IHttpCallLogger httpCallLogger, ILogger logger) :
        ProxyBase(httpClient, httpClient.BaseAddress?.ToString() ?? string.Empty, ErrorSource.CongressAPI, httpCallLogger, logger), IMemberProxy 
    {
        private readonly string apiKey = apiKey;

        public Task<Either<Error, MemberResponse>> GetMember(string bioguideId, CancellationToken cancellationToken = default) =>
            GetAsync<MemberResponse>($"/member/{bioguideId}?api_key={apiKey}", cancellationToken);

        public Task<Either<Error, MemberPagedResponse>> GetPagedMembers(int offset, int limit, CancellationToken cancellationToken = default) =>
            GetAsync<MemberPagedResponse>($"/member?api_key={apiKey}&offset={offset}&limit={limit}", cancellationToken);
    }
}
