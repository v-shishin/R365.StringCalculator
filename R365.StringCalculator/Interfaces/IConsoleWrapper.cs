using System;
using System.Collections.Generic;
using System.Text;

namespace R365.StringCalculator.Interfaces
{
    public interface IConsoleWrapper
    {
        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to
        ///     the standard output stream.
        /// </summary>
        /// <param name="str"></param>
        void WriteLine(string str="");

        /// <summary>
        /// Obtains the next character or function key pressed by the user
        /// </summary>
        /// <returns></returns>
        public ConsoleKeyInfo ReadKey();
    }
}
