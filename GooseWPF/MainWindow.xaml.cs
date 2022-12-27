using GooseWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GooseWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Goal
            /**
             * Every 3 minutes, a random int will be rolled from 1 to 3.
             * If the random int is 1, then the Goose will grab the mouse for 20 seconds and drag it around the screen.
             */

            this.Hide();
            this.Visibility = Visibility.Hidden;

            LoopStateHandler.window = this;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(MouseDragTaskDispatcher.Run);
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Start();
        }
    }
}
