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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public static string userName;
        public static int userType;

        public Home()
        {
            InitializeComponent();
        }

        public Home(string username, int usertype)
        {
            InitializeComponent();

            userName = username;
            userType = usertype;

            setdetail();
            setuser();     
        }

        private void setdetail()
        {
            label1.Content = "Welcome " + userName.ToUpper() + "!";
            label1.FontWeight = FontWeights.Bold;
        }

        private void setuser()
        {
            if (!(userType == 1 || userType == 2))
            {
                houseKeeping.Visibility = Visibility.Collapsed;
                menuManagement.Visibility = Visibility.Collapsed;
                sales.Visibility = Visibility.Collapsed;
            }
        }

        private void MngLogin_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MngLogin());
        }

        private void MngEmp_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MngEmp());
        }

        private void MngTable_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MngTable());
        }

        private void MngCategory_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MngCategory());
        }

        private void MngFoodMenu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MngFoodMenu());
        }

        private void POS_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new POS());
        }

        private void Sales_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Sales());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you want to logout?", "Food on Click", MessageBoxButton.YesNo).ToString() == "Yes")
            {
                this.NavigationService.Navigate(new Login());
            }
        }
    }
}
