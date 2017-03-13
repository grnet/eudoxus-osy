using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class Phase
    {
        public enSemesterType SemesterType
        {
            get
            {
                if (ID % 2 == 0)
                {
                    return enSemesterType.Spring;
                }

                return enSemesterType.Winter;
            }
        }

        public int AcademicYear
        {
            get
            {
                var year = EudoxusOsyConstants.OSY_STARTING_YEAR + ID / 2;

                if (SemesterType == enSemesterType.Winter)
                {
                    year++;
                }

                return year - 1;
            }
        }

        public string AcademicYearString
        {
            get
            {
                return string.Format("{0}-{1}", AcademicYear, (AcademicYear + 1).ToString().Substring(2, 2));
            }
        }

        public string AcademicYearAndSemester
        {
            get
            {
                return string.Format("{0} ({1})", AcademicYearString, SemesterType.GetLabel());
            }
        }
    }
}
