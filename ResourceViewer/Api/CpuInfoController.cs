
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using ResourceViewer.Hubs;
using ResourceViewer.Models;

namespace ResourceViewer.Api
{
    public class PcInfoController : ApiController
    {
        public void Post(PcInfoPostData pcInfo)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<PcInfo>();
            context.Clients.All.pcInfoMessage(pcInfo.MachineName, pcInfo.Processor, pcInfo.MemUsage, pcInfo.UpTime, pcInfo.Processes, pcInfo.TotalMemory);
        }
    }
}