using MassTransit;
using Newtonsoft.Json;
using Shared.Models;

namespace RabbitMqConsumerAPI.Models
{
    public class TicketConsumer : IConsumer<Ticket>
    {
        public Task Consume(ConsumeContext<Ticket> context)
        {
            var data = JsonConvert.SerializeObject(context.Message);
            Console.WriteLine(data); 
            return Task.CompletedTask;
        }
    }
}
