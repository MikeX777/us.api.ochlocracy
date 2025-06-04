using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Us.Ochlocracy.Model
{
    public enum Chamber
    {
        House,
        Senate
    }

    public enum ChamberCode
    {
        H,
        S
    }

    public enum BillType
    {
        S,
        SRES,
        SCONRES,
        SJRES,
        HR,
        HRES,
        HCONRES,
        HJRES,
    }
}
