using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAPI.Models
{
    public class Topic
    {
        [Column("topicid")]
        public int TopicId { get; set; }
        [Column("title")]
        public string Title { get; set; }

        [Column("subjectid")]
        public int SubjectId { get; set; } // Foreign key to Subject

        // Navigation property for the one-to-many relationship with Subject
        public Subject Subject { get; set; }
    }
}

