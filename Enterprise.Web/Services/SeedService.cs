using System;
using System.Collections.Generic;
using Enterprise.Model;
using Enterprise.Persistence.Dao;

namespace Enterprise.Web.Services
{
    public class SeedService : ISeedService
    {
        public IClassroomDao ClassroomDao { private get; set; }

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
                Birthday = new DateTime(1776,2,22),
            };

            var student2 = new Student
            {
                FullName = "Abraham Lincoln",
                Birthday = new DateTime(1809, 2, 12),
            };

            var student3 = new Student
            {
                FullName = "Thomas Jefferson",
                Birthday = new DateTime(1776, 4, 13),
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


        }
    }
}