using System;
namespace StudentAPI.Models
{
	public class TopicStatusRequest
	{
        public int TopicId { get; set; }
        public bool IsComplete { get; set; }
    }
}

