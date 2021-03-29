using Imis.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EudoxusOsy.BusinessModel
{
    public static class BusinessHelper
    {
        public static T GetRandom<T>(this List<T> collection)
        {
            var r = new Random();
            return collection[r.Next(collection.Count)];
        }

        public static string GetUnformattedCode(this string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            return code.Trim().Replace(" ", "").Replace("-", "");
        }

        public static string FormattedOrderCode(this string orderCode)
        {
            if (string.IsNullOrEmpty(orderCode))
                return null;

            return orderCode.Substring(0, 4) + "-" +
                   orderCode.Substring(4, 4);
        }

        public static string FormattedVoucherCode(this string voucherCode)
        {
            if (string.IsNullOrEmpty(voucherCode))
                return null;

            return voucherCode.Substring(0, 4) + "-" +
                   voucherCode.Substring(4, 4) + "-" +
                   voucherCode.Substring(8, 4);
        }

        public static string FullNameTrim(string contactName, int maxLength)
        {
            string fullName = string.Empty;

            if (contactName.Length >= maxLength)
            {
                fullName = contactName.SubstringByLength(maxLength - contactName.Length - 1);
            }
            else
            {
                fullName = contactName;
            }

            return fullName;
        }

        public static int GetFileUploadSizeForUser()
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                Configuration.FileUploadExceptionConfigurationSection exception = null;
                foreach (Configuration.FileUploadExceptionConfigurationSection item in Config.FileUpload.Exceptions)
                {
                    if (item.Username == Thread.CurrentPrincipal.Identity.Name)
                    {
                        exception = item;
                        break;
                    }
                }

                if (exception != null)
                    return exception.FileSize;
                else
                    return Config.FileUpload.DefaultFileSize;
            }
            else
                return Config.FileUpload.DefaultFileSize;
        }

        public static string ToHumanReadableFileSize(double byteSize)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (byteSize >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                byteSize = byteSize / 1024;
            }
            return string.Format("{0:0.##} {1}", byteSize, sizes[order]);
        }

        public static string NumberToWords(decimal decimalNumber)
        {
            var number1 = (int)decimalNumber;
            var number2 = (int)((decimalNumber % 1) * 100);

            return NumberToWords(number1) + " ευρώ " + (number2 > 0 ? "και " + NumberToWords(number2) + " λεπτών " : "");
        }

        public static string NumberToWordsGenitive(decimal decimalNumber)
        {
            var number1 = (int)decimalNumber;
            var number2 = (int)((decimalNumber % 1) * 100);

            return NumberToWordsGenitive(number1) + " ευρώ " + (number2 > 0 ? "και " + NumberToWordsGenitive(number2) + " λεπτών " : "");
        }

        public static string NumberToWords(int number, bool hundredsFemale = false)
        {
            if (number == 0)
                return "μηδέν";

            if (number < 0)
                return "μείον " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                var word = number / 1000000 > 1 ? " εκατομμύρια " : "ένα εκατομμύριο ";
                words += (number / 1000000 > 1 ? NumberToWords(number / 1000000) : "") + word;
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                var word = number / 1000 > 1 ? " χιλιάδες " : (hundredsFemale ? " χίλιες " : " χίλια ");
                words += (number / 1000 > 1 ? NumberToWords(number / 1000, true) : "") + word;
                number %= 1000;
            }

            var hundredsMap = new[] { "μηδέν", "εκατό", "διακόσια", "τριακόσια", "τετρακόσια", "πεντακόσια", "εξακόσια", "επτακόσια", "οκτακόσια", "εννιακόσια" };
            var hundredsMapFemale = new[] { "μηδέν", "εκατό", "διακόσιες", "τριακόσιες", "τετρακόσιες", "πεντακόσιες", "εξακόσιες", "επτακόσιες", "οκτακόσιες", "εννιακόσιες" };

            if ((number / 100) > 0)
            {
                var word = hundredsFemale ? hundredsMapFemale[number / 100] + " " : hundredsMap[number / 100] + " ";
                words += word;
                number %= 100;
            }

            if (number > 0)
            {

                var unitsMap = new[] { "μηδέν", "ένα", "δύο", "τρία", "τέσσερα", "πέντε", "έξι", "επτά", "οκτώ", "εννιά", "δέκα", "έντεκα", "δώδεκα" };
                var tensMap = new[] { "μηδέν", "δέκα", "είκοσι", "τριάντα", "σαράντα", "πενήντα", "εξήντα", "εβδομήντα", "ογδόντα", "ενενήντα" };

                if (number < 13)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static string NumberToWordsGenitive(int number, bool hundredsFemale = false)
        {
            if (number == 0)
                return "μηδέν";

            if (number < 0)
                return "μείον " + NumberToWordsGenitive(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                var word = number / 1000000 > 1 ? " εκατομμυρίων " : " εκατομμυρίου ";
                words += (number / 1000000 > 1 ? NumberToWordsGenitive(number / 1000000) : "") + word;
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                var word = number / 1000 > 1 ? " χιλιάδων " : " χιλίων ";
                words += (number / 1000 > 1 ? NumberToWordsGenitive(number / 1000, true) : "") + word;
                number %= 1000;
            }

            var hundredsMap = new[] { "μηδέν", "εκατό", "διακοσίων", "τριακοσίων", "τετρακοσίων", "πεντακοσίων", "εξακοσίων", "επτακοσίων", "οκτακοσίων", "εννιακοσίων" };

            if ((number / 100) > 0)
            {
                var word = hundredsMap[number / 100] + " ";
                words += word;
                number %= 100;
            }

            if (number > 0)
            {

                var unitsMap = new[] { "μηδέν", "ενός", "δύο", "τριών", "τεσσάρων", "πέντε", "έξι", "επτά", "οκτώ", "εννέα", "δέκα", "έντεκα", "δώδεκα" };
                var tensMap = new[] { "μηδέν", "δέκα", "είκοσι", "τριάντα", "σαράντα", "πενήντα", "εξήντα", "εβδομήντα", "ογδόντα", "ενενήντα" };

                if (number < 13)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static void UpdateApplicationDataEntry(string entryName, string entryValue)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var appData = EudoxusOsyCacheManager<ApplicationData>.Current.GetItems().FirstOrDefault(x => x.Name == ApplicationDataNames.ShouldRunComplementReceipts);
                if (appData != null)
                {
                    var existingValue = new ApplicationDataRepository(uow).Load(appData.ID);
                    appData.Value = enYesNo.Yes.GetValue().ToString();
                }
                else
                {
                    var newAppData = new ApplicationData();
                    newAppData.Name = ApplicationDataNames.ShouldRunComplementReceipts;
                    newAppData.Value = enYesNo.Yes.GetValue().ToString();
                    uow.MarkAsNew(newAppData);
                }

                uow.Commit();
            }
            EudoxusOsyCacheManager<ApplicationData>.Current.Refresh();
        }
    }
}