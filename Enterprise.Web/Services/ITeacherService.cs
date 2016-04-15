using System.Collections.Generic;
using Enterprise.Model;
using Enterprise.Web.Models;

namespace Enterprise.Web.Services
{
    public interface ITeacherService
    {
        /// <summary>
        /// Get teacher by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Teacher Get(int id);

        /// <summary>
        /// Get all teachers, even those which have been deactived
        /// </summary>
        /// <returns></returns>
        List<Teacher> GetAll();

        /// <summary>
        /// Save Teacher
        /// </summary>
        /// <param name="teacher"></param>
        void Save(Teacher teacher);

        /// <summary>
        /// Update teacher
        /// </summary>
        /// <param name="teacher"></param>
        void Update(Teacher teacher);

        /// <summary>
        /// Update teacher from TeacherDto
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        Teacher Update(TeacherDto teacher);

        /// <summary>
        /// Remove Teacher from database
        /// </summary>
        /// <param name="id"></param>
        void HardDelete(int id);

        /// <summary>
        /// Deactivate teacher but do not remove teacher from the database
        /// </summary>
        /// <param name="id"></param>
        void SoftDelete(int id);

        /// <summary>
        /// Set classroom for teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="classroomId"></param>
        void SetClassroom(int teacherId, int classroomId);
    }

}