using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public static class ValidationHelper
    {
        public static bool CheckAFM(string cAfm)
        {
            int nExp = 1;

            // Ελεγχος αν περιλαμβάνει μόνο γράμματα
            long nAfm;
            if (!long.TryParse(cAfm, out nAfm))
                return false;

            // Ελεγχος μήκους ΑΦΜ
            cAfm = cAfm.Trim();
            int nL = cAfm.Length;
            if (nL != 9) return false;

            //Υπολογισμός αν το ΑΦΜ είναι σωστό
            int nSum = 0;
            int xDigit = 0;
            int nT = 0;

            for (int i = nL - 2; i >= 0; i--)
            {
                xDigit = int.Parse(cAfm.Substring(i, 1));
                nT = xDigit * (int)(Math.Pow(2, nExp));
                nSum += nT;
                nExp++;
            }

            xDigit = int.Parse(cAfm.Substring(nL - 1, 1));
            nT = nSum / 11;

            int k = nT * 11;
            k = nSum - k;

            if (k == 10)
                k = 0;

            if (xDigit != k)
                return false;

            return true;
        }

        public static bool CheckIBAN(string iban, bool allowOnlyGreekIBANs = true)
        {
            return ValidateIBAN(iban, allowOnlyGreekIBANs) == enIBANValidationResult.IsValid;
        }

        public static enIBANValidationResult ValidateIBAN(string iban, bool allowOnlyGreekIBANs = true)
        {
            if (string.IsNullOrEmpty(iban))
                return enIBANValidationResult.ValueMissing;

            if (iban.Contains(" "))
                return enIBANValidationResult.ContainsSpaces;

            if (iban.Length < 2)
                return enIBANValidationResult.ValueTooSmall;

            iban = iban.ToUpper();

            var countryCode = iban.Substring(0, 2);

            if (allowOnlyGreekIBANs && countryCode != "GR")
            {
                return enIBANValidationResult.NotGreekIBAN;
            }

            int lengthForCountryCode;

            var countryCodeKnown = _ibanLengths.TryGetValue(countryCode, out lengthForCountryCode);
            if (!countryCodeKnown)
            {
                return enIBANValidationResult.CountryCodeNotKnown;
            }

            if (iban.Length < lengthForCountryCode)
                return enIBANValidationResult.ValueTooSmall;

            if (iban.Length > lengthForCountryCode)
                return enIBANValidationResult.ValueTooBig;

            var newIban = iban.Substring(4) + iban.Substring(0, 4);

            newIban = Regex.Replace(newIban, @"\D", match => ((int)match.Value[0] - 55).ToString());

            var checkSum = BigInteger.Parse(newIban) % 97;

            if (checkSum != 1)
                return enIBANValidationResult.ValueFailsModule97Check;

            return enIBANValidationResult.IsValid;
        }

        private static Dictionary<string, int> _ibanLengths = new Dictionary<string, int>
        {
            {"AL", 28},
            {"AD", 24},
            {"AT", 20},
            {"AZ", 28},
            {"BE", 16},
            {"BH", 22},
            {"BA", 20},
            {"BR", 29},
            {"BG", 22},
            {"CR", 21},
            {"HR", 21},
            {"CY", 28},
            {"CZ", 24},
            {"DK", 18},
            {"DO", 28},
            {"EE", 20},
            {"FO", 18},
            {"FI", 18},
            {"FR", 27},
            {"GE", 22},
            {"DE", 22},
            {"GI", 23},
            {"GR", 27},
            {"GL", 18},
            {"GT", 28},
            {"HU", 28},
            {"IS", 26},
            {"IE", 22},
            {"IL", 23},
            {"IT", 27},
            {"KZ", 20},
            {"KW", 30},
            {"LV", 21},
            {"LB", 28},
            {"LI", 21},
            {"LT", 20},
            {"LU", 20},
            {"MK", 19},
            {"MT", 31},
            {"MR", 27},
            {"MU", 30},
            {"MC", 27},
            {"MD", 24},
            {"ME", 22},
            {"NL", 18},
            {"NO", 15},
            {"PK", 24},
            {"PS", 29},
            {"PL", 28},
            {"PT", 25},
            {"RO", 24},
            {"SM", 27},
            {"SA", 24},
            {"RS", 22},
            {"SK", 24},
            {"SI", 19},
            {"ES", 24},
            {"SE", 24},
            {"CH", 21},
            {"TN", 24},
            {"TR", 26},
            {"AE", 23},
            {"GB", 22},
            {"VG", 24}
        };
    }
}