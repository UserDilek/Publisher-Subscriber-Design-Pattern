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

       

        [HttpPost("/{id}/messages")]
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
                    SubscriptionId = sub.ID,
                };

                await _context.Messages.AddAsync(msg);

            }

            await _context.SaveChangesAsync();

            return Ok("Message has been addded");
        }

        [HttpPost("/{id}/subscription")]
        public async Task<IActionResult> PostTopicSubs(int id, [FromBody] Subscription subscription)
        {

            bool topics = await _context.Topics.AnyAsync(t => t.Id == id);

            if (!topics)
                return NotFound("Topic is not found");


            subscription.TopicId = id;
            await _context.Subscriptions.AddAsync(subscription);


            await _context.SaveChangesAsync();

            return Ok("Subscription is created");
        }

        [HttpPost("/{id}/subsMessage")]
        public async Task<IActionResult> PostSubsMessage(int id)
        {

            bool subs = await _context.Subscriptions.AnyAsync(s => s.ID == id);
            if (!subs)
                return NotFound("Subscriptions not found");

            var messages = await _context.Messages.Where(m => m.SubscriptionId == id).ToArrayAsync();

            if (messages.Count() == 0)

                return NotFound("No new message");


            foreach (var msg in messages)
            {
                msg.MessageStatus = "RESQUESTED";
                
            }
            await _context.SaveChangesAsync();
            return Ok(messages);
        }


        [HttpPost("subscription/{id}/subsMessage")]
        public async Task<IActionResult> PostSubsMessages(int id, int[] confs)
        {

            bool subs = await _context.Subscriptions.AnyAsync(s => s.ID == id);
            if (!subs)
                return NotFound("Subscriptions not found");


            if (confs.Length < 0)
                return BadRequest();

            int count = 0;
            foreach (int i in confs)
            {
                var msg = _context.Messages.FirstOrDefault(m => m.ID == i);

                if (msg != null) {
                    msg.MessageStatus = "SENT";
                    await _context.SaveChangesAsync();
                    count++;

                }
            }


            return Ok($"Acknowledged {count}/{confs.Length} messagee");
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

