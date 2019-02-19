using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{
    public class EmployeeSqlDAL
    {
        private string connectionString;
        private const string SqlSelectAllEmployees = "SELECT * FROM employee;";
        private const string SqlSearchForEmployees = "SELECT * FROM employee where first_name = @firstname and last_name = @lastname;";
        private const string SqlSearchForEmployeesWithoutProjects = "Select * from employee left outer join project_employee ON employee.employee_id = project_employee.employee_id where project_id is null;";

        // Single Parameter Constructor
        public EmployeeSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public IList<Employee> GetAllEmployees()
        {
            SqlCommand command = new SqlCommand(SqlSelectAllEmployees);

            return this.GetEmployeeHelper(command);
        }
        /// <summary>
        /// Searches the system for an employee by first name or last name.
        /// </summary>
        /// <remarks>The search performed is a wildcard search.</remarks>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns>A list of employees that match the search.</returns>
        public IList<Employee> Search(string firstname, string lastname)
        {
            SqlCommand command = new SqlCommand(SqlSearchForEmployees);
            command.Parameters.AddWithValue("@firstname", firstname);
            command.Parameters.AddWithValue("@lastname", lastname);

            return this.GetEmployeeHelper(command);
        }
        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public IList<Employee> GetEmployeesWithoutProjects()
        {
            SqlCommand command = new SqlCommand(SqlSearchForEmployeesWithoutProjects);
            return this.GetEmployeeHelper(command);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private IList<Employee> GetEmployeeHelper(SqlCommand cmd)
        {
            List<Employee> output = new List<Employee>();

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // SqlCommand cmd = new SqlCommand(sqlstring, connection);
                    cmd.Connection = connection;
                    // Execute the query to the database
                    SqlDataReader reader = cmd.ExecuteReader();

                    // The results come back as a SqlDataReader. Loop through each of the rows
                    // and add to the output list
                    while (reader.Read())
                    {
                        // Read in the value from the reader
                        // Reference by index or by column_name
                        Employee employee = new Employee();
                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        employee.HireDate = Convert.ToDateTime(reader["hire_date"]);
                        employee.Gender = Convert.ToChar(reader["gender"]).ToString();

                        // Add the employee to the output list                       
                        output.Add(employee);
                    }
                }
            }
            catch (SqlException ex)
            {
                // A SQL Exception Occurred. Log and throw to our application!!
                throw;
            }
            return output;
        }
    }
}
