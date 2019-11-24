using System;
using System.Collections.Generic;

namespace R365.StringCalculator.Interfaces
{
    public interface IInputParser
    {
        /// <summary>
        /// Tries to extract a list of numbers from the input string
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        IEnumerable<int> ParseNumbers(string inputStr);
    }
}
