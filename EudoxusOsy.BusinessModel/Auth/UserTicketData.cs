using System;
using System.Globalization;

namespace EudoxusOsy.BusinessModel
{
    public class UserTicketData
    {
        public int ReporterID { get; set; }
        public string ContactName { get; set; }
        public DateTime? PasswordLastChangedAt { get; set; }

        public string ToCookie()
        {
            return string.Format("{0}|{1}|{2}", ReporterID.ToString(), ContactName, PasswordLastChangedAt.HasValue ? PasswordLastChangedAt.Value.ToString("yyyyMMdd HH:mm:ss") : string.Empty);
        }

        public static UserTicketData FromCookie(string cookieValue)
        {
            var data = new UserTicketData();
            if (!string.IsNullOrEmpty(cookieValue))
            {
                var values = cookieValue.Split('|');
                if (values.Length >= 1)
                    data.ReporterID = int.Parse(values[0]);
                if (values.Length >= 2)
                    data.ContactName = values[1];
                if (values.Length >= 3)
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(values[2], "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                        data.PasswordLastChangedAt = dt;
                }
            }
            return data;
        }
    }
}
