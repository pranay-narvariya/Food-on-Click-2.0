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
    /// Interaction logic for MngTable.xaml
    /// </summary>
    public partial class MngTable : Page
    {
        SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBconn"].ToString());
        private DataSet ds;
        private SqlDataAdapter sda;

        public MngTable()
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

        private void dataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                myConnection.Open();
                SqlCommand dataCommand = new SqlCommand("GetTable", myConnection);
                dataCommand.CommandType = CommandType.StoredProcedure;

                sda = new SqlDataAdapter(dataCommand);
                ds = new DataSet();
                sda.Fill(ds, "Table");
                dataGrid1.ItemsSource = ds.Tables["Table"].DefaultView;

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

        private void status_Click(object sender, RoutedEventArgs e)
        {
            int id = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["table_id"].ToString());
            CheckBox chk = (CheckBox)sender;

            if (chk.IsChecked == true)
            {
                try
                {
                    myConnection.Open();
                    SqlCommand dataCommand = new SqlCommand("UPDATE table_lookup SET active = 1 WHERE table_id = @id", myConnection);

                    dataCommand.Parameters.AddWithValue("@id", id);

                    dataCommand.ExecuteNonQuery();
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
            else if (chk.IsChecked == false)
            {
                try
                {
                    myConnection.Open();
                    SqlCommand dataCommand = new SqlCommand("UPDATE table_lookup SET active = 0 WHERE table_id = @id", myConnection);

                    dataCommand.Parameters.AddWithValue("@id", id);

                    dataCommand.ExecuteNonQuery();
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Trim().Length == 0)
            {
                MessageBox.Show("Enter Table Number!", "Food on Click");
                textBox1.Focus();
                return;
            }
            else if (textBox2.Text.Trim().Length == 0)
            {
                MessageBox.Show("Enter Capacity!", "Food on Click");
                textBox2.Focus();
                return;
            }

            try
            {
                myConnection.Open();
                SqlCommand dataCommand = new SqlCommand("INSERT INTO table_lookup(table_no, capacity) VALUES (@tableno, @capacity)", myConnection);

                dataCommand.Parameters.AddWithValue("@tableno", textBox1.Text);
                dataCommand.Parameters.AddWithValue("@capacity", textBox2.Text);

                dataCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                myConnection.Close();
                MessageBox.Show("New Table successfully added!", "Food on Click");
            }

            this.NavigationService.Refresh();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (((DataRowView)dataGrid1.SelectedItem).Row["capacity"].ToString().Trim() == "")
            {
                MessageBox.Show("Enter Capacity!", "Food on Click");
                return;
            }

            int id = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["table_id"].ToString());

            try
            {
                myConnection.Open();
                SqlCommand dataCommand = new SqlCommand("UPDATE table_lookup SET table_no = @tableno, capacity = @capacity WHERE table_id = @id", myConnection);

                dataCommand.Parameters.AddWithValue("@tableno", ((DataRowView)dataGrid1.SelectedItem).Row["table_no"].ToString());
                dataCommand.Parameters.AddWithValue("@capacity", ((DataRowView)dataGrid1.SelectedItem).Row["capacity"].ToString());
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
            MessageBox.Show("Category successfully edited!", "Food on Click");
        }
    }
}
