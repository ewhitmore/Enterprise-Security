using System;
using Enterprise.Model;

namespace Enterprise.Web.Models
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }

        public TeacherDto()
        {
            
        }

        /// <summary>
        /// Convert Teacher to TeacherDto
        /// </summary>
        /// <param name="teacher"></param>
        public TeacherDto(Teacher teacher)
        {
            Id = teacher.Id;
            FullName = teacher.FullName;
            Birthday = teacher.Birthday;
        }

        /// <summary>
        /// Convert TeacherDto to Teacher
        /// </summary>
        /// <returns></returns>
        public Teacher ToTeacher()
        {
            var teacher = new Teacher
            {
                Birthday = Birthday,
                FullName = FullName,
                Id = Id
            };

            return teacher;
        }
    }


}