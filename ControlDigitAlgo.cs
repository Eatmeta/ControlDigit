using System;
using System.Collections.Generic;
using System.Linq;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
        public static int[] ParseCode(this long extensible)
        {
            return extensible.ToString().Select(c => int.Parse(c.ToString())).ToArray();
        }
        
        public static int GetSumOfDigits(this int extensible)
        {
            var sum = 0;
            while (extensible >= 10)
            {
                sum += extensible % 10;
                extensible /= 10;
            }

            return sum + extensible;
        }

        public static int[] CompleteToIsbn10Length(this int[] extensible)
        {
            var ten = new int[9];
            var counter = 8;
            foreach (var digit in extensible.Reverse())
            {
                ten[counter] = digit;
                counter--;
            }
            return ten;
        }

        public static IEnumerable<int> SelectOdd(this IEnumerable<int> extensible)
        {
            return extensible.Where((d, index) => index % 2 != 0);
        }

        public static IEnumerable<int> SelectEven(this IEnumerable<int> extensible)
        {
            return extensible.Where((d, index) => index % 2 == 0);
        }
        
        public static int SumWithRule(this IEnumerable<int> extensible, Func<int, int, int> func)
        {
            var enumerable = extensible.ToArray();
            return enumerable.Select((d, index) => (10 - index) * d).Sum();
        }
    }

    public class ControlDigitAlgo
    {
        private static int ReturnControlDigitUpc(int extensible)
        {
            return extensible % 10 == 0
                ? 0
                : 10 - extensible % 10;
        }

        private static char ReturnControlDigitIsbn10(int value)
        {
            return value == 10
                ? 'X'
                : (char) (value + 48);
        }

        private static int ReturnControlDigitLuhn(int value)
        {
            return (10 - value % 10) % 10;
        }

        private static int LuhnForOddLength(int[] digits)
        {
            var sumOdd = digits.SelectOdd().Sum();
            var sumEven = digits.SelectEven().Select(d => (d * 2).GetSumOfDigits()).Sum();
            return ReturnControlDigitLuhn(sumOdd + sumEven);
        }

        private static int LuhnForEvenLength(int[] digits)
        {
            var sumEven = digits.SelectEven().Sum();
            var sumOdd = digits.SelectOdd().Select(d => (d * 2).GetSumOfDigits()).Sum();
            return ReturnControlDigitLuhn(sumEven + sumOdd);
        }

        public static int Upc(long number)
        {
            if (number < 10 && number != 0) return (int) (10 - number * 3 % 10);
            const int factor = 3;
            var digits = number.ParseCode();
            var sumOdd = digits.SelectOdd().Sum();
            var sumEven = digits.Take(digits.Length - 1).SelectEven().Sum();
            return ReturnControlDigitUpc(sumOdd * factor + sumEven);
        }

        public static char Isbn10(long number)
        {
            var digits = number.ParseCode().CompleteToIsbn10Length();
            var sum = digits.SumWithRule((d, index) => (10 - index) * d);
            return ReturnControlDigitIsbn10((11 - sum % 11) % 11);
        }

        public static int Luhn(long number)
        {
            var digits = number.ParseCode();
            return digits.Length % 2 == 0
                ? LuhnForEvenLength(digits)
                : LuhnForOddLength(digits);
        }
    }
}