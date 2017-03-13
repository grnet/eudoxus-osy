using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Imis.Domain;

namespace EudoxusOsy.BusinessModel
{
    public class LockService
    {
        private IUnitOfWork UnitOfWork { get; set; }
        private ILockable Item { get; set; }
        private string Username { get; set; }

        public LockService(IUnitOfWork uow, ILockable item)
        {
            UnitOfWork = uow;
            Item = item;
            Username = Thread.CurrentPrincipal.Identity.Name;
        }

        public bool Lock()
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
                throw new InvalidOperationException("User not authenticated.");

            if (Item.LockedAt.HasValue && Item.LockedAt.Value.AddSeconds(Config.LockWindow) > DateTime.Now)
            {
                if (Item.LockedBy == Username)
                    return ExtendLock();
                else
                    return false;
            }

            Item.LockedAt = DateTime.Now;
            Item.LockedBy = Username;

            UnitOfWork.Commit();
            return true;
        }

        public bool ExtendLock()
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
                throw new InvalidOperationException("User not authenticated.");

            if (Item.LockedBy != Username)
                return false;

            Item.LockedAt = DateTime.Now;

            UnitOfWork.Commit();
            return true;
        }

        public bool Release()
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
                throw new InvalidOperationException("User not authenticated.");

            if (Item.LockedBy != Username)
                return false;

            Item.LockedAt = null;
            Item.LockedBy = null;

            UnitOfWork.Commit();
            return true;
        }
    }
}
