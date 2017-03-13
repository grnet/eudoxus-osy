using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EudoxusOsy.Utils.Queue
{
    public interface IQueueWorker
    {
        void AddToQueue(IQueueEntry entry);
        void RemoveFromQueue(IQueueEntry entry);

        string Name { get; }

        int ProcessQueue();
    }
}
