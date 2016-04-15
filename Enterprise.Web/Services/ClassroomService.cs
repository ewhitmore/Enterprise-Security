using System.Collections.Generic;
using System.Linq;
using Enterprise.Model;
using Enterprise.Persistence.Dao;
using Enterprise.Web.Models;

namespace Enterprise.Web.Services
{
    public class ClassroomService : IClassroomService
    {
        public IClassroomDao ClassroomDao { private get;  set; }
        public ITeacherDao TeacherDao { private get; set; }
        public IStudentDao StudentDao { private get; set; }

        // Required for unit testing
        public ClassroomService(IClassroomDao classroomDao, ITeacherDao teacherDao, IStudentDao studentDao)
        {
            ClassroomDao = classroomDao;
            TeacherDao = teacherDao;
            StudentDao = studentDao;
        }


        public Classroom Get(int id)
        {
            return ClassroomDao.Get(id);
        }


        public List<Classroom> GetAll()
        {
            return ClassroomDao.GetAll().ToList();
        }


        public void Save(Classroom classroom)
        {
            ClassroomDao.Save(classroom);
        }


        public void Update(Classroom classroom)
        {
            ClassroomDao.Update(classroom);
        }


        public Classroom Update(ClassroomDto dto)
        {
            var classroom = ClassroomDao.Get(dto.Id);

            classroom.Teacher = dto.Teacher.ToTeacher();
            classroom.Students = dto.Students.Select(studentDto => studentDto.ToStudent()).ToList();
            classroom.Desks = dto.Desks;
            classroom.Name = dto.Name;

            ClassroomDao.Update(classroom);

            return classroom;
        }

        public void HardDelete(int id)
        {
            ClassroomDao.Delete(ClassroomDao.Get(id));
        }


        public void SoftDelete(int id)
        {
            var classroom = ClassroomDao.Get(id);
            classroom.IsDeleted = true;
            ClassroomDao.Update(classroom);
        }

        public void SetTeacher(int classroomId, int teacherId)
        {
            var classroom = ClassroomDao.Get(classroomId);
            var teacher = TeacherDao.Get(teacherId);

            classroom.Teacher = teacher;
            ClassroomDao.Update(classroom);
        }

        public void AddStudent(int classroomId, int studentId)
        {
            var classroom = ClassroomDao.Get(classroomId);
            var student = StudentDao.Get(studentId);

            classroom.Students.Add(student);
            ClassroomDao.Update(classroom);
        }

    }
}