using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using Guna.UI2.WinForms;


namespace ShipmentTracker.Pages
{

    public partial class add_Shipment : Form
    {
        private MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
        private int customerId, senderId,courierId;
        public add_Shipment()
        {
            InitializeComponent();
            FetchPaymentMethod();
        }

        private void FetchPaymentMethod()
        {
            string query = "SELECT * FROM payment_mode";

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                }

                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string statusText = reader["mode"].ToString();
                        string statusValue = reader["id"].ToString();
                        payment_combo.Items.Add(new KeyValuePair<string, string>(statusText, statusValue));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching payment methods: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }

        private void CustomerData(int id)
        {
            string query = "SELECT full_name, contact_no, full_address FROM shipment_user WHERE id = @id AND user_type = 0";

            try
            {
                if (connection.State == ConnectionState.Closed) {
                    connection.Open();

                }

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    // Create a DataAdapter and a DataTable
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    // Fill the DataTable with the results of the query
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow row = dataTable.Rows[0];

                        customer_full_name.Text = row["full_name"].ToString();
                        contact_no_text.Text = row["contact_no"].ToString();
                        address_text.Text = row["full_address"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No Customer found with the provided ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}  {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open) {
                    connection.Close();
                }
                   
            }
        }


        private void SenderData(int id)
        {
            string query = "SELECT sender_name, address, contact_no FROM sender WHERE id = @id";

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                }

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    // Create a DataAdapter and a DataTable
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    // Fill the DataTable with the results of the query
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow row = dataTable.Rows[0];

                        // Retrieve values from the DataRow and assign them to respective controls
                        sender_name.Text = row["sender_name"].ToString();
                        sender_contact_no_text.Text = row["contact_no"].ToString();
                        sender_address_text.Text = row["address"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No Sender found with the provided ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}  {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }

        private void customer_text_change(object sender, EventArgs e)
        {
            string full_name = customer_full_name.Text;

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                }

                string query = "SELECT full_name,id FROM shipment_user WHERE full_name LIKE @full_name";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@full_name", $"%{full_name}%");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();

                        while (reader.Read())
                        {
                            autoComplete.Add(reader["full_name"].ToString().ToUpper() + " - " + reader["id"].ToString());
                        }

                        customer_full_name.AutoCompleteCustomSource = autoComplete;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }

        private void Customer_enter_key(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(customer_full_name.Text) && e.KeyCode == Keys.Enter)
            {
                // Extract the ID from the selected suggestion
                string selectedSuggestion = customer_full_name.Text;
                string[] parts = selectedSuggestion.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 2)
                {
                    string idString = parts[parts.Length - 1];
                    if (int.TryParse(idString, out int id))
                    {
                        customerId = id;
                        CustomerData(id);
                        //MessageBox.Show($"Selected ID: {id}");
                    }
                    else
                    {
                        MessageBox.Show("Error parsing ID.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid suggestion format.");
                }
            }
        }

        private void sender_name_changed(object sender, EventArgs e)
        {
            string full_name = sender_name.Text;

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                }

                string query = "SELECT sender_name,id FROM sender WHERE sender_name LIKE @full_name";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@full_name", $"%{full_name}%");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();

                        while (reader.Read())
                        {
                            autoComplete.Add(reader["sender_name"].ToString().ToUpper() + " - " + reader["id"].ToString());
                        }

                        sender_name.AutoCompleteCustomSource = autoComplete;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }

        private void Sender_text_keyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(sender_name.Text) && e.KeyCode == Keys.Enter)
            {
                // Extract the ID from the selected suggestion
                string selectedSuggestion = sender_name.Text;
                string[] parts = selectedSuggestion.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 2)
                {
                    string idString = parts[parts.Length - 1];
                    if (int.TryParse(idString, out int id))
                    {
                        senderId = id;
                        SenderData(id);
                    }
                    else
                    {
                        MessageBox.Show("Error parsing ID.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid suggestion format.");
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            string order_receipt = GenerateOrderReceipt();
            string query = "INSERT INTO `transaction_table`(`order_receipt`, `customer_id`, `sender_id`,`courier_id`, `ship_status`, `payment_mode`,`total_amount`,`expected_delivery_date`) VALUES (@order_receipt,@customer_id,@sender_id,@courier_id,@ship_status,@payment_mode,@total_amount,@expected_delivery_date)";
            MySqlCommand query_insert = new MySqlCommand(query, connection);
            query_insert.Parameters.AddWithValue("@order_receipt", order_receipt);
            query_insert.Parameters.AddWithValue("@customer_id", customerId);
            query_insert.Parameters.AddWithValue("@sender_id", senderId);
            query_insert.Parameters.AddWithValue("@courier_id", courierId);

            query_insert.Parameters.AddWithValue("@ship_status", 1);
            query_insert.Parameters.AddWithValue("@payment_mode", payment_combo.SelectedIndex + 1);
            query_insert.Parameters.AddWithValue("@expected_delivery_date", date.Value.Date);

            double amount;
            if (Double.TryParse(total_amount.Text, out amount))
            {
                query_insert.Parameters.AddWithValue("@total_amount", amount);
            }
            else
            {
                MessageBox.Show("Invalid total amount format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (date.Value.Date <= DateTime.Now.Date)
            {
                MessageBox.Show("The selected date must be in the future.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (payment_combo.SelectedIndex != -1 && customerId != 0 && senderId != 0)
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();

                    }
                    query_insert.ExecuteNonQuery();
                    sender_name.Text = "";
                    customer_full_name.Text = "";
                    sender_address_text.Text = "";
                    sender_contact_no_text.Text = "";
                    address_text.Text = "";
                    contact_no_text.Text = "";
                    total_amount.Text = "";
                    payment_combo.SelectedIndex = -1;
                    courier.Text = "";
                    MessageBox.Show("Transaction successfully inserted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inserting transaction: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }


        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomStringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                randomStringBuilder.Append(chars[random.Next(chars.Length)]);
            }
            return randomStringBuilder.ToString();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            sender_name.Text = "";
            customer_full_name.Text = "";
            sender_address_text.Text = "";
            sender_contact_no_text.Text = "";
            address_text.Text = "";
            contact_no_text.Text = "";
            total_amount.Text = "";
            customerId = 0;
            senderId = 0;
        }

        public string GenerateOrderReceipt()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
     
            string randomString = GenerateRandomString(4); 

            string orderReceipt = timestamp + randomString;

            return orderReceipt;
        }

        private void courier_keyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(courier.Text) && e.KeyCode == Keys.Enter)
            {
                // Extract the ID from the selected suggestion
                string selectedSuggestion = courier.Text;
                string[] parts = selectedSuggestion.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 2)
                {
                    string idString = parts[parts.Length - 1];
                    if (int.TryParse(idString, out int id))
                    {
                        courierId = id;
                    }
                    else
                    {
                        MessageBox.Show("Error parsing ID.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid suggestion format.");
                }
            }
        }

        private void AddShipmentLoad(object sender, EventArgs e)
        {
            FetchCourier();
        }

        public void FetchCourier()
        {
            string full_name = courier.Text;
            string query;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                }
                if (string.IsNullOrEmpty(full_name))
                {
                    query = "SELECT name, id FROM courier WHERE isDeleted = 1";
                }
                else
                {
                    query = "SELECT name, id FROM courier WHERE isDeleted = 1 AND name LIKE @full_name";
                }

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(full_name)) {
                        command.Parameters.AddWithValue("@full_name", $"%{full_name}%");

                    }

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();

                        while (reader.Read())
                        {
                            autoComplete.Add(reader["name"].ToString().ToUpper() + " - " + reader["id"].ToString());
                        }

                        courier.AutoCompleteCustomSource = autoComplete;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }

    }
}
