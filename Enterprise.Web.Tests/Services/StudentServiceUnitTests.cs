using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Enterprise.Model;
using Enterprise.Persistence;
using Enterprise.Persistence.Dao;
using Enterprise.Web.Models;
using Enterprise.Web.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Enterprise.Web.Tests.Services
{
    [TestClass]
    public class StudentServiceUnitTests
    {

        [TestInitialize]
        public void Init()
        {
            AutofacConfig.RegisterAutofac();
        }

        // You should create unit tests for all methods within the service.
        // I have created a few examples below

        [TestMethod]
        public void StudentService_Get_1_ReturnsStudent()
        {

            // Arrange
            var mock = AutoMock.GetLoose(); // Create AutoFac Mocking container
            var mockedStudent = new Student
            {
                Birthday = new DateTime(2000,01,01),
                CreatedAt = DateTime.Now,
                FullName = "John Doe",
                IsDeleted = false,
                ModifiedAt = DateTime.Now,
                Id = 1,
                Classroom = new Classroom ()
            };

            mock.Mock<IStudentDao>().Setup(x => x.Get(1)).Returns(mockedStudent); // Return the above student when the Get method is called
            
            var studentService = mock.Create<StudentService>(); // build up The service

            // Act
            var student = studentService.Get(1); // Perform the action

            // Assert
            mock.Mock<IStudentDao>().Verify(x => x.Get(1)); // Verify that our Mock was used
            Assert.AreEqual(mockedStudent, student);  // Do the check

        }

        [TestMethod]
        public void StudentService_UpdateStudentFromDto_ReturnsTrue()
        {
            // Arrange
            var mock = AutoMock.GetLoose(); // Create AutoFac Mocking container
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

            mock.Mock<IStudentDao>().Setup(x => x.Get(1)).Returns(mockedStudent); // Return the above student when the Get method is called

            var studentService = mock.Create<StudentService>();
            var newBirthday = new DateTime(2012, 01, 01);  // Set what the new birthday will be

            var dto = new StudentDto(mockedStudent);
            dto.Birthday = newBirthday;


            // Act
            var student = studentService.Update(dto); // call update

            // Assert
            mock.Mock<IStudentDao>().Verify(x => x.Get(1));
            Assert.AreEqual(newBirthday, student.Birthday); // see if the DTO was able to update the Student Object with a new birthday

        }

        [TestMethod]
        public void StudentService_GetActive_Return3ActiveStudents()
        {
            // Arrange
            var mock = AutoMock.GetLoose();  // Create AutoFac Mocking container
            const int activeStudentCount = 3; // Define the number of active students

            var mockedStudents = new List<Student>();
            var mockedStudent1 = new Student {
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

            // This student is NOT active
            var mockedStudent4 = new Student
            {
                Birthday = new DateTime(2000, 04, 01),
                CreatedAt = DateTime.Now,
                FullName = "Jim Doe",
                IsDeleted = true,
                ModifiedAt = DateTime.Now,
                Id = 4,
                Classroom = new Classroom()
            };

            // Add 4 students to the list (3 active, 1 not active)
            mockedStudents.Add(mockedStudent1);
            mockedStudents.Add(mockedStudent2);
            mockedStudents.Add(mockedStudent3);
            mockedStudents.Add(mockedStudent4);

            // Act
            mock.Mock<IStudentDao>().Setup(x => x.FindAll()).Returns(mockedStudents.ToList().AsQueryable()); // Define IStudentDao's return values

            var studentService = mock.Create<StudentService>();
            var result = studentService.GetActive();

            // Assert
            mock.Mock<IStudentDao>().Verify(x => x.FindAll()); // Verify that the mock was called
            Assert.AreEqual(activeStudentCount, result.Count); // Test results

        }


    }
}
