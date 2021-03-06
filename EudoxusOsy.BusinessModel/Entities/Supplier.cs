﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class Supplier
    {
        public enSupplierType SupplierType
        {
            get { return (enSupplierType)SupplierTypeInt; }
            set
            {
                if (SupplierTypeInt != (int)value)
                    SupplierTypeInt = (int)value;
            }
        }

        public enSupplierStatus Status
        {
            get { return (enSupplierStatus)StatusInt; }
            set
            {
                if (StatusInt != (int)value)
                    StatusInt = (int)value;
            }
        }

        public bool ZeroVatEligible
        {
            get
            {                
                return (!HasLogisticBooks.HasValue || HasLogisticBooks == false) && SupplierType == enSupplierType.SelfPublisher;
            }            
        }

        public SupplierIBAN CurrentIBAN
        {
            get{
                if (SupplierIBANs != null)
                {
                    return SupplierIBANs.OrderByDescending(x => x.ID).FirstOrDefault();
                }
                return null;
            }
        }
    }
}
