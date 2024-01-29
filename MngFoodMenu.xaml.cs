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
    /// Interaction logic for MenuMangement.xaml
    /// </summary>
    public partial class MngFoodMenu : Page
    {
        SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBconn"].ToString());
        private DataSet ds;
        private SqlDataAdapter sda;
        private SqlDataAdapter sda1;
        private SqlDataAdapter sda2;

        public MngFoodMenu()
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
                SqlCommand dataCommand = new SqlCommand("GetFood", myConnection);
                dataCommand.CommandType = CommandType.StoredProcedure;

                sda = new SqlDataAdapter(dataCommand);
                ds = new DataSet();
                sda.Fill(ds, "Food");
                dataGrid1.ItemsSource = ds.Tables["Food"].DefaultView;

                SqlCommand dataCommand1 = new SqlCommand("GetCategory", myConnection);
                dataCommand1.CommandType = CommandType.StoredProcedure;

                sda1 = new SqlDataAdapter(dataCommand1);
                sda1.Fill(ds, "Category");

                comboBox1.ItemsSource = ds.Tables["Category"].DefaultView;
                comboBox1.DisplayMemberPath = ds.Tables["Category"].Columns["category_name"].ToString();
                comboBox1.SelectedValuePath = ds.Tables["Category"].Columns["category_id"].ToString();

                comboBox2.ItemsSource = ds.Tables["Category"].DefaultView;
                comboBox2.DisplayMemberPath = ds.Tables["Category"].Columns["category_name"].ToString();
                comboBox2.SelectedValuePath = ds.Tables["Category"].Columns["category_id"].ToString();
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Trim().Length == 0)
            {
                MessageBox.Show("Enter Food Name!", "Food on Click");
                textBox1.Focus();
                return;
            }
            else if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Select a Category!", "Food on Click");
                comboBox1.Focus();
                return;
            }
            else if (textBox2.Text.Trim().Length == 0)
            {
                MessageBox.Show("Enter Price!", "Food on Click");
                textBox2.Focus();
                return;
            }

            try
            {
                myConnection.Open();
                SqlCommand dataCommand = new SqlCommand("INSERT INTO food_item(food_name, category_id, serving, price) VALUES (@foodname, @category, @serving, @price)", myConnection);

                dataCommand.Parameters.AddWithValue("@foodname", textBox1.Text);
                dataCommand.Parameters.AddWithValue("@category", comboBox1.SelectedValue);
                dataCommand.Parameters.AddWithValue("@price", textBox2.Text);
                dataCommand.Parameters.AddWithValue("@serving", textBox3.Text);

                dataCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                myConnection.Close();
                MessageBox.Show("New Food Item successfully added!", "Food on Click");
            }

            this.NavigationService.Refresh();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                SqlCommand dataCommand = new SqlCommand("SearchFood", myConnection);
                dataCommand.CommandType = CommandType.StoredProcedure;

                dataCommand.Parameters.AddWithValue("@Name", searchTextBox.Text.Trim());

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

        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (((DataRowView)dataGrid1.SelectedItem).Row["food_name"].ToString().Trim() == "")
            {
                MessageBox.Show("Enter Food item name!", "Food on Click");
                return;
            }
            else if (((DataRowView)dataGrid1.SelectedItem).Row["category_id"].ToString().Trim() == "")
            {
                MessageBox.Show("Select a Category!", "Food on Click");
                return;
            }
            else if (((DataRowView)dataGrid1.SelectedItem).Row["price"].ToString().Trim() == "")
            {
                MessageBox.Show("Enter Price!", "Food on Click");
                return;
            }

            int id = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["food_id"].ToString());
            int category = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["category_id"].ToString());
            int price = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["price"].ToString());

            try
            {
                myConnection.Open();
                SqlCommand dataCommand = new SqlCommand("UPDATE food_item SET food_name = @foodname, category_id = @category, serving = @serving, price = @price WHERE food_id = @id", myConnection);

                dataCommand.Parameters.AddWithValue("@foodname", ((DataRowView)dataGrid1.SelectedItem).Row["food_name"].ToString());
                dataCommand.Parameters.AddWithValue("@category", category);
                dataCommand.Parameters.AddWithValue("@serving", ((DataRowView)dataGrid1.SelectedItem).Row["serving"].ToString());
                dataCommand.Parameters.AddWithValue("@price", price);
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
            MessageBox.Show("Food item successfully edited!", "Food on Click");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["food_id"].ToString());
            if (MessageBox.Show("Are you want to delete the Food Item?", "Food on Click", MessageBoxButton.YesNo).ToString() == "Yes")
            {
                try
                {
                    myConnection.Open();
                    SqlCommand dataCommand = new SqlCommand("DELETE FROM food_item WHERE food_id = @id", myConnection);

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
            this.NavigationService.Refresh();
        }
    }
}
