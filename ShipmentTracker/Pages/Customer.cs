using ShipmentTracker.Add;
using ShipmentTracker.userControl;
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

namespace ShipmentTracker.Pages
{
    public partial class Customer : Form
    {
        private MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
      
        public Customer()
        {
            InitializeComponent();       
        }

        public void SavingReload() {

            FetchDataCustomer(null);

        }


        private void FetchDataCustomer(string search) {
            try
            {
                flowLayoutPanel1.Controls.Clear();
                connection.Open();

                string query;
             
                
                
                    query = string.IsNullOrWhiteSpace(search) ?
                        $"SELECT id,full_name,contact_no,full_address,email FROM shipment_user WHERE user_type = 0 ORDER BY id DESC " :
                        "SELECT id,full_name,contact_no,full_address,email FROM shipment_user WHERE email LIKE @search OR full_name LIKE @search AND user_type = 1 ORDER BY id DESC ";

                    MySqlCommand command = new MySqlCommand(query, connection);

                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        command.Parameters.AddWithValue("@search", $"%{search}%");
                    }

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerControl customer = new CustomerControl();
                            customer.customer_id_method = (int)reader["id"];
                            customer.full_name_method = reader["full_name"].ToString();
                            customer.email_method = reader["email"].ToString();
                            customer.contact_no_method = reader["contact_no"].ToString();
                            customer.address_method = reader["full_address"].ToString();
                            flowLayoutPanel1.Controls.Add(customer);
                        }
                    }
                

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Connection Error: Fetching Cars Data \n" + ex.Message + ex.StackTrace);
            }
            finally
            {
                connection.Close();
            }

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
                 AddCustomer add_customer = new AddCustomer(this);
                 add_customer.Show();
        }

        private void CustomerLoad(object sender, EventArgs e)
        {
            FetchDataCustomer(null);
        }

        private void KeyUpsearchCustomer(object sender, KeyEventArgs e)
        {
            string search = search_text.Text;

            if (string.IsNullOrEmpty(search))
            {
                FetchDataCustomer(null);
            }
            else
            {

                FetchDataCustomer(search);
            }

        }
    }
}
