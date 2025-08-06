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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public string LabelText
        {
            get => Output.Text;  // lblOutput is the label's name
            set => Output.Text = value;
        }
        String strCon = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=US;Data Source=HAMMAD\VE_SERVER";

        SqlConnection con = null;
        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (tbPassword.Text == "" || tbUserName.Text == "")
            {
                MessageBox.Show("enter information first");
            }
            else
            {
                try
                {
                    con = new SqlConnection(strCon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    String query = "select * from AdminLogin";
                    SqlCommand sqlCmd = new SqlCommand(query, con);
                    SqlDataReader sdr = sqlCmd.ExecuteReader();
                    bool flagLoged = true;
                    while (sdr.Read())
                    {
                        if (tbUserName.Text == sdr[0].ToString() && tbPassword.Text == sdr[1].ToString())
                        {
                            Output.Text = "" + sdr[0].ToString() + "";
                            flagLoged = false;
                            break;
                        }
                    }

                    if (flagLoged)
                    {
                        MessageBox.Show("Enter valid Information");
                    }
                    sdr.Close();
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Message: " + ex);
                }
            }
        }
    }
}
