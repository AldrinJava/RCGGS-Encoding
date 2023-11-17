using Base64EncoderApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text;

[ApiController]
[Route("[controller]")]
public class EncodingController : ControllerBase
{
    private static ConcurrentDictionary<string, string> _encodedStrings = new ConcurrentDictionary<string, string>();

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
        return Ok(requestId); // Return a unique ID for this encoding process
    }


    [HttpGet("get/{requestId}")]
    public async Task<IActionResult> GetEncodedCharacter(string requestId, CancellationToken cancellationToken)
    {
        if (_encodedStrings.TryGetValue(requestId, out var encodedString))
        {
            if (!string.IsNullOrEmpty(encodedString))
            {
                var character = encodedString[0];
                _encodedStrings[requestId] = encodedString.Substring(1);
                await Task.Delay(new Random().Next(1000, 5000), cancellationToken);
                return Ok(character.ToString());
            }
            return Ok(); // No more characters to send
        }
        return NotFound();
    }

    [HttpPost("cancel/{requestId}")]
    public IActionResult CancelEncoding(string requestId)
    {
        _encodedStrings.TryRemove(requestId, out _);
        return Ok();
    }
}
