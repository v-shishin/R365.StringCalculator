using R365.StringCalculator.Interfaces;
using R365.StringCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R365.StringCalculator.Services
{
    /// <summary>
    /// Contains methods for calculating arithmetic operations for values provided in a string
    /// </summary>
    public class Calculator : ICalculator
    {
        private readonly IInputParser inputParser;
        private const int MAX_ALLOWED_NUMBER = 1000;

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
            var numbers = inputParser.ParseNumbers(input);
            VerifyNonNegativeNumbers(numbers);
            int sum = numbers
                .Where(x => x <= MAX_ALLOWED_NUMBER)
                .Sum();
            return new CalculationResult { Result = sum };
        }

        private static void VerifyNonNegativeNumbers(IEnumerable<int> numbers)
        {
            var negativeNumbers = numbers.Where(x => x < 0);
            if (negativeNumbers.Any())
            {
                //  negativeNumbers.Aggregate("")
                var sb = new StringBuilder();
                sb.AppendJoin(", ", negativeNumbers);
                throw new ApplicationException("Negative numbers are not supported. Entered the following negative numbers: " + sb.ToString());
            }
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
