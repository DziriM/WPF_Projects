using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CurrencyConverter_Static
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter dataAdapter = new SqlDataAdapter();

        private int CurrencyId = 0;
        private double FromAmount = 0;
        private double ToAmount = 0;

        public MainWindow()
        {
            InitializeComponent();
            BindCurrency();
            GetData();
        }

        public void OpenConnectionToDB()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["CurrencyConverterDbConnection"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        private void BindCurrency()
        {
            // Establish connection to DB 
            OpenConnectionToDB();

            // DataTable
            DataTable dt = new DataTable();

            // Command
            // Which type of command we use to write the query
            cmd = new SqlCommand("SELECT Id, CurrencyName from Currency_Master", sqlConnection);
            cmd.CommandType = CommandType.Text;

            // SqlAdapter
            dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dt);

            // Create an object for DataRow
            DataRow newRow = dt.NewRow();

            // Assign a value to Id column
            newRow["Id"] = 0;
            // Assign a value to CurrencyName column
            newRow["CurrencyName"] = "--SELECT--";

            // Insert a new row in dt with the data at a 0 position
            dt.Rows.InsertAt(newRow, 0);

            // Assign the datatable data to the two combobox
            if (dt != null && dt.Rows.Count > 0)
            {
                cmbFromCurrency.ItemsSource = dt.DefaultView;

                cmbToCurrency.ItemsSource = dt.DefaultView;
            }

            // Closing connection
            sqlConnection.Close();

            // Affectation aux ComboBox
            cmbFromCurrency.DisplayMemberPath = "CurrencyName";
            cmbFromCurrency.SelectedValuePath = "Id";
            cmbFromCurrency.SelectedIndex = 0;

            cmbToCurrency.DisplayMemberPath = "CurrencyName";
            cmbToCurrency.SelectedValuePath = "Id";
            cmbToCurrency.SelectedIndex = 0;
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            // Create the variable as ConvertedValue
            double ConvertedValue;

            // Check if the amount textbox is null or blank
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                txtCurrency.Focus();

                return;
            }
            // Check if the two comboBox have valid selected values
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                cmbFromCurrency.Focus();
                return;
            }
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                cmbToCurrency.Focus();
                return;
            }


            // Check if From and To selected values are the same (then there is nothing to convert
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                ConvertedValue = double.Parse(txtCurrency.Text);
                // N3 is used to place 000 after the dot(.)
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3"); 
            }
            else
            {
                ConvertedValue = double.Parse(cmbFromCurrency.SelectedValue.ToString()) * 
                                 double.Parse(txtCurrency.Text) / 
                                 double.Parse(cmbToCurrency.SelectedValue.ToString());

                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;

            if (cmbFromCurrency.Items.Count > 0) 
                cmbFromCurrency.SelectedIndex = 0;

            if (cmbToCurrency.Items.Count > 0)
                cmbToCurrency.SelectedIndex = 0;

            lblCurrency.Content = string.Empty;

            txtCurrency.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAmount.Text == null || txtAmount.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter amount", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtAmount.Focus();
                    return;
                }
                else if (txtCurrencyName.Text == null || txtCurrencyName.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter currency name", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtCurrencyName.Focus();
                    return;
                }
                else
                {
                    // Update button
                    if (CurrencyId > 0) 
                    {
                        // Check for user confirmation
                        if (MessageBox.Show("Are you sure you want to update ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            OpenConnectionToDB();
                            DataTable dt = new DataTable();
                            cmd = new SqlCommand("UPDATE Currency_Master SET Amount = @Amount, CurrencyName = @CurrencyName WHERE Id = @Id", sqlConnection);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Id", CurrencyId);
                            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                            cmd.Parameters.AddWithValue("@CurrencyName", txtCurrencyName.Text);
                            cmd.ExecuteNonQuery();
                            sqlConnection.Close();

                            MessageBox.Show("Data Updated successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    // Save button
                    else
                    {
                        // Check for user confirmation
                        if (MessageBox.Show("Are you sure you want to save ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            OpenConnectionToDB();
                            DataTable dt = new DataTable();
                            cmd = new SqlCommand("INSERT INTO Currency_Master(Amount, CurrencyName) VALUES(@Amount, @CurrencyName)", sqlConnection);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                            cmd.Parameters.AddWithValue("@CurrencyName", txtCurrencyName.Text);
                            cmd.ExecuteNonQuery();
                            sqlConnection.Close();

                            MessageBox.Show("Data saved successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                    ClearMaster();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// To clear all the input
        /// </summary>
        private void ClearMaster()
        {
            try
            {
                txtAmount.Text = string.Empty;
                txtCurrencyName.Text = string.Empty;
                btnSave.Content = "Save";

                // Display the data in the table
                GetData();
                
                CurrencyId = 0;
                BindCurrency();
                txtAmount.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GetData()
        {
            OpenConnectionToDB();
            DataTable dt = new DataTable();
            cmd = new SqlCommand("SELECT * FROM Currency_Master", sqlConnection);
            cmd.CommandType = CommandType.Text;
            dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                dgvCurrency.ItemsSource = dt.DefaultView;
            }
            else
            {
                dgvCurrency.ItemsSource = null;
            }

            sqlConnection.Close();
        } 


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearMaster();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgvCurrency_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            try
            {
                DataGrid grid = (DataGrid)sender;
                DataRowView row_selected = grid.CurrentItem as DataRowView;

                if (row_selected != null)
                {
                    if (grid.SelectedCells.Count > 0)
                    {
                        // Get selected row id column value and set currencyId
                        CurrencyId = Int32.Parse(row_selected["Id"].ToString());

                        if (grid.SelectedCells[0].Column.DisplayIndex == 0)
                        {
                            txtAmount.Text = row_selected["Amount"].ToString();
                            txtCurrencyName.Text = row_selected["CurrencyName"].ToString();
                            btnSave.Content = "Update";
                        }

                        if (grid.SelectedCells[0].Column.DisplayIndex == 1)
                        {
                            if (MessageBox.Show("Are you sure you want to delete ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                OpenConnectionToDB();
                                DataTable dt = new DataTable();
                                cmd = new SqlCommand("DELETE FROM Currency_Master WHERE Id = @Id", sqlConnection);
                                cmd.Parameters.AddWithValue("@Id", CurrencyId);
                                cmd.ExecuteNonQuery();

                                sqlConnection.Close();
                                MessageBox.Show("Data deleted successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                ClearMaster();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
