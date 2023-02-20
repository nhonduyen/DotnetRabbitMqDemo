using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
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
    }));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
