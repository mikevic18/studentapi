using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<SubjectsController> _logger;

        public SubjectsController(AppDbContext dbContext, ILogger<SubjectsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            try
            {
                var subjects = await _dbContext.Subjects.ToListAsync();
                return Ok(subjects);
            }
            catch (Exception ex)
            {
                // Log the exception if an error occurs while fetching subjects
                _logger.LogError($"An error occurred while fetching subjects: {ex}");
                return StatusCode(500, "An error occurred while fetching subjects.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(Subject subject)
        {
            try
            {
                _dbContext.Subjects.Add(subject);
                await _dbContext.SaveChangesAsync();
                return Ok(subject);
            }

            catch (Exception ex)
            {
                // Log the exception if an error occurs while adding a subject
                _logger.LogError($"An error occurred while adding a subject: {ex}");
                return StatusCode(500, "An error occurred while adding a subject.");
            }
        }

        [HttpPost("{subjectId}/topics")]
        public async Task<IActionResult> AddTopicToSubject(int subjectId, Topic topic)
        {
            try
            {
                var subject = await _dbContext.Subjects.FindAsync(subjectId);
                if (subject == null)
                {
                    return NotFound();
                }

                topic.SubjectId = subjectId;
                _dbContext.Topics.Add(topic);
                await _dbContext.SaveChangesAsync();
                return Ok(topic);
            }
            catch (Exception ex)
            {
                // Log the exception if an error occurs while adding a topic to a subject
                _logger.LogError($"An error occurred while adding a topic to a subject: {ex}");
                return StatusCode(500, "An error occurred while adding a topic to a subject.");
            }
        }

    }
}