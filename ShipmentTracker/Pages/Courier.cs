using MySql.Data.MySqlClient;
using ShipmentTracker.Add;
using ShipmentTracker.userControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipmentTracker.Pages
{
    public partial class Courier : Form
    {
        public Courier()
        {
            InitializeComponent();
            //PopulateFlowLayout();
        }

        private void PopulateFlowLayout(string search) {
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
            try
            {

                flowLayoutPanel1.Controls.Clear();
                connection.Open();
                string query;
                if (string.IsNullOrEmpty(search))
                {
                    query = $"SELECT id,name,address,plate_no,driver_license_no,email,contact_no FROM courier WHERE isDeleted = 1 ORDER BY id DESC";
                }
                else
                {
                    query = $"SELECT id,name,address,plate_no,driver_license_no,email,contact_no FROM courier WHERE isDeleted = 1 AND name LIKE @search ORDER BY id DESC";
                }

                MySqlCommand command = new MySqlCommand(query, connection);
                if (!string.IsNullOrEmpty(search))
                {
                    command.Parameters.AddWithValue("@search", $"%{search}%");
                }

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CourierControl courier = new CourierControl();
                        courier.id_method = Convert.ToInt32(reader["id"]);
                        courier.name_method = reader["name"].ToString();
                        courier.address_method = reader["address"].ToString();
                        courier.contact_no_method = reader["contact_no"].ToString();
                        courier.plate_no_method = reader["plate_no"].ToString();
                        courier.email_method = reader["email"].ToString();
                        courier.setCourierForm(this);
                        flowLayoutPanel1.Controls.Add(courier);
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Add_Courier cour = new Add_Courier();
            cour.setCourier(this);
            cour.Show();
        }

        public void SavingReload() {

            PopulateFlowLayout(null);

        }
        private void courier_load(object sender, EventArgs e)
        {
            PopulateFlowLayout(null);
        }

        private void Text_change(object sender, EventArgs e)
        {
            string search = search_text.Text;

            if (string.IsNullOrEmpty(search))
            {
                PopulateFlowLayout(null);
            }
            else
            {

                PopulateFlowLayout(search);
            }
        }
    }
}
