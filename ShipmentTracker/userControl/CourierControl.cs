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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipmentTracker.userControl
{
    public partial class CourierControl : UserControl
    {
        private Courier cour;
        public CourierControl()
        {
            InitializeComponent();
        }

        #region Properties
        [Category("Custom Props")]
        private int id;

        public int id_method
        {
            get { return id; }
            set { id = value; id_lbl.Text = value.ToString("D2"); }
        }
        [Category("Custom Props")]
        private string name;

        public string name_method
        {
            get { return name; }
            set { name = value; name_lbl.Text = value.Length >= 15 ? value.Substring(0,15) : value; }
        }
        [Category("Custom Props")]
        private string address;

        public string address_method
        {
            get { return address; }
            set { address = value; address_lbl.Text = value.Length >= 15 ? value.Substring(0, 15) : value; }
        }
        [Category("Custom Props")]
        private string plate_no;

        public string plate_no_method
        {
            get { return plate_no; }
            set { plate_no = value;plate_no_lbl.Text = value; }
        }
        [Category("Custom Props")]
        private string contact_no;

        public string contact_no_method
        {
            get { return contact_no; }
            set { contact_no = value; contact_no_lbl.Text = value; }
        }
        [Category("Custom Props")]
        private string email;

        public string email_method
        {
            get { return email; }
            set { email = value;email_text.Text = value.Length >= 15 ? value.Substring(0, 15) : value; }
        }






        #endregion
        public void setCourierForm(Courier cour) {

            this.cour = cour;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
         

            DialogResult result = MessageBox.Show("Are you sure you want to remove this courier?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string query = "UPDATE courier SET isDeleted = 2 WHERE id = @id";
                MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString);
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Courier has been successfully removed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cour.SavingReload();
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
            }
        }
    }
}
