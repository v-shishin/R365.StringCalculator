using R365.StringCalculator.Interfaces;
using System;

namespace R365.StringCalculator.Services
{
    public class InputParser : IInputParser
    {
        const char DEFAULT_DELIMITER = ',';
        const int MAX_NUMBER_OF_NUMBERS_ACCEPTED = 2;

        public Tuple<int, int> ParseNumbers(string inputStr)
        {
            // if input value is null empty return a tuple with zeros
            if (string.IsNullOrWhiteSpace(inputStr))
            {
                return Tuple.Create(0, 0);
            }

            string[] splitted = inputStr.Split(DEFAULT_DELIMITER);
            // make sure number of values provided does not exceed allowed amount
            if (splitted.Length > MAX_NUMBER_OF_NUMBERS_ACCEPTED)
            {
                throw new ApplicationException($"Maximum of {MAX_NUMBER_OF_NUMBERS_ACCEPTED} numbers accepted");
            }

            // convert first number
            int value1 = ConvertToNumber(splitted[0]);
            // convert second number, but only if exactly two values provided
            int value2 = (splitted.Length == MAX_NUMBER_OF_NUMBERS_ACCEPTED) ? ConvertToNumber(splitted[1]) : 0;

            return Tuple.Create(value1, value2);
        }

        private static int ConvertToNumber(string str)
        {
            return int.TryParse(str, out int result) ? result : 0;
        }
    }
}
