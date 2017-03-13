using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Services.Resources;

namespace EudoxusOsy.Services
{
    public static class Extensions
    {
        public static string TakeFirstTwoCharacters(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            if (s.Length < 2)
                return s;

            return s.Substring(0, 2);
        }
    }
}
