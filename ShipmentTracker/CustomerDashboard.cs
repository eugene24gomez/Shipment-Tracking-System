using Guna.UI2.WinForms;
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

namespace ShipmentTracker
{
    public partial class CustomerDashboard : Form
    {
        private List<Guna2Button> sidebarButtons = new List<Guna2Button>();
        private CustomerShipmentPage customer_ships = new CustomerShipmentPage();
        private Form currentForm = null;
        private int customerId;
        public CustomerDashboard()
        {
            InitializeComponent();
            sidebarButtons.Add(logout);
            sidebarButtons.Add(shipment);

        }

        public void setCustomerId(int customer_id) {
            this.customerId = customer_id;
        }

        private void ShowForm(Form form)
        {
            if (currentForm == form)
            {
                return;
            }


            contentPanel.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(form);
            currentForm = form;

            form.Show();
        }

        private void CustomerDashboardOnLoad(object sender, EventArgs e)
        {
            //MessageBox.Show(customerId.ToString());
            customer_ships.setCustomerId(customerId);
            ShowForm(customer_ships);

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LogInForm login = new LogInForm();
            login.Show();
        }
    }
}
