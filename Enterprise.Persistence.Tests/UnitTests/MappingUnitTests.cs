using System;
using System.Collections.Generic;
using Enterprise.Model;
using Enterprise.Web;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Context;

namespace Enterprise.Persistence.Tests.UnitTests
{
    [TestClass]
    public class MappingUnitTests
    {

        // Nhibernate Mapping Unit Testing
        // https://github.com/jagregory/fluent-nhibernate/wiki/Persistence-specification-testing

        public ISession Session { get; set; }

        [TestInitialize]
        public void Init()
        {
            TestingSessionFactory.CreateSessionFactory("thread_static"); // Create a thread_static NHibernate Session
            AutofacConfig.RegisterAutofac(TestingSessionFactory.SessionFactory); // Send session to IoC Container
            CurrentSessionContext.Bind(TestingSessionFactory.Session); // Bind to Unit Test Context

            Session = HibernateConfig.SessionFactory.GetCurrentSession();
        }

        [TestCleanup]
        public void CleanUp()
        {
            HibernateConfig.Dispose();  // Release Session

        }

        [TestMethod]
        public void Mapping_CorrectlyMapStudent_ReturnsTrue()
        {
            new PersistenceSpecification<Student>(Session)
                .CheckProperty(x => x.Id,1)
                .CheckProperty(x => x.Birthday, new DateTime(2000,01,01))
                .CheckProperty(x => x.FullName, "Eric Whitmore")
                .CheckReference(x => x.Classroom, new Classroom() {Name = "New Classroom"})
                .VerifyTheMappings();
        }

        [TestMethod]
        public void Mapping_CorrectlyMapClassroom_ReturnsTrue()
        {
            new PersistenceSpecification<Classroom>(Session)
                .CheckProperty(x => x.Id, 1)
                .CheckProperty(x => x.Desks, 10)
                .CheckProperty(x => x.Name, "East Class")
                .CheckReference(x => x.Teacher, GetTeacher())
                .CheckList(x => x.Students, GetStudentList())
                .VerifyTheMappings();

       
        }

        [TestMethod]
        public void Mapping_CorrectlyMapTeacher_ReturnsTrue()
        {
            new PersistenceSpecification<Teacher>(Session)
                .CheckProperty(x => x.Id, 1)
                .CheckProperty(x => x.Birthday, new DateTime(2000,01,01))
                .CheckProperty(x => x.FullName, "Dr. Whitmore")
                .CheckReference(x => x.Classroom, new Classroom() {Name = "New Class"})
                .VerifyTheMappings();
        }

        private Teacher GetTeacher()
        {
            return new Teacher
            {
                FullName = "Dr. Whitmore",
                Birthday = new DateTime(2000,1,1)
            };
        }

        private IList<Student> GetStudentList()
        {
            var mockedStudents = new List<Student>();
            var mockedStudent1 = new Student
            {
                Birthday = new DateTime(2000, 01, 01),
                CreatedAt = DateTime.Now,
                FullName = "John Doe",
                IsDeleted = false,
                ModifiedAt = DateTime.Now
                
            };

            var mockedStudent2 = new Student
            {
                Birthday = new DateTime(2000, 02, 01),
                CreatedAt = DateTime.Now,
                FullName = "Jane Doe",
                IsDeleted = false,
                ModifiedAt = DateTime.Now
                
            };

            var mockedStudent3 = new Student
            {
                Birthday = new DateTime(2000, 03, 01),
                CreatedAt = DateTime.Now,
                FullName = "Sally Doe",
                IsDeleted = false,
                ModifiedAt = DateTime.Now
                
            };

            // This student is NOT active
            var mockedStudent4 = new Student
            {
                Birthday = new DateTime(2000, 04, 01),
                CreatedAt = DateTime.Now,
                FullName = "Jim Doe",
                IsDeleted = true,
                ModifiedAt = DateTime.Now
               
            };

            // Add 4 students to the list (3 active, 1 not active)
            mockedStudents.Add(mockedStudent1);
            mockedStudents.Add(mockedStudent2);
            mockedStudents.Add(mockedStudent3);
            mockedStudents.Add(mockedStudent4);

            return mockedStudents;
        }

    }
}
