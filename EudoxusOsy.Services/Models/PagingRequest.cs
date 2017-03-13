﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.Services
{
    public class PagingRequest
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public int GetSkip()
        {
            return Skip.HasValue ? Skip.Value : 0;
        }

        public int GetTake()
        {
            if (Take.HasValue)
                return Take.Value > ApiConfig.Current.MaxPageSize ? ApiConfig.Current.MaxPageSize : Take.Value;
            else
                return ApiConfig.Current.DefaultPageSize;
        }
    }
}
