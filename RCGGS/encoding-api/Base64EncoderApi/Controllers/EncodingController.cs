using Base64EncoderApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text;


namespace Base64EncoderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EncodingHubs : ControllerBase
    {
        private static ConcurrentDictionary<string, string> _encodedStrings = new ConcurrentDictionary<string, string>();
        private readonly IHubContext<EncodingHub> _hubContext;

        public EncodingHubs(IHubContext<EncodingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("start")]
        public IActionResult StartEncoding([FromBody] EncodingRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Input))
            {
                return BadRequest("Invalid input");
            }

            var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Input));
            var requestId = Guid.NewGuid().ToString();
            _encodedStrings.TryAdd(requestId, encodedString);

            Task.Run(async () =>
            {
                while (_encodedStrings.TryGetValue(requestId, out var remainingString) && !string.IsNullOrEmpty(remainingString))
                {
                    var character = remainingString[0];
                    _encodedStrings[requestId] = remainingString.Substring(1);
                    await _hubContext.Clients.All.SendAsync("ReceiveCharacter", character, requestId);
                    await Task.Delay(new Random().Next(1000, 5000)); // Simulate processing delay
                }
            });

            return Ok(requestId);
        }

        [HttpPost("cancel/{requestId}")]
        public IActionResult CancelEncoding(string requestId)
        {
            _encodedStrings.TryRemove(requestId, out _);
            return Ok();
        }
    }
}