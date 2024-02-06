using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAnnotationsValidationExtensions.Helper
{
    public static class Validator
    {
        private static readonly int[] INN12N2Factors = new[] { 7, 2, 4, 10, 3, 5, 9, 4, 6, 8, 0 };
        private static readonly int[] INN12N1Factors = new[] { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };
        private static readonly int[] INN10N1Factors = new[] { 2, 4, 10, 3, 5, 9, 4, 6, 8 };
        public static bool IsValidINN(string inn)
        {
            return inn?.Length switch
            {
                10 => Validate10DigitINN(inn),
                12 => Validate12DigitINN(inn),
                _ => false
            };
        }
        static bool Validate10DigitINN(string inn)
        {
            int sum = 0;

            for (int i = 0; i < 9; i++)
            {
                sum += (Convert.ToInt32(inn[i].ToString())) * INN10N1Factors[i];
            }

            int controlNumber = sum % 11 % 10;
            return (Convert.ToInt32(inn[9].ToString())) == controlNumber;
        }

        static bool Validate12DigitINN(string inn)
        {
            int sum1 = 0;
            int sum2 = 0;

            for (int i = 0; i <= 10; i++)
            {
                sum1 += (Convert.ToInt32(inn[i].ToString())) * INN12N2Factors[i];
                sum2 += (Convert.ToInt32(inn[i].ToString())) * INN12N1Factors[i];
            }

            int controlNumber1 = sum1 % 11 % 10;
            int controlNumber2 = sum2 % 11 % 10;

            return (Convert.ToInt32(inn[10].ToString())) == controlNumber1 && (Convert.ToUInt32(inn[11].ToString())) == controlNumber2;
        }
    }
}
