using System;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public partial class BusinessLogicErrorsKP
    {
        public enErrorCode ErrorCode
        {
            get { return (enErrorCode)ErrorCodeInt; }
            set
            {
                if (ErrorCodeInt != (int)value)
                    ErrorCodeInt = (int)value;
            }
        }

        public enErrorEntityType ErrorEntityType
        {
            get { return (enErrorEntityType)ErrorEntityTypeInt; }
            set
            {
                if (ErrorEntityTypeInt != (int)value)
                    ErrorEntityTypeInt = (int)value;
            }
        }
    }
}
