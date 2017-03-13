using System;
using System.Collections.Generic;

namespace EudoxusOsy.BusinessModel
{
    public class ChangeSet
    {
        public string Username { get; set; }

        public DateTime Time { get; set; }

        public List<PropertyChange> Changes { get; set; }

        public ChangeSet()
        {
            Username = string.Empty;
            Time = DateTime.Now;
            Changes = new List<PropertyChange>();
        }

        public ChangeSet(string user)
        {
            Username = user;
            Time = DateTime.Now;
            Changes = new List<PropertyChange>();
        }

        public ChangeSet(string user, DateTime time)
        {
            Username = user;
            Time = time;
            Changes = new List<PropertyChange>();
        }

    }
}
