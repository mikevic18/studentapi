using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentAPI.Models;


namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentInfoController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<StudentInfoController> _logger;
        public StudentInfoController(AppDbContext dbContext, ILogger<StudentInfoController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]

        public IActionResult GetStudentsWithSubjects()
        {
            var studentsWithSubjects = _dbContext.Students
                        .Select(student => new
                        {
                            student.StudentId,
                            student.FirstName,
                            student.SecondName,
                            student.Title,
                            student.Email,
                            SubjectIds = _dbContext.StudentSubjects
                                .Where(ss => ss.StudentId == student.StudentId)
                                .Select(ss => ss.SubjectId)
                                .ToList()
                        })
                        .ToList(); ;

            return Ok(studentsWithSubjects);
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentWithCompletedTopics(int studentId)
        {
            try
            {
                var student = await _dbContext.Students
                    .Include(s => s.CompletedTopics)
                    .Where(s => s.StudentId == studentId)
                    .Select(s => new
                    {
                        studentId = s.StudentId,
                        topics = s.CompletedTopics.Select(ct => new
                        {
                            topicId = ct.TopicId,
                            isComplete = ct.IsComplete
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (student == null)
                {
                    // Log the error if student is not found
                    _logger.LogError($"Student with ID {studentId} not found.");
                    return NotFound();
                }

                // Log successful request
                _logger.LogInformation($"Successfully retrieved student with ID {studentId}.");
                return Ok(student);
            }
            catch (Exception ex)
            {
                // Log the exception if any other error occurs
                _logger.LogError($"An error occurred while processing the request: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentWithSubjectsRequest request)
        {
            if (ModelState.IsValid)
            {
                var newStudent = new Student
                {
                    FirstName = request.FirstName,
                    SecondName = request.SecondName,
                    Title = request.Title,
                    Email = request.Email,
                    Dob = request.Dob
                };

                _dbContext.Students.Add(newStudent);
                await _dbContext.SaveChangesAsync();

                if (request.SubjectIds != null && request.SubjectIds.Any())
                {
                    var studentId = newStudent.StudentId;

                    // Verify that provided subjectIds exist in the "subjects" table
                    var existingSubjects = await _dbContext.Subjects
                        .Where(s => request.SubjectIds.Contains(s.SubjectId))
                        .Select(s => s.SubjectId)
                        .ToListAsync();

                    var nonExistingSubjects = request.SubjectIds.Except(existingSubjects).ToList();
                    if (nonExistingSubjects.Any())
                    {
                        return BadRequest($"Subjects with Ids {string.Join(",", nonExistingSubjects)} do not exist.");
                    }


                    // Add student subjects
                    foreach (var subjectId in request.SubjectIds)
                    {
                        var studentSubject = new StudentSubject
                        {
                            StudentId = studentId,
                            SubjectId = subjectId
                        };

                        _dbContext.StudentSubjects.Add(studentSubject);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost("{studentId}/update-topic")]
        public async Task<IActionResult> UpdateTopicStatus(int studentId, [FromBody] TopicStatusRequest request)
        {
            try
            {
                var student = await _dbContext.Students
                    .Include(s => s.CompletedTopics)
                    .FirstOrDefaultAsync(s => s.StudentId == studentId);

                if (student == null)
                {
                    return NotFound();
                }

                if (request == null || request.TopicId <= 0)
                {
                    // Invalid request, log the error and return a bad request response
                    _logger.LogError("Invalid request data or TopicId.");
                    return BadRequest("Invalid request data or TopicId.");
                }

                var completedTopic = student.CompletedTopics.FirstOrDefault(ct => ct.TopicId == request.TopicId);
                if (completedTopic == null)
                {
                    // Create a new CompletedTopic if it doesn't exist
                    completedTopic = new CompletedTopic
                    {
                        StudentId = studentId,
                        TopicId = request.TopicId,
                        IsComplete = request.IsComplete
                    };
                    _dbContext.CompletedTopics.Add(completedTopic);
                }
                else
                {
                    // Update the IsComplete status if it exists
                    completedTopic.IsComplete = request.IsComplete;
                    _dbContext.CompletedTopics.Update(completedTopic);
                }

                await _dbContext.SaveChangesAsync();

                // Log successful update
                _logger.LogInformation($"Successfully updated topic status for student with ID {studentId}.");
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception if any other error occurs
                _logger.LogError($"An error occurred while processing the request: {ex}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}

