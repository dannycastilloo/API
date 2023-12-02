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
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
          if (_context.Students == null)
          {
              return NotFound();
          }
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
          if (_context.Students == null)
          {
              return NotFound();
          }
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // GET BY NAME & GRADE: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentByNameAndGrade(int id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.FirstName))
            {
                return BadRequest("Name is required.");
            }

            var newStudent = new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Phone = student.Phone,
                Email = student.Email,
                GradeId = student.GradeId
            };

            if (_context.Students == null)
            {
                return Problem("Entity set 'SchoolContext.Student' is null.");
            }

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = newStudent.StudentId }, newStudent);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // UPDATE PERSONAL DATA
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentPersonalData(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            var existingStudent = await _context.Students.FindAsync(id);

            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            _context.Entry(existingStudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // UPDATE CONTACT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentContact(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            var existingStudent = await _context.Students.FindAsync(id);

            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.Phone = student.Phone;
            existingStudent.Email = student.Email;
            _context.Entry(existingStudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Student>>> PostStudentsByGrade(StudentByGradeRequest request)
        {
            if (request == null || request.Students == null || request.Students.Count == 0)
            {
                return BadRequest("Invalid request. Please provide students and a grade ID.");
            }

            var grade = await _context.Grades.FindAsync(request.IdGrade);
            if (grade == null)
            {
                return NotFound("Grade not found.");
            }

            foreach (var student in request.Students)
            {
                student.GradeId = request.IdGrade;
                _context.Students.Add(student);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudents", request.Students);
        }



        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }
    }
}
