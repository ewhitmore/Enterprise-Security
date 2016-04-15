using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Enterprise.Model;
using Enterprise.Persistence.Dao;
using Enterprise.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Context;

namespace Enterprise.Persistence.Tests.IntegrationTests
{
    [TestClass]
    public class TeacherDaoIntegrationTests
    {
        public ITeacherDao TeacherDao { get; set; }

        [TestInitialize]
        public void Init()
        {
            TestingSessionFactory.CreateSessionFactory("thread_static"); // Create a thread_static NHibernate Session
            AutofacConfig.RegisterAutofac(TestingSessionFactory.SessionFactory); // Send session to IoC Container
            CurrentSessionContext.Bind(TestingSessionFactory.Session); // Bind to Unit Test Context
            TeacherDao = AutofacConfig.Container.Resolve<ITeacherDao>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            HibernateConfig.Dispose();  // Release Session
        }

        [TestMethod]
        public void TeacherDao_SaveTeacher_ReturnsTrue()
        {
            // Arrange
            var teacherDao = AutofacConfig.Container.Resolve<ITeacherDao>();
            var teacher = new Teacher()
            {
                Birthday = new DateTime(Faker.RandomNumber.Next(1980, 2000), Faker.RandomNumber.Next(1, 12), Faker.RandomNumber.Next(1, 28)),
                FullName = Faker.Name.FullName()
            };

            // Act
            teacherDao.Save(teacher);

            // Assert
            Assert.AreEqual(1, teacher.Id);
        }

        [TestMethod]
        public void TeacherDao_GetAllTeachers_Returns5()
        {
            // Arrange
            SeedDatabase();

            // Act
            var teachers = TeacherDao.GetAll();

            // Assert
            Assert.AreEqual(5, teachers.Count);
        }

        [TestMethod]
        public void TeacherDao_FindTeacherGeorgeWashington_ReturnsTeacher()
        {
            // Arrange
            SeedDatabase();
            const string fullname = "George Washington";

            // Act
            var teachers = TeacherDao.FindAll().Where(teacher => teacher.FullName == fullname).ToList();

            // Assert
            Assert.AreEqual(1, teachers.Count());
            Assert.AreEqual(teachers[0].FullName, fullname);
        }

        [TestMethod]
        public void TeacherDao_UpdateTeacherGeorgeWashington_ReturnsTeacher()
        {
            // Arrange
            SeedDatabase();
            const string fullname = "Barack Obama";
            var teacher = TeacherDao.FindAll().First(x => x.FullName == "George Washington");
            
            // Act
            teacher.FullName = fullname;
            TeacherDao.Update(teacher);
            TeacherDao.Flush();

            var dbTeacher = TeacherDao.FindAll().First(x => x.FullName == fullname);

            // Assert
            Assert.AreEqual(fullname,teacher.FullName); // Test cache object
            Assert.AreEqual(fullname,dbTeacher.FullName); // Test database object
        }


        [TestMethod]
        public void TeacherDao_DeleteTeacherGeorgeWashington_Returns0()
        {
            // Arrange
            SeedDatabase();
            const string fullname = "George Washington";
            var teacher = TeacherDao.FindAll().First(x => x.FullName == fullname);

            // Act
            TeacherDao.Delete(teacher);
            TeacherDao.Flush();

            var teachers = TeacherDao.FindAll().Where(x => x.FullName == fullname);

            // Assert
            Assert.AreEqual(0, teachers.Count()); // Test cache object
           
        }

        private void SeedDatabase()
        {
            var teachers = new List<Teacher>()
            {
                new Teacher()
                {
                    Birthday =
                        new DateTime(Faker.RandomNumber.Next(1980, 2000), Faker.RandomNumber.Next(1, 12),
                            Faker.RandomNumber.Next(1, 28)),
                    FullName = Faker.Name.FullName()
                },

                new Teacher()
                {
                    Birthday =
                        new DateTime(Faker.RandomNumber.Next(1980, 2000), Faker.RandomNumber.Next(1, 12),
                            Faker.RandomNumber.Next(1, 28)),
                    FullName = Faker.Name.FullName()
                },

                new Teacher()
                {
                    Birthday =
                        new DateTime(Faker.RandomNumber.Next(1980, 2000), Faker.RandomNumber.Next(1, 12),
                            Faker.RandomNumber.Next(1, 28)),
                    FullName = Faker.Name.FullName()
                },

                new Teacher()
                {
                    Birthday =
                        new DateTime(Faker.RandomNumber.Next(1980, 2000), Faker.RandomNumber.Next(1, 12),
                            Faker.RandomNumber.Next(1, 28)),
                    FullName = Faker.Name.FullName()
                },

                new Teacher()
                {
                    Birthday = new DateTime(1982,01,01),
                    FullName = "George Washington"
                }
            };

            foreach (var teacher in teachers)
            {
                TeacherDao.Save(teacher);
            }
        }
    }
}