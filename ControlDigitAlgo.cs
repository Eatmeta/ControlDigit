using System;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
        // Вспомогательные методы-расширения поместите в этот класс.
        // Они должны быть понятны и потенциально полезны вне контекста задачи расчета контрольных разрядов.
    }

    public static class ControlDigitAlgo
    {
        public static int Upc(long number)
        {
            var sum = 0;
            var factor = 3;
            do
            {
                var digit = (int)(number % 10);
                sum += factor * digit;
                factor = 4 - factor;
                number /= 10;

            }
            while (number > 0);

            var result = sum % 10;
            if (result != 0)
                result = 10 - result;
            return result;
        }

        public static int Isbn10(long number)
        {
            throw new NotImplementedException();
        }

        public static int Luhn(long number)
        {
            throw new NotImplementedException();
        }
    }
}
