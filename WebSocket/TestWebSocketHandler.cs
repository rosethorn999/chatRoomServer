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
    private string name;
    private string room;
    private JObject all_message = new JObject() { };

    public TestWebSocketHandler()
    {
    }

    public override void OnOpen()
    {
        this.name = this.WebSocketContext.QueryString["name"];//使用者名稱
        this.room = this.WebSocketContext.QueryString["room"];//房間名稱

        clients.Add(this);
        clients.Broadcast(name + " has connected.");
    }

    public override void OnMessage(string message)
    {
        clients.Broadcast("back onmessage");
        try
        {
            setMessage(name, message);
            string reJStr = JsonConvert.SerializeObject(all_message);
            clients.Broadcast(reJStr);
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
        clients.Broadcast(string.Format("{0} has gone away.", name));
    }

    public override void OnError()
    {
        clients.Broadcast(string.Format("Error：{0}", base.Error.ToString()));
    }

    public void setMessage(string name, string message)
    {
        JObject talk = new JObject() { new JProperty("name", name), new JProperty("message", message) };

        if (all_message[this.room] == null)
        {
            JArray tempArray = new JArray();
            tempArray.Add(talk);
            JObject tempObject = new JObject() { new JProperty("talk", tempArray) };
            all_message.Add(new JProperty(this.room, tempObject));
        }
        else
        {
            JArray temp = (JArray)all_message[this.room]["talk"];
            //傳址
            temp.Add(talk);
        }
    }

}
