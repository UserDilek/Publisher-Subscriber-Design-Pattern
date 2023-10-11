using System;
using System.ComponentModel.DataAnnotations;
namespace Message_Broker.Models
{
    public class Message
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? TopicMessage { get; set; }
       
        public int SubscriptionId { get; set; }

        [Required]
        public DateTime ExpiresAfter { get; set; }
        [Required]
        public string MessageStatus { get; set; } = "NEW";

    }

}