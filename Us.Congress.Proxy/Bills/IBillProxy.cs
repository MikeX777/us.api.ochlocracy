using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Bills;

namespace Us.Congress.Proxy.Bills
{
    public interface IBillProxy
    {
        public Task<Either<Error, IEnumerable<BillPartial>>> GetPagedBills(int offset, int limit);
        public Task<Either<Error, IEnumerable<BillPartial>>> GetCongressPagedBills(int congress, int offset, int limit);
        public Task<Either<Error, IEnumerable<BillPartial>>> GetCongressPagedBills(int congress, BillType billType, int offset, int limit);
        public Task<Either<Error, Bill>> GetBillDetail(int congress, BillType billType, string billNumber);

    }
}
