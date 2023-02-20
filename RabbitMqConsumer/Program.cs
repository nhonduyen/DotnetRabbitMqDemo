using RabbitMqConsumer;
using MassTransit;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.AddHostedService<Worker>();
        services.AddMassTransit(x =>
        {
            x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
            {
                var messageQueues = configuration.GetSection("RabbitMQ:MessageQueue:TicketQueue").Value;
                var username = configuration.GetSection("RabbitMQ:Username").Value;
                var password = configuration.GetSection("RabbitMQ:Password").Value;
                var url = configuration.GetSection("RabbitMQ:Url").Value;

                config.Host(new Uri(url), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

            }));
        });
    })
    .Build();



await host.RunAsync();
