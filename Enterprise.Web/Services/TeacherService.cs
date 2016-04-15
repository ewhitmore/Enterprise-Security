using System.Collections.Generic;
using System.Linq;
using Enterprise.Model;
using Enterprise.Persistence.Dao;
using Enterprise.Web.Models;

namespace Enterprise.Web.Services
{
    public class TeacherService : ITeacherService
    {
        protected ITeacherDao TeacherDao { private get; set; }
        protected IClassroomDao ClassroomDao { private get; set; }

        // Required for unit testings
        public TeacherService(ITeacherDao teacherDao, IClassroomDao classroomDao)
        {
            TeacherDao = teacherDao;
            ClassroomDao = classroomDao;
        }

        public Teacher Get(int id)
        {
            return TeacherDao.Get(id);
        }

        public List<Teacher> GetAll()
        {
            return TeacherDao.GetAll().ToList();
        }

        public void Save(Teacher teacher)
        {
            TeacherDao.Save(teacher);
        }

        public void Update(Teacher teacher)
        {
            TeacherDao.Update(teacher);
        }

        public Teacher Update(TeacherDto dto)
        {

            var teacher = TeacherDao.Get(dto.Id);

            teacher.Birthday = dto.Birthday;
            teacher.FullName = dto.FullName;

            TeacherDao.Update(teacher);

            return teacher;
        }

        public void HardDelete(int id)
        {
            TeacherDao.Delete(TeacherDao.Get(id));
        }

        public void SoftDelete(int id)
        {
            var teacher = TeacherDao.Get(id);
            teacher.IsDeleted = true;
            TeacherDao.Update(teacher);
        }

        public void SetClassroom(int teacherId, int classroomId)
        {
            var teacher = TeacherDao.Get(teacherId);
            var classroom = ClassroomDao.Get(classroomId);

            teacher.Classroom = classroom;
            TeacherDao.Update(teacher);
        }
    }
}