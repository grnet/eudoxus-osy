using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public static class CodeGenerationHelper
    {
        public static string GenerateVerificationCode()
        {
            return RandomNumber(10000000, 99999999);
        }

        public static string GenerateSubmissionCode()
        {
            string code = "";
            int counter = 1;
            int sum = 0;

            int random1;
            int random2;
            int checkSum;

            int firstDigit = Config.IsPilotSite
                                ? 1
                                : GenerateRandomNumber(2, 9);

            while (counter <= 3)
            {
                random1 = (counter == 1) ? firstDigit : GenerateRandomNumber();
                random2 = GenerateRandomNumber();
                sum = sum + (random1 * 10 + random2) * counter;
                code = code + random1.ToString() + random2.ToString();

                counter++;
            }

            checkSum = 100 - (sum % 100);

            if (checkSum == 100)
            {
                checkSum = 0;
            }

            if (checkSum < 10)
            {
                code = code + "0";
            }

            code = code + checkSum.ToString();

            return code;
        }

        private static int GenerateRandomNumber()
        {
            var byt = new byte[4];
            var rngCrypto = new RNGCryptoServiceProvider();

            rngCrypto.GetBytes(byt);
            int result = BitConverter.ToInt32(byt, 0);

            return new Random(result).Next(0, 9);
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            var byt = new byte[4];
            var rngCrypto = new RNGCryptoServiceProvider();

            rngCrypto.GetBytes(byt);
            int result = BitConverter.ToInt32(byt, 0);

            return new Random(result).Next(min, max);
        }

        private static string RandomNumber(int min, int max)
        {
            var random = new Random();

            return random.Next(min, max).ToString();
        }

        public static bool IsSubmissionCodeValid(string uniqueCode)
        {
            //Αν ο κωδικός δεν έχει μήκος 8 ψηφία επέστρεψε false
            if (uniqueCode.Length != 8)
                return false;

            int[] uniqueCodeDigits = new int[8];

            //Παίρνουμε τους χαρακτήρες του 12-ψήφιου Κωδικού σε ένα char array
            char[] uniqueCodeArray = uniqueCode.ToArray();

            //Μετατροπή του char array σε int array
            for (int i = 0; i < 8; i++)
            {
                //Αν έστω κι ένας χαρακτήρας δεν είναι ψηφίο επέστρεψε false
                if (!int.TryParse(uniqueCodeArray[i].ToString(), out uniqueCodeDigits[i]))
                {
                    return false;
                }
            }

            //Υπολογισμός του checkSum με βάση τα 10 πρώτα ψηφία
            string calculatedCheckSum = string.Empty;

            int counter = 1;
            int x = 0;
            int y = 1;
            int sum = 0;

            while (counter <= 3)
            {
                sum += (uniqueCodeDigits[x] * 10 + uniqueCodeDigits[y]) * counter;

                x += 2;
                y += 2;
                counter++;
            }

            int checkSum = 100 - (sum % 100);

            if (checkSum == 100)
            {
                checkSum = 0;
            }

            if (checkSum < 10)
            {
                calculatedCheckSum += "0";
            }

            calculatedCheckSum += checkSum.ToString();

            //Το checkSum που έχει ο κωδικός που εισήγαγε ο χρήστης
            string givenCheckSum = uniqueCodeDigits[6].ToString() + uniqueCodeDigits[7].ToString();

            //Έλεγχος ότι τα δύο checkSums ταιριάζουν
            return calculatedCheckSum == givenCheckSum;
        }
    }
}
