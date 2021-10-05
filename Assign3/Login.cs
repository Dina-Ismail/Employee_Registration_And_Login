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

namespace Assign3
{
    public partial class Login : Form
    {
        SqlConnection con = new
        SqlConnection(@"Server=DESKTOP-H12N5AH;Database=FinalProject;Trusted_Connection=true");
        //SqlConnection(@"Server=DESKTOP-H12N5AH;Database=FinalProject;Trusted_Connection=true");
        SqlCommand cmd;
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text != "" && txtPassword.Text != "") //checking that username and password text boxes are not empty
            {
                try
                {
                    //here this part returns if there is any matching username and password
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*),role FROM employee WHERE username='" + txtUsername.Text + "' AND password='" + txtPassword.Text + "'GROUP BY role", con);
                    /* in above line the program is selecting the whole data from table and the matching it with the user name and password provided by user. */
                    DataTable dt = new DataTable(); //this is creating a virtual table  
                    sda.Fill(dt);
                    //it has to be only 1 record matching to login successfully
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        
                        if (dt.Rows[0][1].ToString().ToLower().Trim().Equals("admin"))
                        {
                           MessageBox.Show("You are logging as an "+ dt.Rows[0][1].ToString().ToLower());
                            this.Hide();
                            new HomePage().Show();
                        }
                        else
                             if (dt.Rows[0][1].ToString().ToLower().Trim().Equals("cashier"))

                        {
                            MessageBox.Show("You are logging as an " + dt.Rows[0][1].ToString().ToLower());

                            this.Hide();
                            new HomePage().Show();
                        }
                    }
                    else
                        MessageBox.Show("Please verify your username and password","Invalid username or password", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                catch (Exception exp)
                {
                    MessageBox.Show("Username and password couldn't be found, please contact the Admin " , "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
                MessageBox.Show("Username and passwords can't be blank ", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Forget_Password().Show();
            this.Hide();
        }
    }
}
