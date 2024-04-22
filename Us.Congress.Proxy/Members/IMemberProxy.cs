using LanguageExt;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Members;

namespace Us.Congress.Proxy.Members
{
    public interface IMemberProxy
    {
        public Task<Either<Error, MemberPagedResponse>> GetPagedMembers(int offset, int limit, CancellationToken cancellationToken = default);
        public Task<Either<Error, MemberResponse>> GetMember(string bioguideId, CancellationToken cancellationToken = default);
    }
}
