using R365.StringCalculator.Constants;
using R365.StringCalculator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace R365.StringCalculator.Services
{
    public class InputParser : IInputParser
    {
        readonly static string[] DELIMETERS = new string[] { InputDelimeters.DEFAULT_DELIMITER.ToString(), InputDelimeters.NEWLINE_DELIMETER.ToString() };

        const string DELIMETER_GROUP_NAME = "delimeter";
        const string INPUT_NUMBERS_GROUP_NAME = "input";

        /// <summary>
        /// Regular expression representing input string in the format "//{single_char_delimiter}\n{numbers}"
        /// </summary>
        private static readonly Regex INPUT_PARSING_REGEXP_SINGLE_CHAR_DELIMETER = new Regex("^//(?<" + DELIMETER_GROUP_NAME + ">.)\n(?<" + INPUT_NUMBERS_GROUP_NAME + ">.+)", RegexOptions.Singleline);

        /// <summary>
        /// Regular expression representing input string in the format "//[{delimiter1}][{delimiter2}]...\n{numbers}"
        /// </summary>
        private static readonly Regex INPUT_PARSING_REGEXP_MANY_DELIMETERS = new Regex("^//(?<" + DELIMETER_GROUP_NAME + ">\\[.+\\])\n(?<" + INPUT_NUMBERS_GROUP_NAME + ">.+)", RegexOptions.Singleline);

        public IEnumerable<int> ParseNumbers(string inputStr)
        {
            // if input value is null or empty return empty collection
            if (string.IsNullOrWhiteSpace(inputStr))
            {
                return new List<int>();
            }

            PreParsedInput parsedInput = PreParseInput(inputStr);
            string[] delimeters = GetAllDelimeters(parsedInput.CustomDelimeters);

            return parsedInput.NumbersString
                .Split(delimeters, StringSplitOptions.None)
                .Select(x => ConvertToNumber(x));
        }

        private static int ConvertToNumber(string str)
        {
            return int.TryParse(str, out int result) ? result : 0;
        }

        private static string[] GetAllDelimeters(string[] customDelimeters)
        {
            if (customDelimeters == null)
            {
                return DELIMETERS;
            }
            return DELIMETERS.Concat(customDelimeters).ToArray();
        }

        private static PreParsedInput PreParseInput(string inputStr)
        {
            var result = new PreParsedInput();
            Match matchSingleCharacterDelimeter = INPUT_PARSING_REGEXP_SINGLE_CHAR_DELIMETER.Match(inputStr);
            if (matchSingleCharacterDelimeter.Success)
            {
                result.CustomDelimeters = new string[] { matchSingleCharacterDelimeter.Groups[DELIMETER_GROUP_NAME].Value };
                result.NumbersString = matchSingleCharacterDelimeter.Groups[INPUT_NUMBERS_GROUP_NAME].Value;
            }
            else
            {
                Match matchManyCharacterDelimeter = INPUT_PARSING_REGEXP_MANY_DELIMETERS.Match(inputStr);
                if (matchManyCharacterDelimeter.Success)
                {
                    // string with a list of delimeters in a format "[{delimiter1}][{delimiter2}]..[{delimiterN}]"
                    string delimetersString = matchManyCharacterDelimeter.Groups[DELIMETER_GROUP_NAME].Value;
                    // split it to individual delimeters
                    // option StringSplitOptions.RemoveEmptyEntries is used to remove empty strings
                    string[] delimeters = delimetersString.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                    result.CustomDelimeters = delimeters;
                    result.NumbersString = matchManyCharacterDelimeter.Groups[INPUT_NUMBERS_GROUP_NAME].Value;
                }
                else
                {
                    result.NumbersString = inputStr;
                }
            }
            return result;
        }

        /// <summary>
        /// Inner class representing pre-parsed input format that divides user input into a list of Custom Delimeters and a string containing input numbers
        /// </summary>
        class PreParsedInput
        {
            public string[] CustomDelimeters { get; set; }
            public string NumbersString { get; set; }
        }
    }
}
