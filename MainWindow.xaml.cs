using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Food
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void ReadyToShowDelegate(object sender, EventArgs args);

        public event ReadyToShowDelegate ReadyToShow;

        private DispatcherTimer timer;
        private DispatcherTimer timer1;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(7);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            mainFrames.NavigationService.Navigate(new Login());

            timer1 = new DispatcherTimer();
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += timer_Tick1;
            timer1.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            // This timer imitates a time-consuming operation (or a whole bunch of
            // such operations).
            // When done, it raises a custom event to let the splash screen know that its time is up.

            timer.Stop();

            if (ReadyToShow != null)
            {
                ReadyToShow(this, null);
            }
        }

        void timer_Tick1(object sender, EventArgs e)
        {
            DateTime d;
            d = DateTime.Now;
            lblTime.Content = d.ToLongDateString() + " " + d.ToLongTimeString();
        }

        private void mainFrames_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (mainFrames.Content.ToString() == "Food.Home" || mainFrames.Content.ToString() == "Food.Login")
                {
                    e.Cancel = true;
                }
            }
        }

        private void mainFrames_Navigated(object sender, NavigationEventArgs e)
        {
            if (mainFrames.Content.ToString() != "Food.Login")
            {
                lblTime.Visibility = Visibility.Visible;
            }
        }
    }
}
