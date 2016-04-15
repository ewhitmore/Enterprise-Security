using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Enterprise.Model;
using Enterprise.Web.Models;
using Enterprise.Web.Services;

namespace Enterprise.Web.Controllers
{
    [RoutePrefix("api/v1/classroom")]
    public class ClassroomController : ApiController
    {
        public IClassroomService ClassroomService { get; set; }

        // Needed for unit testing
        public ClassroomController(IClassroomService classroomService)
        {
            ClassroomService = classroomService;
        }

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        [HttpGet]
        public ClassroomDto Get(int id)
        {
            return new ClassroomDto(ClassroomService.Get(id));
        }

        /// <summary>
        /// Get all students including ones that have been deleted
        /// </summary>
        /// <returns></returns>
        [Route("GetAll")]
        [HttpGet]
        public IList<ClassroomDto> GetAll()
        {

            return ClassroomService.GetAll().Select(classroom => new ClassroomDto(classroom)).ToList();
        }

        /// <summary>
        /// Create Student
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] ClassroomDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid Data");
            }

            var classroom = dto.ToClassroom();

            try
            {

                ClassroomService.Save(classroom);
            }
            catch (Exception exception)
            {

                return InternalServerError(exception);
            }

            return CreatedAtRoute("DefaultApi", new { controller = "v1/classroom", id = classroom.Id }, new ClassroomDto(classroom));
        }

        /// <summary>
        /// Update Student
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] ClassroomDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid Data");
            }

            Classroom classroom;

            try
            {
                classroom = ClassroomService.Update(dto);
            }
            catch (Exception exception)
            {

                return InternalServerError(exception);
            }

            return Ok(new ClassroomDto(classroom));

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
                ClassroomService.SoftDelete(id);
            }
            catch (Exception exception)
            {

                return InternalServerError(exception);
            }

            return Ok();
        }
    }
}
