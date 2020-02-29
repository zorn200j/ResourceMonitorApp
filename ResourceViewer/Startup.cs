using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ResourceViewer.Startup))]

namespace ResourceViewer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}