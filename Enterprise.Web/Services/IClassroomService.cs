using System.Collections.Generic;
using Enterprise.Model;
using Enterprise.Web.Models;

namespace Enterprise.Web.Services
{
    public interface IClassroomService
    {
        /// <summary>
        /// Get Classroom by Id
        /// </summary>
        /// <param name="id">Classoom Id</param>
        /// <returns></returns>
        Classroom Get(int id);

        /// <summary>
        /// Get all classrooms, even those which are not active
        /// </summary>
        /// <returns></returns>
        List<Classroom> GetAll();

        /// <summary>
        /// Save Classroom
        /// </summary>
        /// <param name="classroom"></param>
        void Save(Classroom teacher);

        /// <summary>
        /// Update classroom
        /// </summary>
        /// <param name="classroom"></param>
        void Update(Classroom teacher);

        /// <summary>
        /// Update classroom from ClassroomDto
        /// </summary>
        /// <param name="dto">ClassroomDto</param>
        /// <returns></returns>
        Classroom Update(ClassroomDto dto);

        /// <summary>
        /// Remove classroom from the database
        /// </summary>
        /// <param name="id">Classoom Id</param>
        void HardDelete(int id);

        /// <summary>
        /// Deactivate classroom but do not remove it from the database
        /// </summary>
        /// <param name="id">Classoom Id</param>
        void SoftDelete(int id);

        /// <summary>
        /// Set teacher object for classroom
        /// </summary>
        /// <param name="classroomId"></param>
        /// <param name="teacherId"></param>
        void SetTeacher(int classroomId, int teacherId);

        /// <summary>
        /// Add Student to classroom
        /// </summary>
        /// <param name="classroomId"></param>
        /// <param name="studentId"></param>
        void AddStudent(int classroomId, int studentId);
    }
}