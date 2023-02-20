using MassTransit;
using Microsoft.Extensions.Configuration;

namespace RabbitMqConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                _logger.LogInformation("CreateUsingRabbitMq");
                cfg.ReceiveEndpoint(_configuration.GetSection("RabbitMQ:MessageQueue:TicketQueue").Value, e =>
                {
                    e.Consumer<TicketConsumer>();
                });
            });
            await busControl.StartAsync(new CancellationToken());

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}