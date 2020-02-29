namespace ResourceViewer.Models
{
    public class PcInfoPostData
    {
        public string MachineName { get; set; }

        public double Processor { get; set; }

        public ulong MemUsage { get; set; }

        public ulong UpTime { get; set; }

        public ulong TotalMemory { get; set; }

        
    }
}