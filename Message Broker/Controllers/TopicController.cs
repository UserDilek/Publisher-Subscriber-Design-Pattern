using System;
using Message_Broker.Data;
using Message_Broker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Message_Broker.Controllers
{
  
    [ApiController]
    [Route("[controller]")]
    public class TopicController: ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WeatherForecastController> _logger;

        public TopicController(ILogger<WeatherForecastController> logger, AppDbContext context)
		{
            _logger = logger;
            _context = context;
		}

        [HttpGet(Name = "GetTopics")]
        public async  Task<IEnumerable<Topic>> Get()
        {

            var topics = await _context.Topics.ToListAsync();

            return topics;
        }

        [HttpPost("Topic/{id}/messages")]
        public async Task<IActionResult> GetTopics(int id, [FromBody] Message message)
        {

            bool topics = await _context.Topics.AnyAsync(t => t.Id == id);

            if (!topics)
                return NotFound("Topic is not found");

            var subs = await _context.Subscriptions.Where(s => s.TopicId == id).ToArrayAsync();

            if(subs.Count() == 0)
                return NotFound("There are no sunscriptions for this topic");


            foreach (var sub in subs)
            {
                Message msg = new Message
                {
                    TopicMessage = message.TopicMessage,
                    ExpiresAfter = message.ExpiresAfter,
                    SubscriptionId = sub.ID,
                    MessageStatus = message.MessageStatus
                };

                await _context.Messages.AddAsync(msg);

            }

            await _context.SaveChangesAsync();

            return Ok("Message has been addded");
        }




        // POST: api/Topic
        [HttpPost(Name = "PostTopics")]
        [Produces("application/json")]
        public async Task<Topic> Post([FromBody] Topic topic)
        {
        
            try
            {
                await _context.Topics.AddAsync(topic);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }


            return topic;
        }
    }
}

