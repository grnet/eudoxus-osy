using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class BookPriceRequest
    {
        public enBookApplicationType ApplicationType
        {
            get { return (enBookApplicationType)ApplicationTypeInt; }
            set
            {
                if (ApplicationTypeInt != (int)value)
                    ApplicationTypeInt = (int)value;
            }
        }

        public enBookContentType ContentType
        {
            get { return (enBookContentType)ContentTypeInt; }
            set
            {
                if (ContentTypeInt != (int)value)
                    ContentTypeInt = (int)value;
            }
        }

        public enBookCoverType CoverType
        {
            get { return (enBookCoverType)CoverTypeInt; }
            set
            {
                if (CoverTypeInt != (int)value)
                    CoverTypeInt = (int)value;
            }
        }

        public enBookPriceRequestState State
        {
            get { return (enBookPriceRequestState)StateInt; }
            set
            {
                if (StateInt != (int)value)
                    StateInt = (int)value;
            }
        }
    }
}
