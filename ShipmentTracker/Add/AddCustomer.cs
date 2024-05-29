using MySql.Data.MySqlClient;
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
using System.Text.RegularExpressions;
using ShipmentTracker.Pages;

namespace ShipmentTracker.Add
{
    public partial class AddCustomer : Form
    {
        private MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
        MySqlCommand command;
        private Customer customerForm;

        public AddCustomer(Customer customerForm)
        {
            InitializeComponent();
            this.customerForm = customerForm;

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
           
            string full_name, contact_no, email, address;
            full_name = full_name_text.Text;
            contact_no = contact_no_text.Text;
            email = email_text.Text;
            address = address_text.Text;
            List<string> fields = new List<string> { full_name, contact_no, email, address };
            foreach (string field in fields)
            {
                if (string.IsNullOrEmpty(field))
                {
                    MessageBox.Show("All fields are required. Please fill in all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }
            }
            if (contact_no.Length != 11 || !IsNumeric(contact_no) || !contact_no.StartsWith("09")) {
                MessageBox.Show("Invalid Phone No. Maximum of 11 Numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!IsValidEmail(email)) {
                MessageBox.Show("Invalid Email Address. Please enter a valid email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            Boolean isReadyToSave = checkingInDataBase(email, contact_no);
            if (isReadyToSave)
            {
                string query = $"INSERT INTO `customer`(`full_name`, `contact_no`, `full_address`, `email`) VALUES (@full_name,@contact_no,@address,@email)";
                MySqlCommand insert = new MySqlCommand(query, connection);
                insert.Parameters.AddWithValue("@full_name", full_name);
                insert.Parameters.AddWithValue("@contact_no", contact_no);
                insert.Parameters.AddWithValue("@address", address);
                insert.Parameters.AddWithValue("@email", email);
                try
                {
                    connection.Open();
                    if (insert.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Successfully added");
                        Customer customer = new Customer();
                        customerForm.SavingReload();
                        full_name_text.Text = "";
                        contact_no_text.Text = "";
                        email_text.Text = "";
                        address_text.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Saving Error" + ex.StackTrace + "\n" + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

            }

        }

        public static bool IsValidEmail(string email)
        {
            // Regular expression pattern for validating email addresses
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern);

            // Use the Regex.IsMatch method to check if the email matches the pattern
            return regex.IsMatch(email);
        }


        private Boolean checkingInDataBase(string email, string contact_no) {
            string query = "SELECT email,contact_no FROM customer WHERE email = @email OR contact_no = @contact_no";
            command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@contact_no", contact_no);
            try
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show($"The Email or Phone no. is already used.", "Customer Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                connection.Close();
            }
            return true;
        }


        private bool IsNumeric(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
