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
        /// Regular expression representing input string in the format "//[{multiple_characters_delimiter}]\n{numbers}"
        /// </summary>
        private static readonly Regex INPUT_PARSING_REGEXP_MANY_CHARS_DELIMETER = new Regex("^//\\[(?<" + DELIMETER_GROUP_NAME + ">.+)\\]\n(?<" + INPUT_NUMBERS_GROUP_NAME + ">.+)", RegexOptions.Singleline);

        public IEnumerable<int> ParseNumbers(string inputStr)
        {
            // if input value is null or empty return empty collection
            if (string.IsNullOrWhiteSpace(inputStr))
            {
                return new List<int>();
            }

            PreParsedInput parsedInput = PreParseInput(inputStr);
            string[] delimeters = GetAllDelimeters(parsedInput.CustomDelimeter);

            return parsedInput.NumbersString
                .Split(delimeters, StringSplitOptions.None)
                .Select(x => ConvertToNumber(x));
        }

        private static int ConvertToNumber(string str)
        {
            return int.TryParse(str, out int result) ? result : 0;
        }

        private static string[] GetAllDelimeters(string customDelimeter)
        {
            if (string.IsNullOrEmpty(customDelimeter))
            {
                return DELIMETERS;
            }
            return DELIMETERS.Append(customDelimeter).ToArray();
        }

        private static PreParsedInput PreParseInput(string inputStr)
        {
            var result = new PreParsedInput();
            Match matchSingleCharacterDelimeter = INPUT_PARSING_REGEXP_SINGLE_CHAR_DELIMETER.Match(inputStr);
            if (matchSingleCharacterDelimeter.Success)
            {
                result.CustomDelimeter = matchSingleCharacterDelimeter.Groups[DELIMETER_GROUP_NAME].Value;
                result.NumbersString = matchSingleCharacterDelimeter.Groups[INPUT_NUMBERS_GROUP_NAME].Value;
            }
            else
            {
                Match matchManyCharacterDelimeter = INPUT_PARSING_REGEXP_MANY_CHARS_DELIMETER.Match(inputStr);
                if (matchManyCharacterDelimeter.Success)
                {
                    result.CustomDelimeter = matchManyCharacterDelimeter.Groups[DELIMETER_GROUP_NAME].Value;
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
        /// Inner class representing pre-parsed input format that divides user input into Custom Delimeter and a string containing input numbers
        /// </summary>
        class PreParsedInput
        {
            public string CustomDelimeter { get; set; }
            public string NumbersString { get; set; }
        }
    }
}
