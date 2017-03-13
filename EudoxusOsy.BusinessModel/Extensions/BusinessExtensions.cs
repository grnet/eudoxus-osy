using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public static class BusinessExtensions
    {
        public static bool IsLocked(this ILockable lockable)
        {
            if (lockable.LockedAt.HasValue)
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                    return lockable.LockedAt.Value.AddSeconds(Config.LockWindow) >= DateTime.Now && Thread.CurrentPrincipal.Identity.Name != lockable.LockedBy;
                else
                    return lockable.LockedAt.Value.AddSeconds(Config.LockWindow) >= DateTime.Now;
            }
            else
                return false;
        }
    }
}
