using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace _25_62082_2_CompanyApp
{
    public partial class ViewUsersForm : Form
    {
        public ViewUsersForm()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void ViewUsersForm_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();

                    string query =
                        @"SELECT UserID, Username, Email, FullName, CreatedAt
                          FROM Users
                          ORDER BY UserID";

                    using (SqlCommand command =
                           new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter =
                               new SqlDataAdapter(command))
                        {
                            DataTable table = new DataTable();

                            adapter.Fill(table);

                            dgvUsers.DataSource = table;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load users: " + ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewUsersForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}