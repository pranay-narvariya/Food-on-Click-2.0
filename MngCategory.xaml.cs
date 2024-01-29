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
    /// Interaction logic for MngCategory.xaml
    /// </summary>
    public partial class MngCategory : Page
    {
        SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBconn"].ToString());
        private DataSet ds;
        private SqlDataAdapter sda;

        public MngCategory()
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
                SqlCommand dataCommand = new SqlCommand("GetCategory", myConnection);
                dataCommand.CommandType = CommandType.StoredProcedure;

                sda = new SqlDataAdapter(dataCommand);
                ds = new DataSet();
                sda.Fill(ds, "Category");
                dataGrid1.ItemsSource = ds.Tables["Category"].DefaultView;

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
                MessageBox.Show("Enter Category name!", "Food on Click");
                textBox1.Focus();
                return;
            }

            try
            {
                myConnection.Open();
                SqlCommand dataCommand = new SqlCommand("INSERT INTO category(category_name) VALUES (@category)", myConnection);

                dataCommand.Parameters.AddWithValue("@category", textBox1.Text);

                dataCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                myConnection.Close();
                MessageBox.Show("New Category successfully added!", "Food on Click");
            }

            this.NavigationService.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["category_id"].ToString());
            if (MessageBox.Show("Are you want to delete the Category?", "Food on Click", MessageBoxButton.YesNo).ToString() == "Yes")
            {
                try
                {
                    myConnection.Open();
                    SqlCommand dataCommand = new SqlCommand("DELETE FROM category WHERE category_id = @id", myConnection);

                    dataCommand.Parameters.AddWithValue("@id", id);

                    dataCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A food item is present in Menu corresponding this category!", "Food on Click");
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    myConnection.Close();
                }
            }
        }

        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (((DataRowView)dataGrid1.SelectedItem).Row["category_name"].ToString().Trim() == "")
            {
                MessageBox.Show("Enter Category Name!", "Food on Click");
                return;
            }

            int id = Int32.Parse(((DataRowView)dataGrid1.SelectedItem).Row["category_id"].ToString());

            try
            {
                myConnection.Open();
                SqlCommand dataCommand = new SqlCommand("UPDATE category SET category_name = @category WHERE category_id = @id", myConnection);

                dataCommand.Parameters.AddWithValue("@category", ((DataRowView)dataGrid1.SelectedItem).Row["category_name"].ToString());
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
    }
}
