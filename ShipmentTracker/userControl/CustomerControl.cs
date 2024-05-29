using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipmentTracker.userControl
{
    public partial class CustomerControl : UserControl
    {
        public CustomerControl()
        {
            InitializeComponent();
        }


        #region Properties
        [Category("Custom Props")]
        private int customer_id;

        public int customer_id_method
        {
            get { return customer_id; }
            set { customer_id = value; ship_id_text.Text = value.ToString().Length > 4 ? value.ToString("D2").Substring(0, 10) : value.ToString("D2"); }
        }
        [Category("Custom Props")]
        private string full_name;

        public string full_name_method
        {
            get { return full_name; }
            set { full_name = value; receiver_text.Text = value.Length > 10 ? value.Substring(0, 10) + "..." : value; }
        }
        [Category("Custom Props")]
        private string address;

        public string address_method
        {
            get { return address; }
            set { address = value;  address_text.Text = value.Length > 12 ? value.Substring(0, 12) + "..." : value; }
        }
        [Category("Custom Props")]

        private string contact_no;

        public string contact_no_method
        {
            get { return contact_no; }
            set { contact_no = value; contact_no_text.Text = value; }
        }
        [Category("Custom Props")]

        private string email;

        public string email_method
        {
            get { return email; }
            set { email = value; email_text.Text = value.Length > 12 ? value.Substring(0, 12) + "..." : value; }
        }


        #endregion


        private void HoverEnter(object sender, EventArgs e)
        {
            guna2Panel1.FillColor = Color.FromArgb(243, 243, 243);
        }

        private void HoverExit(object sender, EventArgs e)
        {
            guna2Panel1.FillColor = Color.Transparent;

        }
    }
}
