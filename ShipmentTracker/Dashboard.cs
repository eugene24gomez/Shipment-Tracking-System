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
    public partial class Dashboard : Form
    {
        private List<Guna2Button> sidebarButtons = new List<Guna2Button>();
        private Form currentForm = null;

        public Dashboard()
        {
            Shipments ships = new Shipments();

            InitializeComponent();
            sidebarButtons.Add(shipment);
            sidebarButtons.Add(Transaction);
            sidebarButtons.Add(logout);
            sidebarButtons.Add(customer_btn);
            sidebarButtons.Add(sender_btn);
            ShowForm(ships);

        }

        private void ChangeColor(Guna2Button clickedButton)
        {
            clickedButton.FillColor = Color.FromArgb(247, 224, 219);
            // Set the background color of other buttons to transparent
            foreach (Guna2Button button in sidebarButtons)
            {
                if (button != clickedButton)
                {
                    button.FillColor = Color.Transparent;
                }
            }
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

        private void Transaction_Click(object sender, EventArgs e)
        {
                 add_Shipment add = new add_Shipment();

             Guna2Button clickedButton = (Guna2Button)sender;
            ChangeColor(clickedButton);
            ShowForm(add);
        }

        private void carGarage_Click(object sender, EventArgs e)
        {
               Shipments ships = new Shipments();

                Guna2Button clickedButton = (Guna2Button)sender;
            ChangeColor(clickedButton);
            ShowForm(ships);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
                 Customer customer = new Customer();

        Guna2Button clickedButton = (Guna2Button)sender;
            ChangeColor(clickedButton);
            ShowForm(customer);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
                    Sender sendr = new Sender();
                 Guna2Button clickedButton = (Guna2Button)sender;
            ChangeColor(clickedButton);
            ShowForm(sendr);
        }

        private void logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LogInForm login = new LogInForm();
            login.Show();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            Courier cour = new Courier();
            Guna2Button clickedButton = (Guna2Button)sender;
            ChangeColor(clickedButton);
            ShowForm(cour);
        }
    }
}
