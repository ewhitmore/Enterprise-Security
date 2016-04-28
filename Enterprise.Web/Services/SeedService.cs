using System;
using System.Collections.Generic;
using Enterprise.Model;
using Enterprise.Persistence;
using Enterprise.Persistence.Dao;
using Enterprise.Persistence.Model;
using Enterprise.Web.Utils;
using Microsoft.AspNet.Identity;
using NHibernate.AspNet.Identity;

namespace Enterprise.Web.Services
{
    public class SeedService : ISeedService
    {
        public IClassroomDao ClassroomDao { private get; set; }
        public IClientDao ClientDao { private get; set; }

        public ISecurityService SecurityService { get; set; }

        public void Seed()
        {

            var teacher = new Teacher()
            {
                Birthday = new DateTime(1982, 01, 01),
                FullName = "Jane Doe",

            };

            var student1 = new Student
            {
                FullName = "George Washington",
                Birthday = new DateTime(1801,2,22),
            };

            var student2 = new Student
            {
                FullName = "Abraham Lincoln",
                Birthday = new DateTime(1809, 2, 12),
            };

            var student3 = new Student
            {
                FullName = "Thomas Jefferson",
                Birthday = new DateTime(1801, 4, 13),
            };
            var student4 = new Student
            {
                FullName = "John F. Kennedy",
                Birthday = new DateTime(1917, 5, 29),
            };


            var students = new List<Student>();
            students.Add(student1);
            students.Add(student2);
            students.Add(student3);
            students.Add(student4);

            var classroom = new Classroom
            {
                Desks = 20,
                Name = "South",
                Teacher = teacher,
                Students = students
            };

            // Needed for Nagivation
            teacher.Classroom = classroom;

            ClassroomDao.Save(classroom);


            var client = new Client()
            {
                ClientId = "AngularWebClient",
                Secret = Helper.GetHash("abc@123"),
                Name = "AngularJS front-end Application",
                ApplicationType = ApplicationTypes.JavaScript,
                Active = true,
                RefreshTokenLifeTime = 7200,
                AllowedOrigin = "*"
            };

            ClientDao.Save(client);

            SecurityService.CreateUser("user@localhost", "ewhitmore", "ewhitmore");
        }
    }
}