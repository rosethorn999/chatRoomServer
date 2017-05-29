using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.Web.WebSockets;
using Newtonsoft.Json.Linq;

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
    this.name = this.WebSocketContext.QueryString["name"];
    this.room = this.WebSocketContext.QueryString["room"];

    clients.Add(this);
    clients.Broadcast(name + " has connected.");
  }

  public override void OnMessage(string message)
  {
    clients.Broadcast("back onmessage");

    setMessage(name, message);
    //特定群組
    /*var channel = clients.FirstOrDefault(n => ((TestWebSocketHandler)n).room == this.room);
    if (channel != null)
    {
        JArray reData = new JArray();
        reData.Add(new JArray() { "" });
        channel.Send((string)reData);
    }*/

    clients.Broadcast((string)this.all_message);
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
    JArray tempArray = new JArray();
    JArray talk = new JArray() { name, message };
    tempArray.AddAfterSelf(talk);

    all_message[this.room].AddAfterSelf(tempArray);
  }

}

