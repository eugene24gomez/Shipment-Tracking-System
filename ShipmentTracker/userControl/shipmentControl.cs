using ShipmentTracker.Update;
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
    public partial class shipmentControl : UserControl
    {
        public shipmentControl()
        {
            InitializeComponent();
        }

        #region Properties
        [Category("Custom Props")]
        private string plate_no;

        public string plate_no_method
        {
            get { return plate_no; }
            set { plate_no = value; plate_nolbl.Text = value; }
        }

        [Category("Custom Props")]
        private string courier_name;

        public string courier_name_method
        {
            get { return courier_name; }
            set { courier_name = value; courier.Text = value.Length > 10 ? value.Substring(0, 10) + "..." : value; }
        }

        [Category("Custom Props")]
        private int shipment_id;

        public int ship_id_method
        {
            get { return shipment_id; }
            set { shipment_id = value; ship_id_text.Text = value.ToString().Length > 4 ?  value.ToString("D2").Substring(0, 10) : value.ToString("D2"); }
        }
        [Category("Custom Props")]
        private string receiver_name;

        public string receiver_name_method
        {
            get { return receiver_name; }
            set { receiver_name = value; receiver_text.Text = value.Length > 10 ? value.Substring(0, 10) + "..." : value;  }
        }
        [Category("Custom Props")]
        private string address;

        public string address_method
        {
            get { return  address; }
            set { address = value; address_text.Text = value.Length > 12 ? value.Substring(0, 12) + "..." : value; }
        }
        [Category("Custom Props")]

        private string payment_mode;

        public string payment_method
        {
            get { return payment_mode; }
            set { payment_mode = value; payment_mode_text.Text = value; }
        }
        [Category("Custom Props")]

        private double total_amount;

        public double total_amount_method
        {
            get { return total_amount; }
            set { total_amount = value; amount_text.Text = $"Php {value.ToString()}"; }
        }
        [Category("Custom Props")]
        private string status;

        public string status_method
        {
            get { return status; }
            set { status = value; status_label.Text = value;
                if (status == "PENDING")
                {
                    status_label.FillColor = Color.FromArgb(184, 134, 11);
                    status_label.HoverState.FillColor = Color.FromArgb(184, 134, 11);
                    status_label.PressedColor = Color.FromArgb(184, 134, 11);
                }
                else if (status == "IN TRANSIT")
                {
                    status_label.FillColor = Color.FromArgb(72, 61, 139);
                    status_label.HoverState.FillColor = Color.FromArgb(72, 61, 139);
                    status_label.PressedColor = Color.FromArgb(72, 61, 139);

                }
                else if (status == "OUT OF DELIVERY")
                {
                    status_label.FillColor = Color.FromArgb(85, 107, 47);
                    status_label.HoverState.FillColor = Color.FromArgb(85, 107, 47);
                    status_label.PressedColor = Color.FromArgb(85, 107, 47);
                }
                else if (status == "DELIVERED")
                {
                    status_label.FillColor = Color.FromArgb(153, 50, 204);
                    status_label.HoverState.FillColor = Color.FromArgb(153, 50, 204);
                    status_label.PressedColor = Color.FromArgb(153, 50, 204);

                }
                else {
                    status_label.FillColor = Color.FromArgb(233, 150, 122);
                    status_label.HoverState.FillColor = Color.FromArgb(233, 150, 122);
                    status_label.PressedColor = Color.FromArgb(233, 150, 122);
                }
            
            
            }
        }
        [Category("Custom Props")]
        private int user_type;

        public int MyProperty
        {
            get { return user_type; }
            set { user_type = value; if (value == 0) {
                    guna2Panel1.Cursor = default;
                } }
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

        private void ShipMentClick(object sender, MouseEventArgs e)
        {

            //
            if (user_type != 0)
            {
                if (status != "DELIVERED" || status != "CANCELLED")
                {
                    UpdateShipment update = new UpdateShipment();
                    update.transaction_id(shipment_id);
                    update.Show();
                }
            }
           
        }
    }
}
