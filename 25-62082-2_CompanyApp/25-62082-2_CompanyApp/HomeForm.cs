using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace _25_62082_2_CompanyApp
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Session.Username))
            {
                this.Text = "Dashboard - Welcome " + Session.Username;
            }
        }

        // 1. Existing View Users Button Logic
        private void btnViewUsers_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = DatabaseHelper.GetConnection())
                {
                    string query = "SELECT UserID, Username, Email, FullName, CreatedAt FROM dbo.Users";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);


                            MessageBox.Show($"Loaded {dt.Rows.Count} registered user(s).", "Users Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            frmEmployee empForm = new frmEmployee();
            empForm.ShowDialog();
        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                Session.Clear();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }

        private void HomeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Session.UserID == 0)
            {
                Application.Exit();
            }
        }

        private void HomeForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}