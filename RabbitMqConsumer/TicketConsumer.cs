using MassTransit;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Shared.Models;
using Newtonsoft.Json;

namespace RabbitMqConsumer
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
