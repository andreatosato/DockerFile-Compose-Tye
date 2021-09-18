using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DaprMqttCS.Controllers
{
    public class Driver
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [ApiController]
    [Route("[Controller]")]
    public class DriverDetailsController : ControllerBase
    {
        private readonly DaprClient client;

        public DriverDetailsController(DaprClient client)
        {
            this.client = client;
        }

        [Topic("mqtttest", "driverdetails")]
        [HttpPost("driverdetails")]
        public async Task<IActionResult> ProcessMessage(Driver driver)
        {
            Console.WriteLine($"----------> {driver.FirstName} {driver.LastName}");
            return Ok();
        }

        [HttpPost]
        [Route("Sender")]
        public async Task<IActionResult> Sender([FromBody] Driver driver)
        {
            await client.PublishEventAsync("mqtttest", "driverdetails", driver);
            return Created("driverdetails", driver);
        }

    }
}
