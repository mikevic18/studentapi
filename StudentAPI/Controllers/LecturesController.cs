using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class LecturesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<LecturesController> _logger;
    public LecturesController(AppDbContext context, ILogger<LecturesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetLecturesWithTopics()
    {
        try
        {
            _logger.LogInformation("GetLecturesWithTopics endpoint was called.");

            var subjects = await _context.Subjects
                .Include(s => s.Topics)
                .Select(s => new
                {
                    subjectId = s.SubjectId,
                    title = s.Title,
                    imageUrl = s.ImageUrl,
                    duration = s.Duration,
                    topics = s.Topics.Select(t => new
                    {
                        topicId = t.TopicId,
                        title = t.Title
                    }).ToList()
                })
                .ToListAsync();

            return Ok(subjects);
        }
        catch (DbUpdateException ex)
        {
            // Handle SQL-related errors (e.g., duplicate keys, foreign key violations)
            _logger.LogError($"An error occurred while updating the database: {ex}");

            // Extract the SQL-specific error information for a more detailed response
            var sqlException = ex.GetBaseException() as SqlException;
            if (sqlException != null)
            {
                var errorMessages = new StringBuilder();
                foreach (SqlError err in sqlException.Errors)
                {
                    errorMessages.AppendLine($"Error Number: {err.Number}, Message: {err.Message}");
                }

                return StatusCode(500, $"An error occurred while updating the database: {errorMessages}");
            }

            return StatusCode(500, "An error occurred while updating the database.");
        }
        catch (Exception ex)
        {
            // Log the exception if an error occurs while fetching lectures with topics
            _logger.LogError($"An error occurred while fetching lectures with topics: {ex}");
            return StatusCode(500, "An error occurred while fetching lectures with topics.");
        }
    }
}

