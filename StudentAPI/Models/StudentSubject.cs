
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAPI.Models
{
    [Table("student_subjects")]
    public class StudentSubject
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("studentid")]
        public int StudentId { get; set; }
        [Column("subjectid")]
        public int SubjectId { get; set; }
    }
}

