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
    public partial class Employee_Registration : Form
    {
        SqlConnection con = new
        SqlConnection(@"Server=DESKTOP-H12N5AH;Database=FinalProject;Trusted_Connection=true");
        //SqlConnection(@"Server=DESKTOP-H12N5AH;Database=FinalProject;Trusted_Connection=true");
        SqlCommand cmd;
        int EmpId;

        public Employee_Registration()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateData();
            txtId.Visible = false;

        }
        private void PopulateData()
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adapt;
            adapt = new SqlDataAdapter("select emp_ID, first_name, last_name,username,role,employment_date from employee", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }
               
        private void ClearControls()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtConfirm.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
            cbRole.SelectedItem = "";
            calEmploymentDate.SelectionStart = DateTime.Today;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtFirstName.Text != "" && txtLastName.Text !="" && txtUserName.Text != " " && txtPassword.Text != " " && txtConfirm.Text != " " && cbRole.Text != " ") //should I assume that name should be mandtory?
            {
                if (txtPassword.Text == txtConfirm.Text)
                {
                    cmd = new SqlCommand("insert into employee (first_name, last_name, username, password, role, employment_date) " +
                        "values(@first_name, @last_name, @username, @password, @role, @employment_date)", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@last_name", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@username", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@employment_date", calEmploymentDate.SelectionStart);
                    cmd.Parameters.AddWithValue("@role", cbRole.SelectedItem);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Registratoin Was Successfully Completed");
                    PopulateData();
                    ClearControls();
                }
                else
                {
                    MessageBox.Show("Password and Confirm Password Mismatch");

                }

            }
            else
            {
                MessageBox.Show("Please enter mandatory details!");
            }
        }
        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            txtUserName.Enabled = false;
            txtPassword.Enabled = false;
            txtConfirm.Enabled = false;
            if (txtFirstName.Text != "" && txtLastName.Text != "" && txtUserName.Text != " " && txtPassword.Text != " " && txtConfirm.Text != " " && cbRole.Text != " ") //should I assume that name should be mandtory?
            {
                cmd = new SqlCommand("Update employee set first_name=@first_name, last_name=@last_name," +
                    "role = @role, employment_date =@date where emp_ID = @EmpId", con);
                con.Open();
                cmd.Parameters.AddWithValue("@EmpId", EmpId);
                cmd.Parameters.AddWithValue("@first_name", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@last_name", txtLastName.Text);
                cmd.Parameters.AddWithValue("@role", cbRole.SelectedItem);
                cmd.Parameters.AddWithValue("@date", calEmploymentDate.SelectionStart);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Updated Successfully");
                PopulateData();
                ClearControls();
            }
            else
            {
                MessageBox.Show("Please enter mandatory details!");
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            txtUserName.Enabled = false;
            txtPassword.Enabled = false;
            txtConfirm.Enabled = false;

            if (dataGridView1.SelectedRows.Count != 0) // make sure user select at least 1 row
            {
                DataGridViewRow row = this.dataGridView1.SelectedRows[0];
                txtFirstName.Text = row.Cells[1].Value.ToString();
                txtLastName.Text = row.Cells[2].Value.ToString();
                EmpId = int.Parse(row.Cells[0].Value.ToString());
                txtUserName.Text = row.Cells[3].Value.ToString();
                txtId.Text = row.Cells[0].Value.ToString();
                cbRole.Text = row.Cells[4].Value.ToString();
                DateTime projectStart = new DateTime();
                projectStart = Convert.ToDateTime(row.Cells[5].Value.ToString());
                DateTime projectEnd = new DateTime();
                projectEnd = Convert.ToDateTime(row.Cells[5].Value.ToString());
                calEmploymentDate.SelectionRange = new SelectionRange(projectStart, projectEnd);
            }

        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (txtFirstName.Text != "")
            {
                cmd = new SqlCommand("Delete from employee where emp_ID = @EmpId", con);
                con.Open();
                cmd.Parameters.AddWithValue("@EmpId", EmpId);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Deleted Successfully");
                PopulateData();
                ClearControls();
            }
            else
            {
                MessageBox.Show("Please enter mandatory details!");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            cmd = new SqlCommand("select emp_ID, first_name, last_name,username,role,employment_date from employee where CONCAT(first_name, last_name) like '" + "%" +txtSearch.Text + "%'", con);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();
            dataGridView1.DataSource = dt;
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Sort(this.dataGridView1.Columns[1], ListSortDirection.Descending);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // creating Excel Application  
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application  
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            // creating new Excelsheet in workbook  
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            // see the excel sheet behind the program  
            app.Visible = true;
            // get the reference of first sheet. By default its name is Sheet1.  
            // store its reference to worksheet  
            worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];
            worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;
            // changing the name of active sheet  
            worksheet.Name = "Exported from gridview";    //current sheet name
            // storing header part in Excel  
            for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
            }
            // storing Each row and column value to excel sheet  
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)  //Row 
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++) //Column
                {
                    worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
            }
            try {
                // save the application == file name 
                workbook.SaveAs("C:\\output.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Saving Path is incorrect", "Error in file path", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            }


        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.ShowDialog();
                txtFile.Text = openFileDialog1.FileName;
                string filePath = txtFile.Text;
                DataTable dt = new DataTable();
                string[] lines = System.IO.File.ReadAllLines(filePath);

                if (lines.Length > 0)
                {
                    //first line to create header
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(',');
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }
                    //For Data
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] dataWords = lines[i].Split(',');
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dgvImport.DataSource = dt;
                }
            }
            catch(Exception exp)
            {
                MessageBox.Show("file name cannot be blank", "Error in Importing", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                PopulateData();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
