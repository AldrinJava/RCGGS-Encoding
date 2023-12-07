using Microsoft.AspNetCore.SignalR;


public class EncodingHub : Hub
{

    public async Task ReceiveCharacter(string character, string requestId)
    {
        // Logic for handling the received character
        // In this case, we're just echoing back the character to all connected clients.
        await Clients.All.SendAsync("ReceiveCharacter", character, requestId);
    }

}
