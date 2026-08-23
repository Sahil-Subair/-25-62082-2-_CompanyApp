using System;
using System.Data;
using System.Data.SqlClient;

namespace _25_62082_2_LoginSystem
{
    public class Employee
    {
        public static bool AddEmployee(string id, string name, int age, string contact, string gender, int? createdBy)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO dbo.Emp_details (EmpId, EmpName, EmpAge, EmpContact, EmpGender, CreatedBy) 
                                VALUES (@EmpId, @EmpName, @EmpAge, @EmpContact, @EmpGender, @CreatedBy)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EmpId", id);
                    cmd.Parameters.AddWithValue("@EmpName", name);
                    cmd.Parameters.AddWithValue("@EmpAge", age);
                    cmd.Parameters.AddWithValue("@EmpContact", (object)contact ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EmpGender", (object)gender ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedBy", (object)createdBy ?? DBNull.Value);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static DataTable GetAllEmployees()
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {

                string query = @"SELECT 
                                    e.EmpId, 
                                    e.EmpName, 
                                    e.EmpAge, 
                                    e.EmpContact, 
                                    e.EmpGender, 
                                    u.Username AS CreatedBy 
                                 FROM dbo.Emp_details e 
                                 LEFT JOIN dbo.Users u ON e.CreatedBy = u.UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static bool UpdateEmployee(string id, string name, int age, string contact, string gender)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = @"UPDATE dbo.Emp_details 
                                SET EmpName = @EmpName, EmpAge = @EmpAge, EmpContact = @EmpContact, EmpGender = @EmpGender 
                                WHERE EmpId = @EmpId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EmpId", id);
                    cmd.Parameters.AddWithValue("@EmpName", name);
                    cmd.Parameters.AddWithValue("@EmpAge", age);
                    cmd.Parameters.AddWithValue("@EmpContact", (object)contact ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EmpGender", (object)gender ?? DBNull.Value);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool DeleteEmployee(string id)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "DELETE FROM dbo.Emp_details WHERE EmpId = @EmpId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EmpId", id);
                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}