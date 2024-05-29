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
using ShipmentTracker.Add;

namespace ShipmentTracker.Pages
{
    public partial class Sender : Form
    {
        private MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
        private int pageSize = 10;
        private int currentPage = 1;
        private int totalPages = 0;
        public Sender()
        {
            InitializeComponent();
        }

        public void SavingReload()
        {

            FetchDataCustomer(null);

        }


        private void FetchDataCustomer(string search)
        {
            //string query = "SELECT id,full_name,contact_no,full_address,email FROM customer";
            try
            {
                flowLayoutPanel1.Controls.Clear();
                //page_count.Text = currentPage.ToString() + "...";
                connection.Open();

                string query;


                    // Construct main query with pagination
                    query = string.IsNullOrWhiteSpace(search) ?
                      $"SELECT *, DATE(added_at) AS added_at_date FROM sender ORDER BY id DESC " :
                        "SELECT *, DATE(added_at) AS added_at_date FROM sender WHERE sender_name LIKE @search ORDER BY id DESC";

                    MySqlCommand command = new MySqlCommand(query, connection);

                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        command.Parameters.AddWithValue("@search", $"%{search}%");
         
                    }

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SenderControl sender_control = new SenderControl();
                            sender_control.customer_id_method = (int)reader["id"];
                            sender_control.full_name_method = reader["sender_name"].ToString();
                            string formattedDate = ((DateTime)reader["added_at_date"]).ToString("yyyy-MM-dd");

                            sender_control.added_at_method = formattedDate;
                            sender_control.contact_no_method = reader["contact_no"].ToString();
                            sender_control.address_method = reader["address"].ToString();
                            flowLayoutPanel1.Controls.Add(sender_control);
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

        private void senderLoad(object sender, EventArgs e)
        {
            FetchDataCustomer(null);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            AddSender add_sender = new AddSender(this);
            add_sender.Show();
        }

        private void SenderSearchTextKeyUp(object sender, EventArgs e)
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
