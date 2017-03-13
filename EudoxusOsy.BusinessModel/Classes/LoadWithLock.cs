using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class LoadWithLock<T> where T : class
    {
        public T Entity { get; set; }

        public bool IsLocked { get; set; }
        public string LockedBy { get; set; }

        public static LoadWithLock<T> GetUnlocked(T entity)
        {
            return new LoadWithLock<T>() { Entity = entity, IsLocked = false, LockedBy = null };
        }

        public static LoadWithLock<T> GetLocked(string lockedBy)
        {
            return new LoadWithLock<T>() { Entity = null, IsLocked = true, LockedBy = lockedBy };
        }
    }
}
