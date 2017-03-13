using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel.Flow
{
    public enum enCatalogGroupTriggers
    {
        Delete,
        Select,
        Approve,
        SendToYDE,
        ReturnFromYDE,
        RevertSelection,
        RevertApproval,
        RevertSelectionByMinistry
    }
}
