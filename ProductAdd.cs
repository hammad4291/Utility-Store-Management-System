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
    public partial class ProductAdd : Form
    {
        public ProductAdd()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // Hook the SelectionChanged event
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }
        DataSet ds = new DataSet();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ds.Tables[0].Rows.Add(int.Parse(tbPID.Text), tbPName.Text, tbPPrice.Text, tbQuantity.Text);
            try
            {
                String StrCon = "Integrated Security = SSPI; Persist Security Info=False;Initial Catalog = US; Data Source = HAMMAD\\VE_SERVER\r\n";
                SqlConnection con = new SqlConnection(StrCon);
                con.Open();
                String query = "select * from Products";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                SqlCommandBuilder cmb = new SqlCommandBuilder(sda);
                sda.Update(ds);
                ds.Tables[0].AcceptChanges();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        DataGridViewRow sRow = new DataGridViewRow();

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Get the selected row in the DataGridView
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // Find the corresponding DataRow in the DataTable
                    if (selectedRow.Index >= 0) // Ensure the index is valid
                    {
                        DataRow row = ds.Tables[0].Rows[selectedRow.Index];

                        // Update the DataRow with the values from the TextBoxes
                        row["ProductID"] = tbPID.Text;
                        row["ProductName"] = tbPName.Text;
                        row["ProductPrice"] = tbPPrice.Text;
                        row["Quantity"] = tbQuantity.Text;

                        // Update the database to reflect changes
                        String StrCon = "Integrated Security = SSPI; Persist Security Info=False;Initial Catalog = US; Data Source = HAMMAD\\VE_SERVER";
                        SqlConnection con = new SqlConnection(StrCon);
                        con.Open();
                        String query = "select * from Products";
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        SqlCommandBuilder cmb = new SqlCommandBuilder(sda);
                        sda.Update(ds); // Apply changes to the database
                        ds.Tables[0].AcceptChanges(); // Commit changes to the DataTable
                        con.Close();

                        MessageBox.Show("Selected row updated successfully!");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
                {
                    // Find the corresponding row in the DataTable
                    if (dr.Index >= 0) // Ensure the index is valid
                    {
                        DataRow row = ds.Tables[0].Rows[dr.Index];
                        row.Delete(); // Mark the row for deletion
                    }
                }

                // Update the database to reflect changes
                String StrCon = "Integrated Security = SSPI; Persist Security Info=False;Initial Catalog = US; Data Source = HAMMAD\\VE_SERVER";
                SqlConnection con = new SqlConnection(StrCon);
                con.Open();
                String query = "select * from Products";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                SqlCommandBuilder cmb = new SqlCommandBuilder(sda);
                sda.Update(ds); // Apply changes to the database
                ds.Tables[0].AcceptChanges(); // Commit changes to the DataTable
                con.Close();

                MessageBox.Show("Selected rows deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            // Ensure at least one row is selected
            if (dataGridView1.SelectedRows.Count > 0)
            {

                // Get the first selected row
                DataGridViewRow sr = dataGridView1.SelectedRows[0];
                sRow = sr;
                // Safely access and assign cell values to textboxes
                tbPID.Text = sr.Cells[0].Value?.ToString() ?? string.Empty;
                tbPName.Text = sr.Cells[1].Value?.ToString() ?? string.Empty;
                tbPPrice.Text = sr.Cells[2].Value?.ToString() ?? string.Empty;
                tbQuantity.Text = sr.Cells[3].Value?.ToString() ?? string.Empty;
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
