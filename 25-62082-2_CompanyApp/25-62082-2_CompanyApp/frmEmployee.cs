using System;
using System.Data;
using System.Windows.Forms;

namespace _25_62082_2_CompanyApp
{
    public partial class frmEmployee : Form
    {
        public frmEmployee()
        {
            InitializeComponent();
        }

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            try
            {
                DataTable dt = Employee.GetAllEmployees();
                dgvEmployees.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmpId.Text) || string.IsNullOrWhiteSpace(txtEmpName.Text))
            {
                MessageBox.Show("Employee ID and Name are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtEmpAge.Text, out int age))
            {
                MessageBox.Show("Please enter a valid age.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool success = Employee.AddEmployee(
                txtEmpId.Text.Trim(),
                txtEmpName.Text.Trim(),
                age,
                txtEmpContact.Text.Trim(),
                cmbGender.SelectedItem?.ToString(),
                Session.UserID > 0 ? (int?)Session.UserID : null
            );

            if (success)
            {
                MessageBox.Show("Employee added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadEmployeeData();
                ClearInputs();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmpId.Text))
            {
                MessageBox.Show("Select an Employee ID to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int.TryParse(txtEmpAge.Text, out int age);

            bool success = Employee.UpdateEmployee(
                txtEmpId.Text.Trim(),
                txtEmpName.Text.Trim(),
                age,
                txtEmpContact.Text.Trim(),
                cmbGender.SelectedItem?.ToString()
            );

            if (success)
            {
                MessageBox.Show("Employee updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadEmployeeData();
                ClearInputs();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmpId.Text))
            {
                MessageBox.Show("Select an Employee ID to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool success = Employee.DeleteEmployee(txtEmpId.Text.Trim());
                if (success)
                {
                    MessageBox.Show("Employee deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEmployeeData();
                    ClearInputs();
                }
            }
        }

        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvEmployees.Rows[e.RowIndex];
                txtEmpId.Text = row.Cells["EmpId"].Value?.ToString();
                txtEmpName.Text = row.Cells["EmpName"].Value?.ToString();
                txtEmpAge.Text = row.Cells["EmpAge"].Value?.ToString();
                txtEmpContact.Text = row.Cells["EmpContact"].Value?.ToString();
                cmbGender.Text = row.Cells["EmpGender"].Value?.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtEmpId.Clear();
            txtEmpName.Clear();
            txtEmpAge.Clear();
            txtEmpContact.Clear();
            cmbGender.SelectedIndex = -1;
            txtEmpId.Focus();
        }
        private void txtEmpId_TextChanged(object sender, EventArgs e) { }
        private void txtEmpContact_TextChanged(object sender, EventArgs e) { }
        private void txtEmpAge_TextChanged(object sender, EventArgs e) { }
        private void txtEmpName_TextChanged(object sender, EventArgs e) { }
        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}