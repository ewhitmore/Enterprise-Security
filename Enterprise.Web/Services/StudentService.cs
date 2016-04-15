using System.Collections.Generic;
using System.Linq;
using Enterprise.Model;
using Enterprise.Persistence.Dao;
using Enterprise.Web.Models;

namespace Enterprise.Web.Services
{
    public class StudentService : IStudentService
    {
        public IStudentDao StudentDao { private get; set; }
        public IClassroomDao ClassroomDao { private get; set; }

        // required for unit testing
        public StudentService(IStudentDao studentDao, IClassroomDao classroomDao)
        {
            StudentDao = studentDao;
            ClassroomDao = classroomDao;
        }

        public Student Get(int id)
        {
            return StudentDao.Get(id);
        }

        public IList<Student> GetAll()
        {
            return StudentDao.GetAll().ToList();
        }

        public IList<Student> FindAll()
        {
            return StudentDao.FindAll().ToList();
        }

        public IList<Student> GetActive()
        {
            return StudentDao.FindAll().Where(student => student.IsDeleted == false).ToList();
        }

        public void Save(Student student)
        {
            StudentDao.Save(student);
        }

        public void Update(Student student)
        {
            StudentDao.Update(student);
        }

        public Student Update(StudentDto dto)
        {

            var student = StudentDao.Get(dto.Id);

            student.FullName = dto.FullName;
            student.Birthday = dto.Birthday;
            
            StudentDao.Update(student);

            return student;
        }

        public void HardDelete(int id)
        {
            StudentDao.Delete(StudentDao.Get(id));
        }

        public void SoftDelete(int id)
        {
            var student = StudentDao.Get(id);
            student.IsDeleted = true;
            StudentDao.Update(student);
        }

        public void SetClassroom(int studentId, int classroomId)
        {
            var student = StudentDao.Get(studentId);
            var classroom = ClassroomDao.Get(classroomId);

            student.Classroom = classroom;
            StudentDao.Update(student);
        }
    }
}