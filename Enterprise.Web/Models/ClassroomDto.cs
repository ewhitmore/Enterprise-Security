using System.Collections.Generic;
using System.Linq;
using Enterprise.Model;


namespace Enterprise.Web.Models
{

    public class ClassroomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Desks { get; set; }
        public TeacherDto Teacher { get; set; }
        public List<StudentDto> Students { get; set; }



        public ClassroomDto()
        {
            
        }

        /// <summary>
        /// Convert Classroom to ClassroomDto
        /// </summary>
        /// <param name="classroom"></param>
        public ClassroomDto(Classroom classroom)
        {

            Id = classroom.Id;
            Name = classroom.Name;
            Desks = classroom.Desks;
            Teacher = new TeacherDto(classroom.Teacher);

            // Convert Student objects to StudentDto objects
            Students = classroom.Students.Select(student => new StudentDto(student)).ToList();

        }

        /// <summary>
        /// Convert ClassroomDto to Classroom
        /// </summary>
        /// <returns></returns>
        public Classroom ToClassroom()
        {
            var classroom = new Classroom
            {
                Id = Id,
                Name = Name,
                Desks = Desks,
                Teacher = Teacher.ToTeacher(),

                // Convert StudentDto objects to Student Objects
                Students = Students.Select(studentDto => studentDto.ToStudent()).ToList()
            };

            return classroom;
        }
    }
}