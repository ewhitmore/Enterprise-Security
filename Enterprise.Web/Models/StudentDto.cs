using System;
using Enterprise.Model;

namespace Enterprise.Web.Models
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }

        public StudentDto()
        {
            
        }

        /// <summary>
        /// Convert Student to StudentDto
        /// </summary>
        /// <param name="student"></param>
        public StudentDto(Student student)
        {
            Id = student.Id;
            FullName = student.FullName;
            Birthday = student.Birthday;
        }

        /// <summary>
        /// Convert StudentDto to Student
        /// </summary>
        /// <returns></returns>
        public Student ToStudent()
        {
            var student = new Student
            {
                Birthday = Birthday,
                FullName = FullName,
                Id = Id
            };

            return student;
        }
    }
}