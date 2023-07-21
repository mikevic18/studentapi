using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentAPI.Models
{
    public class Subject
    {
        [Column("subjectid")]
        public int SubjectId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("imageurl")]
        public string ImageUrl { get; set; }
        [Column("duration")]
        public string Duration { get; set; }

        // Navigation property for the one-to-many relationship with Subtopics
        public List<Topic> Topics { get; set; }
    
    }
}
