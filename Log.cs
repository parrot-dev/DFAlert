using ff14bot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DFAlert
{
    static class Log
    {
        public static void print(string input, Color col)
        {
            Logging.Write(col, string.Format("[DFAlert] {0}", input));
        }

        public static void print(string input)
        {
            print(input, Colors.Red);
        }
    }
}
