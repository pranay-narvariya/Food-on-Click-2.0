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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace Food
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        private MainWindow m_mainWindow;

        private DispatcherTimer splashAnimationTimer;

        public Splash()
        {
            InitializeComponent();

            splashAnimationTimer = new DispatcherTimer();
            splashAnimationTimer.Interval = TimeSpan.FromMilliseconds(500);
            splashAnimationTimer.Start();

            m_mainWindow = new MainWindow();

            m_mainWindow.ReadyToShow += new MainWindow.ReadyToShowDelegate(m_mainWindow_ReadyToShow);
        }

        void m_mainWindow_ReadyToShow(object sender, EventArgs args)
        {
            // When the main window is done with its time-consuming tasks, 
            // hide the splash screen and show the main window.

            #region Animate the splash screen fading

            Storyboard sb = new Storyboard();
            //
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };
            //
            Storyboard.SetTarget(da, this);
            Storyboard.SetTargetProperty(da, new PropertyPath(OpacityProperty));
            sb.Children.Add(da);
            //
            sb.Completed += new EventHandler(sb_Completed);
            //
            sb.Begin();

            #endregion // Animate the splash screen fading
        }

        void sb_Completed(object sender, EventArgs e)
        {
            // When the splash screen fades out, we can show the main window:
            m_mainWindow.Visibility = System.Windows.Visibility.Visible;
            this.Close();
        }
    }
}
