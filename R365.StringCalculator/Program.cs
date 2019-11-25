using R365.StringCalculator.Constants;
using R365.StringCalculator.Services;
using System;
using System.Text;

namespace R365.StringCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var calcManager = new CalculatorManager(new Calculator(new InputParser()), new ConsoleWrapper());
            calcManager.Execute();
        }


    }
}
