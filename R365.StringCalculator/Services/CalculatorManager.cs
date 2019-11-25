using R365.StringCalculator.Constants;
using R365.StringCalculator.Interfaces;
using System;
using System.Text;

namespace R365.StringCalculator.Services
{
    public class CalculatorManager
    {
        private readonly ICalculator calculator;
        private readonly IConsoleWrapper console;

        public CalculatorManager(ICalculator calculator, IConsoleWrapper console)
        {
            this.calculator = calculator;
            this.console = console;
        }

        public void Execute()
        {
            if (this.console == null)
            {
                throw new ApplicationException("Console instance was not initialized");
            }

            try
            {
                VerifyCalculatorInitialized();
                console.WriteLine("Enter numbers separated by commas or new line (CTRL+ENTER).");
                console.WriteLine("When done, press ENTER.");

                string input = GetUserInput();
                var result = calculator.Calculate(input);

                console.WriteLine($"Result: {result.Result}");
            }
            catch (Exception ex)
            {
                console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Reads user input and returns a string of entered characters
        /// </summary>
        /// <returns></returns>
        private string GetUserInput()
        {
            // StringBuilder will store user entered characters
            var sb = new StringBuilder();
            // Read user input until Enter is pressed
            while (true)
            {
                var k = console.ReadKey();
                // New line character '\n' in Windows is entered when pressing CTRL+ENTER
                // Condition below is necessary to distinguish it from pressing just ENTER or SHIFT+ENTER
                if (k.Key == ConsoleKey.Enter && k.KeyChar != InputDelimeters.NEWLINE_DELIMETER)
                {
                    // Go to the next line to prevent overwriting of the last entered values when printing result later 
                    console.WriteLine();
                    break;
                }
                sb.Append(k.KeyChar);
            }
            string input = sb.ToString();
            return input;
        }

        private void VerifyCalculatorInitialized()
        {
            if (this.calculator == null)
            {
                throw new ApplicationException("Calculator instance was not initialized");
            }
        }
    }
}
