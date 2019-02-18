using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{
    public class ProjectSqlDAL
    {
        private string connectionString;
        private const string SqlSelectAllProjects = "SELECT * FROM project;";
        private const string SqlInsertProject = "INSERT INTO project (name,from_date,to_date) VALUES (@name, @startdate, @enddate);";
        private const string SqlGetLastProjecttId = "SELECT MAX(project_id) FROM project;";
        private const string SqlRemoveEmployeeFromProject = "Delete from project_employee where project_id = @project_id and employee_id = @employee_id;";
        private const string SqlAssignEmployeetoProject = "insert into project_employee (project_id,employee_id) values (@project_id,@employee_id);";

        // Single Parameter Constructor
        public ProjectSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public IList<Project> GetAllProjects()
        {
            List<Project> output = new List<Project>();

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlSelectAllProjects, connection);

                    // Execute the query to the database
                    SqlDataReader reader = cmd.ExecuteReader();

                    // The results come back as a SqlDataReader. Loop through each of the rows
                    // and add to the output list
                    while (reader.Read())
                    {
                        // Read in the value from the reader
                        // Reference by index or by column_name
                        Project project = new Project();
                        project.ProjectId = Convert.ToInt32(reader["project_id"]);
                        project.Name = Convert.ToString(reader["name"]);
                        project.StartDate = Convert.ToDateTime(reader["from_date"]);
                        project.EndDate = Convert.ToDateTime(reader["to_date"]);

                        // Add the department to the output list                       
                        output.Add(project);
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

        /// <summary>
        /// Assigns an employee to a project using their IDs.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlAssignEmployeetoProject, connection);
                    cmd.Parameters.AddWithValue("@project_id", projectId);
                    cmd.Parameters.AddWithValue("@employee_id", employeeId);
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();                    
                    SqlCommand cmd = new SqlCommand(SqlRemoveEmployeeFromProject, connection);
                    cmd.Parameters.AddWithValue("@project_id", projectId);
                    cmd.Parameters.AddWithValue("@employee_id", employeeId);
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }
    

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(SqlInsertProject, connection);
                    cmd.Parameters.AddWithValue("@name", newProject.Name);
                    cmd.Parameters.AddWithValue("@startdate", newProject.StartDate);
                    cmd.Parameters.AddWithValue("@enddate", newProject.EndDate);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(SqlGetLastProjecttId, connection);
                    int newId = Convert.ToInt32(cmd.ExecuteScalar());

                    return newId;
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

    }
}
