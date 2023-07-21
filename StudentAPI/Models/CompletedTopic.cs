using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAPI.Models
{
	public class CompletedTopic
	{
        [Column("id")]
        public int Id { get; set; }
        [Column("studentid")]
        public int StudentId { get; set; }
        [Column("topicid")]
        public int TopicId { get; set; }
        [Column("iscomplete")]
        public bool IsComplete { get; set; }

        public Student Student { get; set; }
    }
}

