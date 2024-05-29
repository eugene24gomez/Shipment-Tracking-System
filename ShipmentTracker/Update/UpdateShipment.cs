using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace ShipmentTracker.Update
{
   

    public partial class UpdateShipment : Form
    {
        private MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
        private int transactionid,status;
        public UpdateShipment()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void transaction_id(int trans_id)
        {
            this.transactionid = trans_id;
        }

        private void FetchStatus()
        {
            string query = "SELECT * FROM transaction_status";

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                }
                status_combo.Items.Clear();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string statusText = reader["status"].ToString();
                        string statusValue = reader["id"].ToString();
                        status_combo.Items.Add(new KeyValuePair<string, string>(statusText, statusValue));
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

        private void FetchShipReceipt() {

            try
            {
                connection.Open();
                string sql = $"SELECT ship_status, order_receipt FROM transaction_table WHERE id = @transactionid";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@transactionid", transactionid);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            status = (int)reader["ship_status"];
                            string orderReceipt = reader["order_receipt"].ToString();
                            guna2TextBox1.Text = orderReceipt;
                        }
                        else
                        {
                            MessageBox.Show("No order receipt found for the specified transaction ID.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching order receipt: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void UpdateStatus() { 
            try
            {
                connection.Open();
                string updateQuery = "UPDATE transaction_table SET ship_status = @status WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@status", status_combo.SelectedIndex + 1);
                    command.Parameters.AddWithValue("@id", transactionid); 

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Status updated successfully.");
                        FetchStatus();
                        FetchShipReceipt();
                        status_combo.SelectedIndex = status - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            int newStatus = status_combo.SelectedIndex + 1;
            if (!(status < newStatus))
            {
                MessageBox.Show($"New status should be higher than the current status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                status_combo.SelectedIndex = status - 1;

            }
            else {
                UpdateStatus();
            }
        }

        private void UpdateLoad(object sender, EventArgs e)
        {
            FetchShipReceipt();
            FetchStatus();
            status_combo.SelectedIndex = status - 1;
        }
    }
}
