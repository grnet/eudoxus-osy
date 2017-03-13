using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public interface ILockable
    {
        DateTime? LockedAt { get; set; }
        string LockedBy { get; set; }
    }
}
