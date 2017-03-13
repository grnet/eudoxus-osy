using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class SupplierDetail
    {
        public enIdentificationType LegalPersonIdentificationType
        {
            get { return (enIdentificationType) LegalPersonIdentificationTypeInt; }
            set
            {
                if (LegalPersonIdentificationTypeInt != (int)value)
                    LegalPersonIdentificationTypeInt = (int)value;
            }
        }

        public enIdentificationType SelfPublisherIdentificationType
        {
            get { return (enIdentificationType)SelfPublisherIdentificationTypeInt; }
            set
            {
                if (SelfPublisherIdentificationTypeInt != (int)value)
                    SelfPublisherIdentificationTypeInt = (int)value;
            }
        }

        public enIdentificationType ContactIdentificationType
        {
            get { return (enIdentificationType)ContactIdentificationTypeInt; }
            set
            {
                if (ContactIdentificationTypeInt != (int)value)
                    ContactIdentificationTypeInt = (int)value;
            }
        }
    }
}
