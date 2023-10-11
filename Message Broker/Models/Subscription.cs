using System;
using System.ComponentModel.DataAnnotations;
namespace Message_Broker.Models
{
	public class Subscription
    {
		[Key]
		public int ID { get; set; }
		[Required]
        public string? Name { get; set; }
		[Required]
		public int TopicId { get; set; }
    }
}

