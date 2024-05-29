using MySql.Data.MySqlClient;
using ShipmentTracker.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipmentTracker.Add
{
    public partial class Add_Courier : Form
    {

        Courier courier;
        public Add_Courier()
        {
            InitializeComponent();
        }

        public void setCourier(Courier courier) {
            this.courier = courier;
            
        }

        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            List<string> fields = new List<string> { full_name_text.Text, contact_no_text.Text, address_text.Text,license.Text,car_plate.Text, email_text.Text };
            foreach (string field in fields)
            {
                if (string.IsNullOrEmpty(field))
                {
                    MessageBox.Show("All fields are required. Please fill in all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (contact_no_text.Text.Length != 11 || !IsNumeric(contact_no_text.Text) || !contact_no_text.Text.StartsWith("09"))
            {
                MessageBox.Show("Invalid Phone No. Maximum of 11 Numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!IsValidEmail(email_text.Text))
            {
                MessageBox.Show("Invalid Email Address. Please enter a valid email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Boolean isReadyTOsave = CheckCourierDetails(email_text.Text, contact_no_text.Text, license.Text, car_plate.Text);
            if (isReadyTOsave) {
                MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);

                string query = $"INSERT INTO `courier`(`name`, `contact_no`, `address`, `email`,`plate_no`,`driver_license_no`) VALUES (@name,@contact_no,@address,@email,@plate_no,@driver_license_no)";
                MySqlCommand insert = new MySqlCommand(query, connection);
                insert.Parameters.AddWithValue("@name", full_name_text.Text);
                insert.Parameters.AddWithValue("@contact_no", contact_no_text.Text);
                insert.Parameters.AddWithValue("@address", address_text.Text);
                insert.Parameters.AddWithValue("@email", email_text.Text);
                insert.Parameters.AddWithValue("@plate_no", car_plate.Text);
                insert.Parameters.AddWithValue("@driver_license_no", license.Text);


                try
                {
                    connection.Open();
                    if (insert.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Successfully added");
                        //Customer customer = new Customer();
                        courier.SavingReload();
                        this.Hide();
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




        public bool CheckCourierDetails(string email, string contact_no, string driver, string plate)
        {
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
            MySqlCommand command;
            string query = "SELECT email, contact_no, driver_license_no, plate_no FROM courier WHERE email = @value OR contact_no = @value OR driver_license_no = @value OR plate_no = @value";
            command = new MySqlCommand(query, connection);

            try
            {
                connection.Open();

                // Check email
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@value", email);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show($"The Email '{email}' is already used.", "Courier Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }

                // Check contact number
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@value", contact_no);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show($"The Contact No '{contact_no}' is already used.", "Courier Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }

                // Check driver license number
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@value", driver);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show($"The Driver License No '{driver}' is already used.", "Courier Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }

                // Check plate number
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@value", plate);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show($"The Plate No '{plate}' is already used.", "Courier Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
