using ff14bot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace DFAlert
{
    static class Log
    {
        public static void Print(string input, Color color)
        {
            Logging.Write(color, $"[DFAlert] {input}");
        }

        public static void Print(string input)
        {
            Print(input, Colors.Red);
        }
    }
}
