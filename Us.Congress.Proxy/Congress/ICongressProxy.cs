using LanguageExt;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Congress;

namespace Us.Congress.Proxy.Congress
{
    public interface ICongressProxy
    {
        public Task<Either<Error, CongressPagedResponse>> GetPagedCongress(int offset, int limit, CancellationToken cancellationToken = default);
        public Task<Either<Error, CongressResponse>> GetCongress(int congress, CancellationToken cancellationToken = default);
    }
}
