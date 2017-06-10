using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.Web.WebSockets;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class TestWebSocketHandler : WebSocketHandler
{
    private static WebSocketCollection clients = new WebSocketCollection();

    public TestWebSocketHandler()
    {
    }

    public override void OnOpen()
    {
        clients.Add(this);
        clients.Broadcast("Connected");
    }

    public override void OnMessage(string message)
    {
        clients.Broadcast("back onmessage");
        try
        {
            clients.Broadcast(message);
        }
        catch (Exception ex)
        {
            clients.Broadcast(ex.ToString());
        }

        //特定群組
        /*var channel = clients.FirstOrDefault(n => ((TestWebSocketHandler)n).room == this.room);
        if (channel != null)
        {
            JArray reData = new JArray();
            reData.Add(new JArray() { "" });
            channel.Send((string)reData);
        }*/
    }

    public override void OnClose()
    {
        clients.Remove(this);
        //clients.Broadcast(string.Format("{0} has gone away.", name));
    }

    public override void OnError()
    {
        clients.Broadcast(string.Format("Error：{0}", base.Error.ToString()));
    }

}
