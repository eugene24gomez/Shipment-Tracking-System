using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace ShipmentTracker.Pages
{
    public partial class LogInForm : Form
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string username_data, password_data;
            username_data = username.Text;
            password_data = password.Text;
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            string query = "SELECT id, password, user_type FROM shipment_user WHERE username = @username";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@username", username_data);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Username exists
                        {
                            string storedHash = reader["password"].ToString();
                            int userType = Convert.ToInt32(reader["user_type"]);
                            int id = Convert.ToInt32(reader["id"]);

                            if (VerifyHash(password_data, storedHash))
                            {
                                if (userType == 0)
                                {
                                    CustomerDashboard customer_dash = new CustomerDashboard();
                                    customer_dash.setCustomerId(id);
                                    customer_dash.Show();
                                    this.Hide();
                                }
                                else if (userType == 1)
                                {
                                    Dashboard dashboard = new Dashboard();
                                    dashboard.Show();
                                    this.Hide();
                                }

                               
                            }
                            else
                            {
                                MessageBox.Show("Incorrect Password!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Username Not Found!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public static string CalculateMD5Hash(string input)
        {
            byte[] data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static bool VerifyHash(string input, string hash)
        {
            string hashedInput = CalculateMD5Hash(input);
            return string.Equals(hashedInput, hash, StringComparison.OrdinalIgnoreCase);
        }

        private void LogInForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Register register = new Register();
            register.Show();
        }
    }
}
