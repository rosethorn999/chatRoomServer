using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ws 的摘要描述
/// </summary>
public class ws : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        if (context.IsWebSocketRequest)
        {
            context.AcceptWebSocketRequest(new TestWebSocketHandler());
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
