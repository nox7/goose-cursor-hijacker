using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GooseWPF.Classes
{
    internal class LoopStateHandler
    {
        public static Window? window;
        public static bool isLooping = false;

        public static long startTimeMilliseconds = 0;
        public static long goalTimeEllapsedMilliseconds = 0;
        public static int startX = 0;
        public static int startY = 0;
        public static int destinationX = 0;
        public static int destinationY = 0;
    }
}
