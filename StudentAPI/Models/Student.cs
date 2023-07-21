using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAPI.Models
{
    [Table("students")]
    public class Student
    {
        [Column("studentid")]
        public int StudentId { get; set; }
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("secondname")]
        public string SecondName { get; set; }
        [Column("dob")]
        public DateOnly Dob { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("email")]
        public string Email { get; set; }
        public List<CompletedTopic> CompletedTopics { get; set; }
    }
}

