using System;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Foundation.Web.Maple.Server.Routing;

namespace ConnectedLed.Meadow.MapleRequestHandlers
{
    public class LedControllerRequestHandler : RequestHandlerBase
    {
        [HttpGet]
        public void SignText()
        {
            Console.WriteLine("GET::SignText");

            string text = base.QueryString["text"] as string;

            this.Context.Response.ContentType = ContentTypes.Application_Text;
            this.Context.Response.StatusCode = 200;
            _ = this.Send($"{text} received");
        }
    }
}