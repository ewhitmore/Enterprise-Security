using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using Autofac.Extras.Moq;
using Autofac.Integration.WebApi;
using Enterprise.Model;
using Enterprise.Web.Controllers;
using Enterprise.Web.Models;
using Enterprise.Web.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Enterprise.Web.Tests.Controllers
{
    [TestClass]
    public class StudentControllerUnitTests
    {
        private HttpConfiguration _configuration;

        [TestInitialize]
        public void Init()
        {
            AutofacConfig.RegisterAutofac();

            _configuration = new HttpConfiguration
            {
                DependencyResolver = new AutofacWebApiDependencyResolver(AutofacConfig.Container)
            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            _configuration.Dispose();
        }

        [TestMethod]
        public void StudentController_Get_1_ReturnsStudent()
        {
            // Arrange
            var mock = AutoMock.GetLoose();

            var mockedStudent = new Student
            {
                Birthday = new DateTime(2000, 01, 01),
                CreatedAt = DateTime.Now,
                FullName = "John Doe",
                IsDeleted = false,
                ModifiedAt = DateTime.Now,
                Id = 1,
                Classroom = new Classroom()
            };

            var dto = new StudentDto(mockedStudent);

            mock.Mock<IStudentService>().Setup(x => x.Get(1)).Returns(mockedStudent); // Return the above student when the Get method is called
            var studentController = mock.Create<StudentController>(); // build up The service

            // Act
            var result = studentController.Get(1);

            // Assert
            mock.Mock<IStudentService>().Verify(x => x.Get(1)); // Verify that our Mock was used
            Assert.AreEqual(dto.Id, result.Id);  // Do the check

        }

        [TestMethod]
        public void StudentController_GetActive_ReturnsStudents()
        {
            // Arrange
            var mock = AutoMock.GetLoose();

            var mockedStudents = new List<Student>();
            var mockedStudent1 = new Student
            {
                Birthday = new DateTime(2000, 01, 01),
                CreatedAt = DateTime.Now,
                FullName = "John Doe",
                IsDeleted = false,
                ModifiedAt = DateTime.Now,
                Id = 1,
                Classroom = new Classroom()
            };

            var mockedStudent2 = new Student
            {
                Birthday = new DateTime(2000, 02, 01),
                CreatedAt = DateTime.Now,
                FullName = "Jane Doe",
                IsDeleted = false,
                ModifiedAt = DateTime.Now,
                Id = 2,
                Classroom = new Classroom()
            };

            var mockedStudent3 = new Student
            {
                Birthday = new DateTime(2000, 03, 01),
                CreatedAt = DateTime.Now,
                FullName = "Sally Doe",
                IsDeleted = false,
                ModifiedAt = DateTime.Now,
                Id = 3,
                Classroom = new Classroom()
            };

            // Add 4 students to the list (3 active, 1 not active)
            mockedStudents.Add(mockedStudent1);
            mockedStudents.Add(mockedStudent2);
            mockedStudents.Add(mockedStudent3);

            mock.Mock<IStudentService>().Setup(x => x.GetActive()).Returns(mockedStudents); // Return the above student when the Get method is called
            var studentController = mock.Create<StudentController>(); // build up The service

            // Act
            var result = studentController.GetActive();

            // Assert
            mock.Mock<IStudentService>().Verify(x => x.GetActive()); // Verify that our Mock was used
            Assert.AreEqual(mockedStudents[0].Id, result[0].Id);  // Do the check
            Assert.AreEqual(mockedStudents.Count, result.Count);
        }

        [TestMethod]
        public void StudentController_GetAll_ReturnsStudents()
        {
            // Arrange
            var mock = AutoMock.GetLoose();

            var mockedStudents = new List<Student>();
            var mockedStudent1 = new Student
            {
                Birthday = new DateTime(2000, 01, 01),
                CreatedAt = DateTime.Now,
                FullName = "John Doe",
                IsDeleted = false,
                ModifiedAt = DateTime.Now,
                Id = 1,
                Classroom = new Classroom()
            };

            var mockedStudent2 = new Student
            {
                Birthday = new DateTime(2000, 02, 01),
                CreatedAt = DateTime.Now,
                FullName = "Jane Doe",
                IsDeleted = false,
                ModifiedAt = DateTime.Now,
                Id = 2,
                Classroom = new Classroom()
            };

            var mockedStudent3 = new Student
            {
                Birthday = new DateTime(2000, 03, 01),
                CreatedAt = DateTime.Now,
                FullName = "Sally Doe",
                IsDeleted = false,
                ModifiedAt = DateTime.Now,
                Id = 3,
                Classroom = new Classroom()
            };

            // Add 4 students to the list (3 active, 1 not active)
            mockedStudents.Add(mockedStudent1);
            mockedStudents.Add(mockedStudent2);
            mockedStudents.Add(mockedStudent3);

            mock.Mock<IStudentService>().Setup(x => x.GetAll()).Returns(mockedStudents); // Return the above student when the Get method is called
            var studentController = mock.Create<StudentController>(); // build up The service

            // Act
            var result = studentController.GetAll();

            // Assert
            mock.Mock<IStudentService>().Verify(x => x.GetAll()); // Verify that our Mock was used
            Assert.AreEqual(mockedStudents[0].Id, result[0].Id);  // Do the check
            Assert.AreEqual(mockedStudents.Count, result.Count);
        }

        [TestMethod]
        public void StudentController_Create_ReturnsRoute()
        {

            // Arrange
            var mock = AutoMock.GetLoose();

            var mockedStudentDto = new StudentDto
            {
                Birthday = new DateTime(2000, 01, 01),
                FullName = "John Doe",
            };

            var studentController = mock.Create<StudentController>(); // build up The service

            // Act
            IHttpActionResult result = studentController.Create(mockedStudentDto);  // Submit Dto and get back a IHttpActionResult
            var createdResult = result as CreatedAtRouteNegotiatedContentResult<StudentDto>; // Pull the StudentDto out of IHttpActionResult

            // Assert
            Assert.AreEqual(typeof(CreatedAtRouteNegotiatedContentResult<StudentDto>), result.GetType());  // Check to make sure the result is a route which includes StudentDto type
            Assert.AreEqual(mockedStudentDto.FullName, createdResult?.Content.FullName); // Check StudentDto data
            Assert.AreEqual("DefaultApi", createdResult.RouteName); // Check to make sure we are using the correct route name
            Assert.AreEqual("v1/student", createdResult.RouteValues["controller"]); // make sure the controller returns the correct route

        }

        [TestMethod]
        public void StudentController_Create_ReturnsBadRequest()
        {
            // Arrange
            var mock = AutoMock.GetLoose();
            var studentController = mock.Create<StudentController>(); // build up The service

            // Act
            IHttpActionResult result = studentController.Create(null);  // Submit Dto and get back a IHttpActionResult
            var response = result as NegotiatedContentResult<BadRequestResult>;

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void StudentController_Update_ReturnsStudentDto()
        {
            // Arrange
            var mock = AutoMock.GetLoose();
            var studentController = mock.Create<StudentController>(); // build up The service

            var mockedStudentDto = new StudentDto
            {
                Id = 1,
                Birthday = new DateTime(2000, 01, 01),
                FullName = "John Doe",
            };

            mock.Mock<IStudentService>().Setup(x => x.Update(mockedStudentDto)).Returns(mockedStudentDto.ToStudent); // Return the above student when the Get method is called

            // Act
            IHttpActionResult result = studentController.Update(mockedStudentDto);  // Submit Dto and get back a IHttpActionResult
            var response = result as OkNegotiatedContentResult<StudentDto>;

            // Assert
            Assert.AreEqual(response?.Content.Id, mockedStudentDto.Id);
        }

        [TestMethod]
        public void StudentController_Update_ReturnsBadRequest()
        {
            // Arrange
            var mock = AutoMock.GetLoose();
            var studentController = mock.Create<StudentController>(); // build up The service

            // Act
            IHttpActionResult result = studentController.Update(null);  // Submit Dto and get back a IHttpActionResult

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void StudentController_Delete_ReturnOk()
        {
            // Arrange
            var mock = AutoMock.GetLoose();
            var studentController = mock.Create<StudentController>(); // build up The service

            // Act
            IHttpActionResult result = studentController.Delete(1);  // Submit Dto and get back a IHttpActionResult

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
