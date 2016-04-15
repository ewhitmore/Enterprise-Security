using System;
using System.Collections.Generic;
using Enterprise.Model;
using Enterprise.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Enterprise.Web.Tests.Models
{
    [TestClass]
    public class StudentDtoUnitTests
    {

        private Classroom Classroom { get; set; }
        private List<Student> Students { get; set; }
        private Teacher Teacher { get; set; }

        [TestInitialize]
        public void Init()
        {
            Teacher = new Teacher {FullName = "Jane Doe", Birthday = new DateTime(1980, 1, 1), Id = 1};
            Students = new List<Student>
            {
                new Student {FullName = "Dick", Birthday = new DateTime(2000,12,25), Id = 1 },
                new Student {FullName = "Jane", Birthday = new DateTime(2001,12,25), Id = 2 },
                new Student {FullName = "Sally", Birthday = new DateTime(2002,12,25), Id = 3 }
            };

            Classroom = new Classroom()
            {
                Id = 1,
                Name = "CPS 200",
                Desks = 30,
                IsDeleted = false,
                Teacher = Teacher,
                Students = Students
            };
        }

        [TestMethod]
        public void StudentDto_ConvertStudentToStudentDto_ReturnsTrue()
        {
            // Arrange
            
            // Act
            var dto = new StudentDto(Students[0]);

            // Assert
            Assert.AreEqual(Students[0].Id,dto.Id);
        }

        [TestMethod]
        public void StudentDto_ConvertStudentDtoToStudent_ReturnsTrue()
        {
            // Arrange

            // Act
            var dto = new StudentDto(Students[0]);
            var newStudent = dto.ToStudent();

            // Assert
            Assert.AreEqual(Students[0].Id, newStudent.Id);
        }
    }
}
