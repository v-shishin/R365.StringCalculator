using System;

namespace R365.StringCalculator.Interfaces
{
    public interface IInputParser
    {
        /// <summary>
        /// Tries to extract two numbers from the input string
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        Tuple<int, int> ParseNumbers(string inputStr);
    }
}
