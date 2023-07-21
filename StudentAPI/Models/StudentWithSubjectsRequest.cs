using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAPI.Models
{
    public class StudentWithSubjectsRequest
	{
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateOnly Dob { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public List<int> SubjectIds { get; set; }
    }
}

