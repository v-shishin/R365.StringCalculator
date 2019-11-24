using R365.StringCalculator.Interfaces;
using R365.StringCalculator.Models;
using System;

namespace R365.StringCalculator.Services
{
    /// <summary>
    /// Contains methods for calculating arithmetic operations for values provided in a string
    /// </summary>
    public class Calculator
    {
        private readonly IInputParser inputParser;

        public Calculator(IInputParser inputParser)
        {
            this.inputParser = inputParser;
        }

        /// <summary>
        /// Calculates Sum of values provided in a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CalculationResult Calculate(string input)
        {
            VerifyParser();
            Tuple<int, int> numbers = inputParser.ParseNumbers(input);
            return new CalculationResult {Result= numbers.Item1 + numbers.Item2 };
        }

        private void VerifyParser()
        {
            if (this.inputParser == null)
            {
                throw new ApplicationException("Instance of InputParser was not provided in the constructor");
            }
        }
    }
}
