using R365.StringCalculator.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace R365.StringCalculator.Services
{
    public class InputParser : IInputParser
    {
        const char DEFAULT_DELIMITER = ',';

        public IEnumerable<int> ParseNumbers(string inputStr)
        {
            // if input value is null or empty return empty collection
            if (string.IsNullOrWhiteSpace(inputStr))
            {
                return new List<int>();
            }

            return inputStr
                .Split(DEFAULT_DELIMITER)
                .Select(x => ConvertToNumber(x));
        }

        private static int ConvertToNumber(string str)
        {
            return int.TryParse(str, out int result) ? result : 0;
        }
    }
}
