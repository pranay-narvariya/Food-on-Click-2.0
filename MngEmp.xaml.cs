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
    /// Interaction logic for EmpManagement.xaml
    /// </summary>
    public partial class MngEmp : Page
    {
        SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBconn"].ToString());
        private DataSet ds;
        private SqlDataAdapter sda;
        private SqlDataAdapter sda1;
        private SqlDataAdapter sda2;

        public MngEmp()
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
                dataGrid1.ItemsSource = ds.Tables["Employee"].DefaultView;

                SqlCommand dataCommand1 = new SqlCommand("GetRole", myConnection);
                dataCommand1.CommandType = CommandType.StoredProcedure;

                dataCommand1.Parameters.AddWithValue("@usertype", Home.userType);
                sda1 = new SqlDataAdapter(dataCommand1);
                sda1.Fill(ds, "Role");

                comboBox1.ItemsSource = ds.Tables["Role"].DefaultView;
                comboBox1.DisplayMemberPath = ds.Tables["Role"].Columns["designation"].ToString();
                comboBox1.SelectedValuePath = ds.Tables["Role"].Columns["id"].ToString();

                comboBox2.ItemsSource = ds.Tables["Role"].DefaultView;
                comboBox2.DisplayMemberPath = ds.Tables["Role"].Columns["designation"].ToString();
                comboBox2.SelectedValuePath = ds.Tables["Role"].Columns["id"].ToString();
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
                SqlCommand dataCommand = new SqlCommand("Searchemp", myConnection);
                dataCommand.CommandType = CommandType.StoredProcedure;

                dataCommand.Parameters.AddWithValue("@Name", searchTextBox.Text.Trim());
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["id"].ToString());
            if (MessageBox.Show("Are you want to delete the User?", "Food on Click", MessageBoxButton.YesNo).ToString() == "Yes")
            {
                try
                {
                    myConnection.Open();
                    SqlCommand dataCommand = new SqlCommand("UPDATE employee SET active = 0 WHERE id = @id", myConnection);

                    dataCommand.Parameters.AddWithValue("@id", id);

                    dataCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A UserID is present in login corresponding this Employee!", "Food on Click");
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    myConnection.Close();
                }
            }
            this.NavigationService.Refresh();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if(textBox1.Text.Trim().Length == 0)
            {
                MessageBox.Show("Enter Employee Name!", "Food on Click");
                textBox1.Focus();
                return;
            }
            else if (textBox2.Text.Trim().Length == 0)
            {
                MessageBox.Show("Enter Address!", "Food on Click");
                textBox2.Focus();
                return;
            }
            else if (textBox3.Text.Trim().Length == 0)
            {
                MessageBox.Show("Enter Mobile No.!", "Food on Click");
                textBox3.Focus();
                return;
            }
            else if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Select a Role!", "Food on Click");
                comboBox1.Focus();
                return;
            }

            try
            {
                myConnection.Open();
                SqlCommand dataCommand = new SqlCommand("INSERT INTO employee(name, address, mobile_no, role) VALUES (@name, @address, @mobile, @type)", myConnection);

                dataCommand.Parameters.AddWithValue("@name", textBox1.Text);
                dataCommand.Parameters.AddWithValue("@address", textBox2.Text);
                dataCommand.Parameters.AddWithValue("@mobile", textBox3.Text);
                dataCommand.Parameters.AddWithValue("@type", comboBox1.SelectedValue);

                dataCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                myConnection.Close();
                MessageBox.Show("New Employee successfully added!", "Food on Click");
            }

            this.NavigationService.Refresh();
        }

        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (((DataRowView)dataGrid1.SelectedItem).Row["name"].ToString().Trim() == "")
            {
                MessageBox.Show("Enter Employee Name!", "Food on Click");
                return;
            }
            else if (((DataRowView)dataGrid1.SelectedItem).Row["address"].ToString().Trim() == "")
            {
                MessageBox.Show("Enter Address!", "Food on Click");
                return;
            }
            else if (((DataRowView)dataGrid1.SelectedItem).Row["mobile_no"].ToString().Trim() == "")
            {
                MessageBox.Show("Enter Mobile No.!", "Food on Click");
                return;
            }
            else if (((DataRowView)dataGrid1.SelectedItem).Row["role"].ToString().Trim() == "")
            {
                MessageBox.Show("Select a Role!", "Food on Click");
                return;
            }

            int id = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["id"].ToString());
            int mobile = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["mobile_no"].ToString());
            int role = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["role"].ToString());

            try
            {
                myConnection.Open();
                SqlCommand dataCommand = new SqlCommand("UPDATE employee SET name = @name, address = @address, mobile_no = @mobile, role = @role WHERE id = @id", myConnection);

                dataCommand.Parameters.AddWithValue("@name", ((DataRowView)dataGrid1.SelectedItem).Row["name"].ToString());
                dataCommand.Parameters.AddWithValue("@address", ((DataRowView)dataGrid1.SelectedItem).Row["address"].ToString());
                dataCommand.Parameters.AddWithValue("@mobile", mobile);
                dataCommand.Parameters.AddWithValue("@role", role);
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
            MessageBox.Show("Employee data successfully edited!", "Food on Click");
        }

        private void dataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
