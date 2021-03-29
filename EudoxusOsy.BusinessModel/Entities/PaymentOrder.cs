using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class PaymentOrder
    {
        public enPaymentOrderState State
        {
            get { return (enPaymentOrderState)StateInt; }
            set
            {
                if (StateInt != (int)value)
                    StateInt = (int)value;
            }
        }

        public string PaymentOffice
        {
            get
            {
                if (CatalogGroup == null || CatalogGroup.Supplier == null)
                {
                    return string.Empty;
                }
                else if(CatalogGroup.Supplier.PaymentPfoID == -1)
                {
                    return CatalogGroup.Supplier.PaymentPfo;
                }
                else if(CatalogGroup.Supplier.PaymentPfoID.HasValue)
                {
                    return EudoxusOsyCacheManager<PublicFinancialOffice>.Current.Get(CatalogGroup.Supplier.PaymentPfoID.Value).Name;
                }
                return string.Empty;
            }
        }

        public string TotalAmount
        {
            get
            {                
                if(CatalogGroup.Supplier.HasLogisticBooks == true)
                {
                    if(CatalogGroup.Vat > 0m)
                    {
                        return ((decimal)Amount + CatalogGroup.Vat).Value.ToString("c");
                    }
                    else
                    {
                        return ((decimal)Amount * (1 + Math.Round((decimal)(CatalogGroup.Deduction.Vat / 100), 2, MidpointRounding.AwayFromZero))).ToString("c");
                    }
                }

                return Amount.ToString("c");
            }

        }

        public decimal TotalAmountDecimal
        {
            get
            {
                if (CatalogGroup.Supplier.HasLogisticBooks == true)
                {
                    if (CatalogGroup.Vat > 0m)
                    {
                        return ((decimal) Amount + CatalogGroup.Vat).Value;
                    }
                    else
                    {
                        return ((decimal) Amount * (1 + Math.Round((decimal) (CatalogGroup.Deduction.Vat / 100), 2)));
                    }
                }                

                return (decimal)Amount;
            }

        }
    }
}
