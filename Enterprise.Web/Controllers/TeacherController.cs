using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Enterprise.Model;
using Enterprise.Web.Models;
using Enterprise.Web.Services;

namespace Enterprise.Web.Controllers
{
    [RoutePrefix("api/v1/teacher")]
    public class TeacherController : ApiController
    {
        public ITeacherService TeacherService { get; set; }

        // Needed for Unit Testing
        public TeacherController(ITeacherService teacherService)
        {
            TeacherService = teacherService;
        }

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        [HttpGet]
        public TeacherDto Get(int id)
        {
            return new TeacherDto(TeacherService.Get(id));
        }

        /// <summary>
        /// Get active students
        /// </summary>
        /// <returns></returns>
        //[Route("")]
        //public IList<ClassroomDto> GetActive()
        //{
        //    return ClassroomService.GetActive().Select(student => new ClassroomDto(student)).ToList();
        //}

        /// <summary>
        /// Get all students including ones that have been deleted
        /// </summary>
        /// <returns></returns>
        [Route("GetAll")]
        [HttpGet]
        public IList<TeacherDto> GetAll()
        {
            return TeacherService.GetAll().Select(teacher => new TeacherDto(teacher)).ToList();
        }

        /// <summary>
        /// Create Student
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] TeacherDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid Data");
            }

            var teacher = dto.ToTeacher();

            try
            {

                TeacherService.Save(teacher);
            }
            catch (Exception exception)
            {

                return InternalServerError(exception);
            }

            return CreatedAtRoute("DefaultApi", new { controller = "v1/teacher", id = teacher.Id }, new TeacherDto(teacher));
        }

        /// <summary>
        /// Update Student
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] TeacherDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid Data");
            }

            Teacher teacher;

            try
            {
                teacher = TeacherService.Update(dto);
            }
            catch (Exception exception)
            {

                return InternalServerError(exception);
            }

            return Ok(new TeacherDto(teacher));

        }

        /// <summary>
        /// Delete Classroom
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                TeacherService.SoftDelete(id);
            }
            catch (Exception exception)
            {

                return InternalServerError(exception);
            }

            return Ok();
        }
    }
}
