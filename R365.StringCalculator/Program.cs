using R365.StringCalculator.Services;
using System;

namespace R365.StringCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Enter input numbers:");
                string input = Console.ReadLine();
                Calculator calc = new Calculator(new InputParser());
                var result = calc.Calculate(input);
                Console.WriteLine($"Result: {result.Result}");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
