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
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;

namespace Food
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        int counter = 3;
        SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBconn"].ToString());

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {           
            if (textBox1.Text.Length == 0)  
            {  
                MessageBox.Show("Enter a User Name!", "Food on Click");
                textBox1.Focus();
                return;
            }
            else if (passwordBox1.Password.Length == 0)
            {
                MessageBox.Show("Enter a Password!", "Food on Click");
                passwordBox1.Focus();
                return;
            }
            else
            {
                try
                {
                    var username = textBox1.Text.ToString();
                    var password = passwordBox1.Password.ToString();

                    myConnection.Open();
                    SqlCommand dataCommand = new SqlCommand("Loginchk", myConnection);
                    dataCommand.CommandType = CommandType.StoredProcedure;

                    dataCommand.Parameters.AddWithValue("@username", username);
                    dataCommand.Parameters.AddWithValue("@Password", password);

                    SqlDataReader dataReader = dataCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            this.NavigationService.Navigate(new Home(dataReader.GetString(5), dataReader.GetInt32(6)));
                        }
                    }
                    else
                    {
                        counter--;
                        if (counter == 0)
                        {
                            MessageBox.Show("Maximum 3 attempts are acceptable!", "Food on Click");
                            Application.Current.Shutdown();
                        }
                        MessageBox.Show(counter + " attempts remaining!", "Food on Click");
                        Reset();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    myConnection.Close();
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Reset()
        {
            textBox1.Clear();
            passwordBox1.Clear();
            textBox1.Focus();
        }

        private void passwordBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                button1_Click(this, new RoutedEventArgs());
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                passwordBox1.Focus();
            }
        }
    }
}
