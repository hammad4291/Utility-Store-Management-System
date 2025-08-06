using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USMS_Project
{
    public partial class Form1 : Form
    {
        public OrderAdd orderAdd = new OrderAdd();
        public Form1()
        {
            InitializeComponent();
            Login login = new Login();
            login.MdiParent = this;
            login.Show();
        }

        private void addOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            orderAdd.MdiParent = this;
            orderAdd.Show();
        }

        private void addProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductAdd productAdd = new ProductAdd();
            productAdd.MdiParent = this;
            productAdd.Show();
        }

        private void showProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductShow productShow = new ProductShow();
            productShow.MdiParent = this;    
            productShow.Show();
        }

        private void showOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderShow orderShow = new OrderShow();
            orderShow.MdiParent = this;
            orderShow.Show();
        }

        private void showCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CostomerShow costomerShow = new CostomerShow();
            costomerShow.MdiParent = this;
            costomerShow.Show();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.MdiParent = this;
            login.Show();

        }
    }
}
