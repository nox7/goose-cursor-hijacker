using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace GooseWPF.Classes
{
    internal class MouseDragTaskDispatcher
    {

        public static void Run(object? source, EventArgs e)
        {

            // Don't run if there is a Goose loop running already
            if (LoopStateHandler.isLooping)
            {
                return;
            }

            Random rnd = new Random();
            if (rnd.Next(1,3) == 1)
            {
                // Run Goose
                Debug.WriteLine("Goose!");
                StartLoop();
            }
            else
            {
                Debug.WriteLine("No goose");
            }
        }

        public static void StartLoop()
        {
            Debug.WriteLine("Called");

            // Store current time
            DateTime startTime = DateTime.Now;
            long startUnixTimeMilliseconds = ((DateTimeOffset)startTime).ToUnixTimeMilliseconds();
            LoopStateHandler.startTimeMilliseconds = startUnixTimeMilliseconds;

            Random rnd = new Random();
            double totalScreenWidth = SystemParameters.VirtualScreenWidth;
            double totalScreenHeight = SystemParameters.VirtualScreenHeight;
            int offsetX = rnd.Next(-300, 300);
            int offsetY = rnd.Next(-300, 300);

            // Get the cursor's current position
            POINT lpPoint = MouseHelper.GetCursorPosition();
            int currentCursorX = lpPoint.X;
            int currentCursorY = lpPoint.Y;

            // Put the Goose at the mouse
            LoopStateHandler.window.Top = currentCursorX;
            LoopStateHandler.window.Left = currentCursorY;

            LoopStateHandler.startX = currentCursorX;
            LoopStateHandler.startY = currentCursorY;

            int randomLocationX = currentCursorX + offsetX;
            int randomLocationY = currentCursorY + offsetY;

            // Clamp them so they don't go off screen
            randomLocationX = Math.Clamp(randomLocationX, 0, (int)totalScreenWidth);
            randomLocationY = Math.Clamp(randomLocationY, 0, (int)totalScreenHeight);

            LoopStateHandler.destinationX = randomLocationX;
            LoopStateHandler.destinationY = randomLocationY;

            Debug.WriteLine("Screen resolution: " + totalScreenWidth + "x" + totalScreenHeight);
            Debug.WriteLine("Random location picked: " + randomLocationX + "x" + randomLocationY);

            // Seconds
            long totalTimeSeconds = rnd.Next(5, 15);
            long totalTimeMilliseconds = totalTimeSeconds * 1000;
            LoopStateHandler.goalTimeEllapsedMilliseconds = totalTimeMilliseconds;

            Debug.WriteLine("Random time chosen: " + totalTimeSeconds + " seconds.");

            LoopStateHandler.window.Show();
            LoopStateHandler.window.Focus();
            LoopStateHandler.isLooping = true;
            LoopStateHandler.window.Visibility = Visibility.Visible;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Poll);
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Start();

            Debug.WriteLine("Timer started.");
        }

        public static void Poll(object? source, EventArgs e)
        {

            int gooseXOffset = 50;
            int gooseYOffset = 40;

            if (!LoopStateHandler.isLooping)
            {
                return;
            }

            Debug.WriteLine("Timer polled.");

            DateTime currentTime = DateTime.Now;
            long currentUnixMilliseconds = ((DateTimeOffset)currentTime).ToUnixTimeMilliseconds();
            long millisecondsElapsed = currentUnixMilliseconds - LoopStateHandler.startTimeMilliseconds;

            float percentCompleted = (float)millisecondsElapsed / (float)LoopStateHandler.goalTimeEllapsedMilliseconds;
            int newXCoordinate = (int) Lerp((float)LoopStateHandler.startX, (float)LoopStateHandler.destinationX, percentCompleted);
            int newYCoordinate = (int) Lerp((float)LoopStateHandler.startY, (float)LoopStateHandler.destinationY, percentCompleted);

            Debug.WriteLine("Percent complete " + percentCompleted);
            Debug.WriteLine("Setting mouse to " + newXCoordinate + "x" + newYCoordinate);

            MouseHelper.SetCursorPosition(newXCoordinate + gooseXOffset, newYCoordinate + gooseYOffset);
            LoopStateHandler.window.Left = newXCoordinate;
            LoopStateHandler.window.Top = newYCoordinate;

            if (percentCompleted >= 1)
            {
                LoopStateHandler.window.Hide();
                LoopStateHandler.isLooping = false;
                LoopStateHandler.window.Visibility = Visibility.Hidden;
            }
        }

        private static float Lerp(float startPosition, float endPosition, float percentCompleted)
        {
            return startPosition + (endPosition - startPosition) * percentCompleted;
        }
    }
}
