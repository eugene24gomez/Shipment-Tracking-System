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
using ShipmentTracker.userControl;

namespace ShipmentTracker.Pages
{
    public partial class CustomerShipmentPage : Form
    {
        private MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
        private int customerId;
        public CustomerShipmentPage()
        {
            InitializeComponent();
        }

        public void setCustomerId(int customer_id) {
            this.customerId = customer_id;
        }

        private void shipments_data(string search)
        {
            //string query = "SELECT id,full_name,contact_no,full_address,email FROM customer";
            try
            {
                flowLayoutPanel1.Controls.Clear();
                connection.Open();
                string query;
                if (string.IsNullOrEmpty(search))
                {
                    query = $"SELECT trans.id, c.full_name,ts.status, c.full_address, pm.mode, trans.total_amount,courier.name,courier.plate_no FROM transaction_table AS trans JOIN courier ON trans.courier_id = courier.id JOIN shipment_user AS c ON c.id = trans.customer_id JOIN payment_mode AS pm ON pm.id = trans.payment_mode JOIN transaction_status as ts ON ts.id = trans.ship_status WHERE trans.customer_id = @id ORDER BY trans.id DESC";
                }
                else
                {
                    query = $"SELECT trans.id, c.full_name,ts.status, c.full_address, pm.mode, trans.total_amount,courier.name,courier.plate_no FROM transaction_table AS trans JOIN courier ON trans.courier_id = courier.id JOIN shipment_user AS c ON c.id = trans.customer_id JOIN payment_mode AS pm ON pm.id = trans.payment_mode JOIN transaction_status as ts ON ts.id = trans.ship_status WHERE trans.customer_id = @id trans.order_receipt LIKE @search ORDER BY trans.id DESC";
                }

                MySqlCommand command = new MySqlCommand(query, connection);
                if (!string.IsNullOrEmpty(search))
                {
                    command.Parameters.AddWithValue("@search", $"%{search}%");

                }
                command.Parameters.AddWithValue("@id", customerId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        shipmentControl ships = new shipmentControl();
                        ships.ship_id_method = (int)reader["id"];
                        ships.receiver_name_method = reader["full_name"].ToString();
                        ships.address_method = reader["full_address"].ToString();
                        ships.payment_method = reader["mode"].ToString();
                        ships.total_amount_method = Convert.ToDouble(reader["total_amount"]);
                        ships.status_method = reader["status"].ToString();
                        ships.plate_no_method = reader["plate_no"].ToString();
                        ships.courier_name_method = reader["name"].ToString();
                        ships.MyProperty = 0;
                        flowLayoutPanel1.Controls.Add(ships);
                    }
                }


                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Connection Error: Fetching Shipments Data \n" + ex.Message + ex.StackTrace);
            }
            finally
            {
                connection.Close();
            }

        }

        private void ShipmentLoad(object sender, EventArgs e)
        {
            shipments_data(null);
        }

        private void KeyUP(object sender, KeyEventArgs e)
        {
            string search = search_text.Text;

            if (string.IsNullOrEmpty(search))
            {
                shipments_data(null);
            }
            else
            {

                shipments_data(search);
            }
        }
    }
}
