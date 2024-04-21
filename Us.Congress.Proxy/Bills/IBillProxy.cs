using LanguageExt;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Bills;

namespace Us.Congress.Proxy.Bills
{
    public interface IBillProxy
    {
        public Task<Either<Error, BillPartialResponse>> GetPagedBills(int offset, int limit, CancellationToken cancellationToken = default);
        public Task<Either<Error, BillPartialResponse>> GetCongressPagedBills(int congress, int offset, int limit, CancellationToken cancellationToken = default);
        public Task<Either<Error, BillPartialResponse>> GetCongressPagedBills(int congress, BillType billType, int offset, int limit, CancellationToken cancellationToken = default);
        public Task<Either<Error, BillResponse>> GetBillDetail(int congress, BillType billType, string billNumber, CancellationToken cancellationToken = default);

    }
}
