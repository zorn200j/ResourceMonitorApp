namespace ResourceViewer.Models
{
    public class PcInfoPostData
    {
        public string MachineName { get; set; }

        public double Processor { get; set; }

        public ulong MemUsage { get; set; }

        public ulong UpTime { get; set; }

        public ulong Processes { get; set; }

        public ulong Disk { get; set; }

        public ulong Interrupts { get; set; }

        public ulong Mutexes { get; set; }

        public ulong TotalMemory { get; set; }

        
    }
}