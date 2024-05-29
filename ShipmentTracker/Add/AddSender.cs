using ShipmentTracker.Pages;
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

namespace ShipmentTracker.Add
{
    public partial class AddSender : Form
    {
        private MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
        MySqlCommand command;
        private Sender sndr;

        public AddSender(Sender sndr)
        {
            InitializeComponent();
            this.sndr = sndr;

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            string company_name, contact_no, address;
            company_name = full_name_text.Text;
            contact_no = contact_no_text.Text;
            address = address_text.Text;
            List<string> fields = new List<string> { company_name, contact_no, address };
            foreach (string field in fields)
            {
                if (string.IsNullOrEmpty(field))
                {
                    MessageBox.Show("All fields are required. Please fill in all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (contact_no.Length != 11 || !IsNumeric(contact_no) || !contact_no.StartsWith("09"))
            {
                MessageBox.Show("Invalid Phone No. Maximum of 11 Numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Boolean isReadyToSave = checkingInDataBase(contact_no, company_name);
            if (isReadyToSave)
            {
                string query = $"INSERT INTO `sender`(`sender_name`, `contact_no`, `address`) VALUES (@full_name,@contact_no,@address)";
                MySqlCommand insert = new MySqlCommand(query, connection);
                insert.Parameters.AddWithValue("@full_name", company_name.ToUpper());
                insert.Parameters.AddWithValue("@contact_no", contact_no);
                insert.Parameters.AddWithValue("@address", address.ToUpper());
                try
                {
                    connection.Open();
                    if (insert.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Successfully added");
                        Customer customer = new Customer();
                        sndr.SavingReload();
                        full_name_text.Text = "";
                        contact_no_text.Text = "";
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



        private Boolean checkingInDataBase( string contact_no,string name)
        {
            string query = "SELECT contact_no FROM sender WHERE  contact_no = @contact_no OR sender_name = @name";
            command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@contact_no", contact_no);
            command.Parameters.AddWithValue("@name", name);

            try
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show($"The Phone no. Or the Company  is already registered.", "Customer Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
