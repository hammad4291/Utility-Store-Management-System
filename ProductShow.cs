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
    public partial class ProductShow : Form
    {
        public ProductShow()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DataSet ds = new DataSet();
            try
            {
                String StrCon = "Integrated Security = SSPI; Persist Security Info=False;Initial Catalog = US; Data Source = HAMMAD\\VE_SERVER\r\n";
                SqlConnection con = new SqlConnection(StrCon);
                con.Open();
                String query = "select * from Products";
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the column being formatted is the "Quantity" column
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Quantity" && e.Value != null)
            {
                try
                {
                    int quantity = Convert.ToInt32(e.Value); // Convert the cell value to an integer
                    if (quantity < 5) // Check if the Quantity is less than 5
                    {
                        // Change the entire row's background color to red
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White; // Optional: Set text color to white for better visibility
                    }
                    else
                    {
                        // Reset the row's color if the condition is not met
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                catch
                {
                    // Handle any conversion errors or unexpected values gracefully
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

    }
}
