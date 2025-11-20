using EmployeeService.Models;
using EmployeeService.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.SqlClient;
using System.Net;
using System.ServiceModel.Web;

namespace UnitTestEmployeeService
{
    [TestClass]
    public class EmployeeServiceTests
    {
        [TestMethod]
        public void GetEmployeeById_ReturnsEmployee_WhenExists()
        {
            // Arrange
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(r => r.GetById("1")).Returns(new EmployeeDto { Id = 1, Name = "John" });
            var service = new EmployeeService.EmployeeService(mockRepo.Object);

            // Act
            var result = service.GetEmployeeById("1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("John", result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(WebFaultException<string>))]
        public void GetEmployeeById_ThrowsNotFound_WhenNull()
        {
            // Arrange
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(r => r.GetById("1")).Returns((EmployeeDto)null);
            var service = new EmployeeService.EmployeeService(mockRepo.Object);

            try
            {
                // Act
                service.GetEmployeeById("1");
            }
            catch (WebFaultException<string> ex)
            {
                // Assert
                Assert.AreEqual(HttpStatusCode.NotFound, ex.StatusCode);
                Assert.AreEqual("Employee not found", ex.Detail);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(WebFaultException<string>))]
        public void GetEmployeeById_ThrowsDatabaseError_WhenSqlException()
        {
            // Arrange
            var sqlEx = CreateSqlException();
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(r => r.GetById("1")).Throws(sqlEx);
            var service = new EmployeeService.EmployeeService(mockRepo.Object);

            try
            {
                // Act
                service.GetEmployeeById("1");
            }
            catch (WebFaultException<string> ex)
            {
                // Assert
                Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
                Assert.IsTrue(ex.Detail.StartsWith("Database error"));
                throw;
            }
        }

        [TestMethod]
        public void EnableEmployee_ReturnsDto_WhenSuccess()
        {
            // Arrange
            var dto = new EmployeeEnableDto { IsEnabled = true };
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(r => r.Enable("1", dto)); // no exception
            var service = new EmployeeService.EmployeeService(mockRepo.Object);

            // Act
            var result = service.EnableEmployee("1", dto);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsEnabled);
        }

        [TestMethod]
        [ExpectedException(typeof(WebFaultException<string>))]
        public void EnableEmployee_ThrowsNotFound_WhenInvalidOperation()
        {
            // Arrange
            var dto = new EmployeeEnableDto { IsEnabled = true };
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(r => r.Enable("1", dto)).Throws(new InvalidOperationException("Not found"));
            var service = new EmployeeService.EmployeeService(mockRepo.Object);

            try
            {
                // Act
                service.EnableEmployee("1", dto);
            }
            catch (WebFaultException<string> ex)
            {
                // Assert
                Assert.AreEqual(HttpStatusCode.NotFound, ex.StatusCode);
                Assert.AreEqual("Not found", ex.Detail);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(WebFaultException<string>))]
        public void EnableEmployee_ThrowsDatabaseError_WhenSqlException()
        {
            // Arrange
            var sqlEx = CreateSqlException();
            var dto = new EmployeeEnableDto { IsEnabled = true };
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(r => r.Enable("1", dto)).Throws(sqlEx);
            var service = new EmployeeService.EmployeeService(mockRepo.Object);

            try
            {
                // Act
                service.EnableEmployee("1", dto);
            }
            catch (WebFaultException<string> ex)
            {
                // Assert
                Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
                Assert.IsTrue(ex.Detail.StartsWith("Database error"));
                throw;
            }
        }

        private SqlException CreateSqlException()
        {
            try
            {
                var conn = new SqlConnection("Data Source=invalid");
                conn.Open();
            }
            catch (SqlException ex)
            {
                return ex;
            }

            return null;
        }
    }
}
