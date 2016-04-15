using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Enterprise.Model;
using Enterprise.Web.Models;
using Enterprise.Web.Services;

namespace Enterprise.Web.Controllers
{
    [RoutePrefix("api/v1/student")]
    public class StudentController : ApiController
    {
        public IStudentService StudentService { get; set; }

        // Required for Unit Testing
        public StudentController(IStudentService studentService)
        {
            StudentService = studentService;
        }

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:int}")]
        [HttpGet]
        public StudentDto Get(int id)
        {
            return new StudentDto(StudentService.Get(id));
        }

        /// <summary>
        /// Get active students
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IList<StudentDto> GetActive()
        {
            return StudentService.GetActive().Select(student => new StudentDto(student)).ToList();
        }
        
        /// <summary>
        /// Get all students including ones that have been deleted
        /// </summary>
        /// <returns></returns>
        [Route("GetAll")]
        [HttpGet]
        public IList<StudentDto> GetAll()
        {
            return StudentService.GetAll().Select(student => new StudentDto(student)).ToList();
        }
        
        /// <summary>
        /// Create Student
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] StudentDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid Data");
            }

            var student = dto.ToStudent();

            try
            {
                
                StudentService.Save(student);
            }
            catch (Exception exception)
            {

                return InternalServerError(exception);
            }

            return CreatedAtRoute("DefaultApi", new { controller = "v1/student",  id = student.Id }, new StudentDto(student));
        }

        /// <summary>
        /// Update Student
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        public IHttpActionResult Update([FromBody] StudentDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid Data");
            }

            Student student;

            try
            {
                 student = StudentService.Update(dto);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }

            return Ok(new StudentDto(student));

        }

        /// <summary>
        /// Delete Student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                StudentService.SoftDelete(id);
            }
            catch (Exception exception)
            {

                return InternalServerError(exception);
            }

            return Ok();
        }
    }
}
