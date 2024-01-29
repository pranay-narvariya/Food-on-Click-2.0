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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace Food
{
    /// <summary>
    /// Interaction logic for MngLogin.xaml
    /// </summary>
    public partial class MngLogin : Page
    {
        SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBconn"].ToString());
        private DataSet ds;
        private SqlDataAdapter sda;
        private SqlDataAdapter sda1;
        private SqlDataAdapter sda2;

        public MngLogin()
        {
            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand dataCommand = new SqlCommand("GetEmp", myConnection);
                dataCommand.CommandType = CommandType.StoredProcedure;

                dataCommand.Parameters.AddWithValue("@usertype", Home.userType);

                sda = new SqlDataAdapter(dataCommand);
                ds = new DataSet();
                sda.Fill(ds, "Employee");

                foreach (DataRow row in ds.Tables["Employee"].Rows)
                {
                    row["name"] = row["name"].ToString().ToUpper() +" - "+ row["designation"].ToString().ToUpper();
                }

                comboBox1.ItemsSource = ds.Tables["Employee"].DefaultView;
                comboBox1.DisplayMemberPath = ds.Tables["Employee"].Columns["name"].ToString();
                comboBox1.SelectedValuePath = ds.Tables["Employee"].Columns["id"].ToString();

                SqlCommand dataCommand1 = new SqlCommand("GetLogin", myConnection);
                dataCommand1.CommandType = CommandType.StoredProcedure;

                dataCommand1.Parameters.AddWithValue("@usertype", Home.userType);
                sda1 = new SqlDataAdapter(dataCommand1);
                sda1.Fill(ds, "Login");

                dataGrid1.ItemsSource = ds.Tables["Login"].DefaultView;
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

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                SqlCommand dataCommand = new SqlCommand("SearchLogin", myConnection);
                dataCommand.CommandType = CommandType.StoredProcedure;

                dataCommand.Parameters.AddWithValue("@username", searchTextBox.Text.Trim());
                dataCommand.Parameters.AddWithValue("@usertype", Home.userType);

                sda2 = new SqlDataAdapter(dataCommand);
                ds = new DataSet();
                sda2.Fill(ds, "Search");
                dataGrid1.ItemsSource = ds.Tables["Search"].DefaultView;
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

        private void dataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Select a Employee!", "Food on Click");
                comboBox1.Focus();
                return;
            }
            else if (textBox3.Text.Trim().Length == 0)
            {
                MessageBox.Show("Enter a username!", "Food on Click");
                textBox3.Focus();
                return;
            }
            else if (textBox4.Text.Trim().Length == 0)
            {
                MessageBox.Show("Enter Password!", "Food on Click");
                textBox4.Focus();
                return;
            }
            else if (textBox5.Text.Trim().Length == 0)
            {
                MessageBox.Show("Re-Enter Mobile Password!", "Food on Click");
                textBox5.Focus();
                return;
            }
            else if (textBox4.Text != textBox5.Text)
            {
                MessageBox.Show("Passwords Do not Match!", "Food on Click");
                textBox4.Clear();
                textBox5.Clear();
                textBox4.Focus();
                return;
            }

            try
            {
                myConnection.Open();
                SqlCommand dataCommand = new SqlCommand("INSERT INTO login(login_username, login_password, emp_id) VALUES (@username, @password, @emp_id)", myConnection);

                dataCommand.Parameters.AddWithValue("@username", textBox3.Text);
                dataCommand.Parameters.AddWithValue("@password", textBox5.Text);
                dataCommand.Parameters.AddWithValue("@emp_id", comboBox1.SelectedValue);

                dataCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                myConnection.Close();
                MessageBox.Show("New login successfully added!", "Food on Click");
            }

            this.NavigationService.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["id"].ToString());

            if (MessageBox.Show("Are you want to delete the User from Login?", "Food on Click", MessageBoxButton.YesNo).ToString() == "Yes")
            {
                try
                {
                    myConnection.Open();
                    SqlCommand dataCommand = new SqlCommand("DELETE FROM login WHERE id = @id", myConnection);

                    dataCommand.Parameters.AddWithValue("@id", id);

                    dataCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    myConnection.Close();
                    MessageBox.Show("Employee successfully deleted!", "Food on Click");
                }
            }
        }

        private void textBox3_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand dataCommand = new SqlCommand("SELECT login.login_username FROM login WHERE login.login_username LIKE ''+ @username +''", myConnection);

                dataCommand.Parameters.AddWithValue("@username", textBox3.Text.Trim().ToLower());

                sda2 = new SqlDataAdapter(dataCommand);
                ds = new DataSet();
                sda2.Fill(ds, "Repeat");
                if (ds.Tables["Repeat"].Rows.Count >=1)
                {
                    MessageBox.Show("Username already exists. Select a new username!", "Food on Click");
                    textBox3.Clear();
                    textBox3.Focus();
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

        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (((DataRowView)dataGrid1.SelectedItem).Row["login_username"].ToString().Trim() == "")
            {
                MessageBox.Show("Enter Employee Username!", "Food on Click");
                return;
            }
            else if (((DataRowView)dataGrid1.SelectedItem).Row["login_password"].ToString().Trim() == "")
            {
                MessageBox.Show("Enter Password!", "Food on Click");
                return;
            }

            int id = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["id"].ToString());

            try
            {
                myConnection.Open();
                SqlCommand dataCommand = new SqlCommand("UPDATE login SET login_username = @username, login_password = @password WHERE id = @id", myConnection);

                dataCommand.Parameters.AddWithValue("@username", ((DataRowView)dataGrid1.SelectedItem).Row["login_username"].ToString());
                dataCommand.Parameters.AddWithValue("@password", ((DataRowView)dataGrid1.SelectedItem).Row["login_password"].ToString());
                dataCommand.Parameters.AddWithValue("@id", id);

                dataCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                myConnection.Close();
            }
        }

        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            MessageBox.Show("Employee login data successfully edited!", "Food on Click");
        }
    }
}
