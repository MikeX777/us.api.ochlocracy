using LanguageExt;
using Serilog;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Bills;
using Us.Proxy.Common;

namespace Us.Congress.Proxy.Bills
{
    public class BillProxy(string apiKey, HttpClient httpClient, IHttpCallLogger httpCallLogger, ILogger logger) : 
        ProxyBase(httpClient, httpClient.BaseAddress?.ToString() ?? string.Empty, ErrorSource.CongressAPI, httpCallLogger, logger), IBillProxy
    {
        private readonly string apiKey = apiKey;

        public Task<Either<Error, BillResponse>> GetBillDetail(int congress, BillType billType, string billNumber, CancellationToken cancellationToken = default) =>
            GetAsync<BillResponse>($"/bill/{congress}/{billType}/{billNumber}?api_key={apiKey}", cancellationToken: cancellationToken);

        public Task<Either<Error, BillPartialResponse>> GetCongressPagedBills(int congress, int offset, int limit, CancellationToken cancellationToken = default) =>
            GetAsync<BillPartialResponse>($"/bill/{congress}?api_key={apiKey}&offset={offset}&limit={limit}", cancellationToken: cancellationToken);

        public Task<Either<Error, BillPartialResponse>> GetCongressPagedBills(int congress, BillType billType, int offset, int limit, CancellationToken cancellationToken = default) =>
            GetAsync<BillPartialResponse>($"/bill/{congress}/{billType}?api_key={apiKey}&offset={offset}&limit={limit}", cancellationToken: cancellationToken);

        public Task<Either<Error, BillPartialResponse>> GetPagedBills(int offset, int limit, CancellationToken cancellationToken = default) =>
            GetAsync<BillPartialResponse>($"/bill?api_key={apiKey}&offset={offset}&limit={limit}", cancellationToken: cancellationToken);
    }
}
