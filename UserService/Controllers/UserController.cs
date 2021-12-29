using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserServiceContext _context;

        public UserController(UserServiceContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() => await _context.Users.ToListAsync();

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var integrationEventData = JsonConvert.SerializeObject(new {
                id = user.ID,
                newname = user.Name
            });

            PublishToMessageQueue("user.update", integrationEventData);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var integrationEventData = JsonConvert.SerializeObject(new {
                id = user.ID,
                name = user.Name
            });

            PublishToMessageQueue("user.add", integrationEventData);

            return CreatedAtAction("GetUser", new { id = user.ID }, user);
        }

        private void PublishToMessageQueue(string integrationEvent, string eventData)
        {
            // TODO: Reuse and close connections and channel, etc
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(eventData);
            channel.BasicPublish(exchange: "user", routingKey: integrationEvent, basicProperties: null, body: body);
        }
    }
}