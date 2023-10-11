using System;
using System.ComponentModel.DataAnnotations;

namespace Subcriber.Dtos
{
	public class MessageReadDto
	{
		            
        public int ID { get; set; }
           
        public string? TopicMessage { get; set; }

        public DateTime ExpiresAfter { get; set; }
        
        public string? MessageStatus { get; set; } 
   
	}
}
