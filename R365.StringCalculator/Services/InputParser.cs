using R365.StringCalculator.Constants;
using R365.StringCalculator.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace R365.StringCalculator.Services
{
    public class InputParser : IInputParser
    {
        readonly static char[] DELIMETERS = new char[] { InputDelimeters.DEFAULT_DELIMITER, InputDelimeters.NEWLINE_DELIMETER };

        const string DELIMETER_GROUP_NAME = "delimeter";
        const string INPUT_NUMBERS_GROUP_NAME = "input";
        /// <summary>
        /// Regular expression representing input string in the format "//{single_char_delimiter}\n{numbers}"
        /// </summary>
        private static readonly Regex INPUT_PARSING_REGEXP = new Regex("^//(?<"+ DELIMETER_GROUP_NAME + ">.)\n(?<"+ INPUT_NUMBERS_GROUP_NAME + ">.+)", RegexOptions.Singleline);

        public IEnumerable<int> ParseNumbers(string inputStr)
        {
            // if input value is null or empty return empty collection
            if (string.IsNullOrWhiteSpace(inputStr))
            {
                return new List<int>();
            }

            PreParsedInput parsedInput = PreParseInput(inputStr);
            char[] delimeters = GetAllDelimeters(parsedInput.CustomDelimeter);

            return parsedInput.NumbersString
                .Split(delimeters)
                .Select(x => ConvertToNumber(x));
        }

        private static int ConvertToNumber(string str)
        {
            return int.TryParse(str, out int result) ? result : 0;
        }

        private static char[] GetAllDelimeters(char? customDelimeter)
        {
            if (!customDelimeter.HasValue)
            {
                return DELIMETERS;
            }
            return DELIMETERS.Append(customDelimeter.Value).ToArray();
        }

        private static PreParsedInput PreParseInput(string inputStr)
        {
            var result = new PreParsedInput();
            Match match = INPUT_PARSING_REGEXP.Match(inputStr);
            if (match.Success)
            {
                // Custom delimeter is a single character
                result.CustomDelimeter = match.Groups[DELIMETER_GROUP_NAME].Value[0];
                result.NumbersString = match.Groups[INPUT_NUMBERS_GROUP_NAME].Value;
            }
            else
            {
                result.NumbersString = inputStr;
            }
            return result;
        }

        /// <summary>
        /// Inner class representing pre-parsed input format that divides user input into Custom Delimeter and a string containing input numbers
        /// </summary>
        class PreParsedInput
        {
            public char? CustomDelimeter { get; set; }
            public string NumbersString { get; set; }
        }
    }
}
