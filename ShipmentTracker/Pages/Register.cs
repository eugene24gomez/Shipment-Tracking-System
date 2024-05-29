using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Security.Cryptography;

namespace ShipmentTracker.Pages
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }
        public static bool IsValidEmail(string email)
        {
            // Regular expression pattern for validating email addresses
            string pattern = @"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com)$";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LogInForm login = new LogInForm();
            this.Hide();
            login.Show();
        }

        private string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string fullName, password, userName, contact_no, email, full_address;
            fullName = full_name_text.Text;
            password = password_text.Text;
            userName = username_text.Text;
            contact_no = contact_no_text.Text;
            email = email_text.Text;
            full_address = full_address_text.Text;

            if (string.IsNullOrEmpty(fullName) ||
            string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(userName) ||
            string.IsNullOrEmpty(contact_no) ||
            string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(full_address))
            {
                MessageBox.Show("All fields are required. Please fill in all the details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Regex.IsMatch(contact_no, @"^\d{11}$"))
            {
                MessageBox.Show("Error: Contact number must contain exactly 11 numeric characters.", "Validation Error");
                return;
            }
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Error: Invalid Email.", "Validation Error");
                return;
            }
            string hashedPassword = GetMd5Hash(password);

            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if username or email already exists
                    string query = "SELECT COUNT(*) FROM shipment_user WHERE username = @UserName OR email = @Email";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@Email", email);

                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Username or Email already exists. Please use a different one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Proceed with the registration logic
                    string insertQuery = "INSERT INTO shipment_user (full_name, password, username, contact_no, email, full_address) VALUES (@FullName, @Password, @UserName, @ContactNo, @Email, @FullAddress)";
                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@FullName", fullName.ToUpper());
                        insertCommand.Parameters.AddWithValue("@Password", hashedPassword);  // Store the hashed password
                        insertCommand.Parameters.AddWithValue("@UserName", userName.ToUpper());
                        insertCommand.Parameters.AddWithValue("@ContactNo", contact_no);
                        insertCommand.Parameters.AddWithValue("@Email", email.ToUpper());
                        insertCommand.Parameters.AddWithValue("@FullAddress", full_address.ToUpper());

                        insertCommand.ExecuteNonQuery();
                    }
                    full_name_text.Text = "";
                      password_text.Text = "";
                     username_text.Text = "";
                     contact_no_text.Text = "";
                     email_text.Text = "";
                    full_address_text.Text = "";
                    
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogInForm login = new LogInForm();
                    this.Hide();
                    login.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
            
    }
}
