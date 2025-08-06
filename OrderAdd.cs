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
    public partial class OrderAdd : Form
    {
        public OrderAdd()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        // Corrected connection string
        String StrCon = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=US;Data Source=HAMMAD\VE_SERVER";
        SqlConnection con = null;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Initialize and open the connection
                using (con = new SqlConnection(StrCon))
                {
                    con.Open();

                    // Query to fetch all products
                    String query = "SELECT * FROM Products";
                    SqlCommand cmd = new SqlCommand(query, con);

                    // Execute reader
                    SqlDataReader dr = cmd.ExecuteReader();
                    bool contain = false;

                    // Iterate through the data reader
                    while (dr.Read())
                    {
                        // Check if the product matches the input
                        if (dr[1].ToString().Trim().ToLower() == tbProductAdd.Text.Trim().ToLower())
                        {
                            contain = true;

                            // Add the product details to the DataGridView
                            dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[3].ToString(), "1", dr[2].ToString(), dr[2].ToString());
                            break;
                        }
                    }

                    // Handle the case where the product does not exist
                    if (!contain)
                    {
                        throw new Exception("This product does not exist, or you can check the spelling.");
                    }

                    // Close the DataReader explicitly
                    dr.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            float totalBill = 0;

            if (string.IsNullOrWhiteSpace(tbCID.Text))
            {
                MessageBox.Show("Enter The Customer ID");
                tbCID.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(tbCName.Text))
            {
                MessageBox.Show("Enter The Customer Name");
                tbCName.Focus();
                return;
            }

            try
            {
                Login loginForm = new Login();
                
                using (SqlConnection con = new SqlConnection(StrCon))
                {
                    con.Open();

                    // Insert customer using parameterized query to prevent SQL injection
                    String query = "INSERT INTO Customer (CustomerID, CustomerName) VALUES (@CustomerID, @CustomerName)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CustomerID", tbCID.Text);
                    cmd.Parameters.AddWithValue("@CustomerName", tbCName.Text);
                    cmd.ExecuteNonQuery();

                    // Insert order details and update available quantity
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue; // Skip new row

                        // Get product details from DataGridView
                        int productID = int.Parse(row.Cells[0].Value.ToString()); // Product ID
                        int orderedQuantity = int.Parse(row.Cells[3].Value.ToString()); // Ordered Quantity
                        int availableQuantity = int.Parse(row.Cells[2].Value.ToString()); // Available Quantity

                        // Check if sufficient stock is available
                        if (orderedQuantity > availableQuantity)
                        {
                            MessageBox.Show($"Insufficient stock for Product ID {productID}. Available: {availableQuantity}, Ordered: {orderedQuantity}", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Insert order details
                        query = "INSERT INTO Orders (OrderID, CustomerID, ProductID, ProductName, ProductPrice, ProductQuantity) " +
                                "VALUES (@OrderID, @CustomerID, @ProductID, @ProductName, @ProductPrice, @ProductQuantity)";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@OrderID", tbCID.Text);
                        cmd.Parameters.AddWithValue("@CustomerID", tbCID.Text);
                        cmd.Parameters.AddWithValue("@ProductID", productID);
                        cmd.Parameters.AddWithValue("@ProductName", row.Cells[1].Value);
                        cmd.Parameters.AddWithValue("@ProductPrice", row.Cells[4].Value);
                        cmd.Parameters.AddWithValue("@ProductQuantity", orderedQuantity);
                        cmd.ExecuteNonQuery();

                        // Update available quantity in the Products table
                        int updatedQuantity = availableQuantity - orderedQuantity;
                        query = "UPDATE Products SET Quantity = @UpdatedQuantity WHERE ProductID = @ProductID";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@UpdatedQuantity", updatedQuantity);
                        cmd.Parameters.AddWithValue("@ProductID", productID);
                        cmd.ExecuteNonQuery();

                        // Add to total bill
                        totalBill += float.Parse(row.Cells[5].Value.ToString()); // Assuming Total Price is in column 5
                    }

                    // Display total bill
                    MessageBox.Show($"Generated by {loginForm.LabelText}Total Bill: {totalBill}", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int selRow = e.RowIndex;
            if (e.ColumnIndex == 8)
            {                // Remove row when column index 7 is clicked
                dataGridView1.Rows.RemoveAt(selRow);
            }
            else if (e.ColumnIndex == 6 && int.Parse(dataGridView1.Rows[selRow].Cells[2].Value.ToString()) > int.Parse(dataGridView1.Rows[selRow].Cells[3].Value.ToString()))
            {
                // Increment quantity and update total price
                int qnt = int.Parse(dataGridView1.Rows[selRow].Cells[3].Value.ToString());
                qnt++; // Increment quantity
                dataGridView1.Rows[selRow].Cells[3].Value = qnt;

                int unitPrice = int.Parse(dataGridView1.Rows[selRow].Cells[4].Value.ToString());
                int totPrice = unitPrice * qnt;
                dataGridView1.Rows[selRow].Cells[5].Value = totPrice; // Update total price
            }
            else if (e.ColumnIndex == 7)
            {
                // Decrement quantity if greater than 1 and update total price
                int qnt = int.Parse(dataGridView1.Rows[selRow].Cells[3].Value.ToString());
                if (qnt > 1)
                {
                    qnt--; // Decrement quantity
                    dataGridView1.Rows[selRow].Cells[3].Value = qnt;

                    int unitPrice = int.Parse(dataGridView1.Rows[selRow].Cells[4].Value.ToString());
                    int totPrice = unitPrice * qnt;
                    dataGridView1.Rows[selRow].Cells[5].Value = totPrice; // Update total price
                }
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
