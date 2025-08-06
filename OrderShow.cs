using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USMS_Project
{
    public partial class OrderShow : Form
    {
        DataSet ds = new DataSet();
        public OrderShow()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            try
            {
                String StrCon = "Integrated Security = SSPI; Persist Security Info=False;Initial Catalog = US; Data Source = HAMMAD\\VE_SERVER\r\n";
                SqlConnection con = new SqlConnection(StrCon);
                con.Open();
                String query = "select * from Orders";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                String StrCon = "Integrated Security = SSPI; Persist Security Info=False;Initial Catalog = US; Data Source = HAMMAD\\VE_SERVER";
                SqlConnection con = new SqlConnection(StrCon);
                con.Open();

                foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
                {
                    if (dr.Index >= 0) // Ensure the index is valid
                    {
                        // Retrieve the primary key of the selected row
                        int orderId = Convert.ToInt32(dr.Cells["OrderID"].Value); // Assuming 'OrderID' is the primary key column name

                        // Execute delete command for the corresponding row in the database
                        String deleteQuery = "DELETE FROM Orders WHERE OrderID = @OrderID";
                        SqlCommand deleteCmd = new SqlCommand(deleteQuery, con);
                        deleteCmd.Parameters.AddWithValue("@OrderID", orderId);
                        deleteCmd.ExecuteNonQuery();

                        // Remove the row from the DataTable
                        dataGridView1.Rows.Remove(dr);
                    }
                }

                con.Close();

                MessageBox.Show("Selected rows deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
