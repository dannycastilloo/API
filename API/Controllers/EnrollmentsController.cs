using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public EnrollmentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/Enrollments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollment()
        {
          if (_context.Enrollment == null)
          {
              return NotFound();
          }
            return await _context.Enrollment.ToListAsync();
        }

        // GET: api/Enrollments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> GetEnrollment(int id)
        {
          if (_context.Enrollment == null)
          {
              return NotFound();
          }
            var enrollment = await _context.Enrollment.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;
        }

        // PUT: api/Enrollments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnrollment(int id, Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return BadRequest();
            }

            _context.Entry(enrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Enrollments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
        {
          if (_context.Enrollment == null)
          {
              return Problem("Entity set 'SchoolContext.Enrollment'  is null.");
          }
            _context.Enrollment.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnrollment", new { id = enrollment.EnrollmentId }, enrollment);
        }

        // DELETE: api/Enrollments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            if (_context.Enrollment == null)
            {
                return NotFound();
            }
            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: ENROLL REQUEST
        [HttpPost]
        public async Task<IActionResult> Enroll(EnrollRequest request)
        {
            if (request == null || request.Courses == null || request.Courses.Count == 0)
            {
                return BadRequest("Invalid request. Please provide student ID and a list of courses to enroll.");
            }

            // Verifica si el estudiante existe
            var student = await _context.Students.FindAsync(request.IdStudent);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            foreach (var course in request.Courses)
            {
                var existingCourse = await _context.Courses.FindAsync(course.CourseId);
                if (existingCourse == null)
                {
                    return NotFound($"Course with ID {course.CourseId} not found.");
                }

                // Verifica si el estudiante ya está matriculado en el curso
                var isEnrolled = await _context.Enrollments
                    .AnyAsync(e => e.StudentId == student.StudentId && e.CourseId == existingCourse.CourseId);

                if (!isEnrolled)
                {
                    // Crea un nuevo objeto Enrollment
                    var enrollment = new Enrollment
                    {
                        Datetime = DateTime.Now,
                        StudentId = student.StudentId,
                        Student = student,
                        CourseId = existingCourse.CourseId,
                        Course = existingCourse
                    };

                    _context.Enrollments.Add(enrollment);
                }
                // Handle existing enrollments as needed
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = request.IdStudent }, student);
        }




        private bool EnrollmentExists(int id)
        {
            return (_context.Enrollment?.Any(e => e.EnrollmentId == id)).GetValueOrDefault();
        }
    }
}
