using System.Collections.Generic;
using Enterprise.Model;
using Enterprise.Web.Models;

namespace Enterprise.Web.Services
{
    public interface IStudentService
    {
        /// <summary>
        /// Get Student by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Student Get(int id);
        
        /// <summary>
        /// Get all students include inactive ones
        /// </summary>
        /// <returns></returns>
        IList<Student> GetAll();

        /// <summary>
        /// Get students based on linq query.
        /// </summary>
        /// <returns></returns>
        IList<Student> FindAll();

        /// <summary>
        /// Get active students
        /// </summary>
        /// <returns></returns>
        IList<Student> GetActive();

        /// <summary>
        /// Save student record in database
        /// </summary>
        /// <param name="student"></param>
        void Save(Student student);

        /// <summary>
        /// Update student record in database
        /// </summary>
        /// <param name="student"></param>
        void Update(Student student);

        /// <summary>
        /// Update student record from StudentDto
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Student Update(StudentDto dto);

        /// <summary>
        /// Remove student from the database
        /// </summary>
        /// <param name="id"></param>
        void HardDelete(int id);

        /// <summary>
        /// Deactivate student but do not remove it from the database
        /// </summary>
        /// <param name="id"></param>
        void SoftDelete(int id);

        /// <summary>
        /// Add student to classroom
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="classroomId"></param>
        void SetClassroom(int studentId, int classroomId);
    }
}