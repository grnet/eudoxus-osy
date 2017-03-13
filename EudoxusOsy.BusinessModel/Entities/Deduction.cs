using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class Deduction
    {
        public enDeductionVatType VatType
        {
            get { return (enDeductionVatType)VatTypeInt; }
            set
            {
                if (VatTypeInt != (int)value)
                    VatTypeInt = (int)value;
            }
        }
    }
}
