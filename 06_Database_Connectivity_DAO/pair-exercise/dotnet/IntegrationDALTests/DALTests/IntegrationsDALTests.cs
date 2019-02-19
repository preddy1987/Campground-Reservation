using System;
using System.Data.SqlClient;
using System.Transactions;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectDB.DAL;
using ProjectDB.Models;

namespace IntegrationDALTests
{
    [TestClass]
    public class UnitTest1
    {
        private TransactionScope tran;      //<-- used to begin a transaction during initialize and rollback during cleanup
        private string connectionString = @"Data Source=localhost\sqlexpress;Initial Catalog=EmployeeDB;Integrated Security=True";
        private int departmentId;                 //<-- used to hold the department id of the row created for our test


        // Set up the database before each test        
        [TestInitialize]
        public void Initialize()
        {
            // Initialize a new transaction scope. This automatically begins the transaction.
            tran = new TransactionScope();

        }

        // Cleanup runs after every single test
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back
        }

        #region Methods for testing

        [TestMethod]
        public void DepartmentTests()
        {

            // Open a SqlConnection object using the active transaction
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                DepartmentSqlDAL testDAL = new DepartmentSqlDAL(connectionString);
                IList<Department> testList = new List<Department>();
                testList = testDAL.GetDepartments();
                int savecount = testList.Count;
                Assert.AreNotEqual(0, testList.Count, "There should be Initial Data");

                Department testDepartment = new Department();
                testDepartment.Name = "alkjfagjhasivs";
                departmentId = testDAL.CreateDepartment(testDepartment);
                Assert.AreNotEqual(0,departmentId, "Deparment ID should be greater than 0");

                testDepartment = testDAL.GetADepartment(departmentId);
                Assert.AreEqual(departmentId, testDepartment.Id,"Department IDs should match");
                Assert.AreEqual("alkjfagjhasivs",testDepartment.Name, "Departname should be \"alkjfagjhasivs\"");

                testList = testDAL.GetDepartments();
                Assert.AreEqual(savecount+1, testList.Count, "List should have 1 more element");

                testDepartment.Name = "zzhzhzhzhzhzhz";
                testDepartment.Id = departmentId;
                bool status = testDAL.UpdateDepartment(testDepartment);
                Assert.AreEqual(true, status);
                testDepartment = testDAL.GetADepartment(departmentId);
                Assert.AreEqual("zzhzhzhzhzhzhz",testDepartment.Name);

            }
        }
        [TestMethod]
        public void EmployeeTests()
        {
            // Open a SqlConnection object using the active transaction
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                EmployeeSqlDAL testDAL = new EmployeeSqlDAL(connectionString);
                IList<Employee> testList = new List<Employee>();
                testList = testDAL.GetAllEmployees();
                int savecount = testList.Count;
                Assert.AreNotEqual(0, testList.Count, "There should be Initial Data");

                Employee testEmployee = new Employee();
                testEmployee.DepartmentId = 1;
                testEmployee.FirstName = "Roger";
                testEmployee.LastName = "Rabbit";
                testEmployee.JobTitle = "Cartoon Actor";
                testEmployee.BirthDate = Convert.ToDateTime("01/01/1981");
                testEmployee.Gender = "M";
                testEmployee.HireDate = Convert.ToDateTime("06/22/1988");

                SqlCommand command = new SqlCommand("Insert into employee (department_id,first_name,last_name,job_title,birth_date,gender,hire_date)"+
                    $" Values ('{testEmployee.DepartmentId}','{testEmployee.FirstName}','{testEmployee.LastName}','{testEmployee.JobTitle}','{testEmployee.BirthDate}','{testEmployee.Gender}','{testEmployee.HireDate}')", conn);
                command.ExecuteNonQuery();

                testList = testDAL.GetAllEmployees();
                Assert.AreEqual(savecount + 1, testList.Count, "List should have 1 more element");
                testList = testDAL.Search("Roger", "Rabbit");
                Assert.AreEqual(1,testList.Count,"Search should return 1");


            }

        }
        [TestMethod]
        public void ProjectTests()
        {
            // Open a SqlConnection object using the active transaction
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
            }
        }
        #endregion
    }
}
