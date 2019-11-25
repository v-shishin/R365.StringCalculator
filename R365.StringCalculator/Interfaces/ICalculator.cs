using R365.StringCalculator.Models;

namespace R365.StringCalculator.Interfaces
{
    public interface ICalculator
    {
        CalculationResult Calculate(string input);
    }
}