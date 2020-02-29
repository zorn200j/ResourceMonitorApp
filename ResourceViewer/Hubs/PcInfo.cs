
using Microsoft.AspNet.SignalR;

namespace ResourceViewer.Hubs
{
    public class PcInfo : Hub
    {
        public void SendPcInfo(string machineName, double processor, int memUsage, int upTime, int processes, int totalMemory)
        {
            this.Clients.All.pcInfoMessage(machineName, processor, memUsage, upTime, processes, totalMemory);
        }
    }
}