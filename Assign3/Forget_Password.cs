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
    public partial class Forget_Password : Form
    {
        SqlConnection con = new
        SqlConnection(@"Server=DESKTOP-H12N5AH;Database=FinalProject;Trusted_Connection=true");
        //SqlConnection(@"Server=DESKTOP-H12N5AH;Database=FinalProject;Trusted_Connection=true");
        SqlCommand cmd;
        int Empid;
        public Forget_Password()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text != "" && cbRole.SelectedItem.ToString() != "") //checking that username and password text boxes are not empty
            {
                try
                    {
                    //here this part returns if there is any matching username and password
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*),emp_id FROM employee WHERE username='" + txtUsername.Text + "' AND role='" + cbRole.Text + "' GROUP BY emp_id", con);

                    /* in above line the program is selecting the whole data from table and the matching it with the user name and password provided by user. */
                    DataTable dt = new DataTable(); //this is creating a virtual table  
                    sda.Fill(dt);
                    Empid = int.Parse(dt.Rows[0][1].ToString());

                    MessageBox.Show("Found Your record, Now enter your new password", "Record Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //it has to be only 1 record matching to login successfully
                    if (dt.Rows[0][0].ToString() == "1")
                        {
                          // show rest textboxes

                            txtpassword.Visible = true;
                            txtConfirm.Visible = true;
                            btnSubmit.Visible = true;
                            lblnewpassword.Visible = true;
                            lblconfirm.Visible = true;  
                        }
                        else
                            MessageBox.Show("Please verify your information, or contact an admin", "Invalid username or password", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    catch (Exception ex)
                    {
                    MessageBox.Show("Please verify your information, or contact an admin", "Invalid username or password", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else if (txtEmployeeID.Text != "" && cbRole.SelectedItem.ToString() != "") //checking that username and password text boxes are not empty

            {
                try
                {

                    //here this part returns if there is any matching username and password
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*),emp_id FROM employee WHERE emp_id='" + txtEmployeeID.Text + "' AND role='" + cbRole.Text + "' GROUP BY emp_id", con);
                    
                    /* in above line the program is selecting the whole data from table and the matching it with the user name and password provided by user. */
                    DataTable dt = new DataTable(); //this is creating a virtual table  
                    sda.Fill(dt);
                    Empid = int.Parse(dt.Rows[0][1].ToString());
                    MessageBox.Show("Found Your record, Now enter your new password", "Record Found",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    //it has to be only 1 record matching to login successfully
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                       // show rest textboxes

                        txtpassword.Visible = true;
                        txtConfirm.Visible = true;
                        btnSubmit.Visible = true;
                        lblnewpassword.Visible = true;
                        lblconfirm.Visible = true;
                    }
                    else
                        MessageBox.Show("Please verify your information, or contact an admin", "Invalid username or password", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please verify your information, or contact an admin", "Invalid username or password", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtpassword.Text.Equals(txtConfirm.Text))
            {
                // here user will update the password
                cmd = new SqlCommand("Update employee set password=@password where emp_ID = @EmpId", con);
                con.Open();
                cmd.Parameters.AddWithValue("@EmpId", Empid);
                cmd.Parameters.AddWithValue("@password", txtpassword.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Pasword has been reset", "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new Login().Show();
            }
            else
                MessageBox.Show("Your password and confirm password should be the same", "Invalid password", MessageBoxButtons.OK, MessageBoxIcon.Stop);


        }

        private void Forget_Password_Load(object sender, EventArgs e)
        {
            txtConfirm.Visible = false;
            txtpassword.Visible = false;
            btnSubmit.Visible = false;
            lblconfirm.Visible = false;
            lblnewpassword.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
