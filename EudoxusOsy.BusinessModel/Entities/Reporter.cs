using System;

namespace EudoxusOsy.BusinessModel
{
    public partial class Reporter : IUserChangeTracking
    {
        protected override void OnPropertyChanged(string property)
        {

        }

        public enReporterType ReporterType
        {
            get { return (enReporterType)ReporterTypeInt; }
            set
            {
                if (ReporterTypeInt != (int)value)
                    ReporterTypeInt = (int)value;
            }
        }

        public enAuthorizationType AuthorizationType
        {
            get { return (enAuthorizationType)AuthorizationTypeInt; }
            set
            {
                if (AuthorizationTypeInt != (int)value)
                    AuthorizationTypeInt = (int)value;
            }
        }

        public enVerificationStatus VerificationStatus
        {
            get { return (enVerificationStatus)VerificationStatusInt; }
            set
            {
                if (VerificationStatusInt != (int)value)
                    VerificationStatusInt = (int)value;
            }
        }

        public virtual string GetLabel()
        {
            return string.Empty;
        }

        public static Reporter CreateMinistryUser()
        {
            var reporter = new Reporter()
            {
                ReporterType = enReporterType.Ministry,                
                IsActivated = true                
            };

            return reporter;
        }
    }
}
