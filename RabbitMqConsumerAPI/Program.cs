using MassTransit;
using RabbitMqConsumerAPI.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TicketConsumer>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        var messageQueues = builder.Configuration.GetSection("RabbitMQ:MessageQueue:TicketQueue").Value;
        var username = builder.Configuration.GetSection("RabbitMQ:Username").Value;
        var password = builder.Configuration.GetSection("RabbitMQ:Password").Value;
        var url = builder.Configuration.GetSection("RabbitMQ:Url").Value;

        config.Host(new Uri(url), h =>
        {
            h.Username(username);
            h.Password(password);
        });
        config.ReceiveEndpoint(messageQueues, ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<TicketConsumer>(provider);
        });
    }));
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
