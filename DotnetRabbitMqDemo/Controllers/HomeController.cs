using DotnetRabbitMqDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MassTransit;
using Shared.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DotnetRabbitMqDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IBus _bus;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IBus bus)
        {
            _logger = logger;
            _configuration = configuration;
            _bus = bus;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Ticket ticket)
        {
            if (ticket != null)
            {
                ticket.BookedOn = DateTime.UtcNow;
                Uri uri = new Uri(_configuration["RabbitMQ:MessageQueue:TicketQueue"]);
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(ticket);
                _logger.LogInformation($"Message sent {JsonConvert.SerializeObject(ticket)}");
                TempData["Message"] = "Booking Success";
                return RedirectToAction("index", "home");
            }
            return BadRequest();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}