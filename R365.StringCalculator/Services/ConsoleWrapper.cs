using R365.StringCalculator.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace R365.StringCalculator.Services
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey(false);
        }

        public void WriteLine(string str = "")
        {
            Console.WriteLine(str);
        }
    }
}
