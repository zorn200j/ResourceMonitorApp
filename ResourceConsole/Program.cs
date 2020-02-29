using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ResourceConsole
{
    class Program
    {
        static bool _running = true;
        static PerformanceCounter _cpuCounter, _memUsageCounter, _upTimeCounter, _processCounter, _diskCounter, _interruptsCounter, _mutexCounter;

        static void Main(string[] args)
        {
            Thread pollingThread = null;

          
            Console.WriteLine("Computer Resource Monitor Console");

            try
            {
                _cpuCounter = new PerformanceCounter();
                _cpuCounter.CategoryName = "Processor";
                _cpuCounter.CounterName = "% Processor Time";
                _cpuCounter.InstanceName = "_Total";

                _memUsageCounter = new PerformanceCounter("Memory", "Available KBytes");

                _upTimeCounter = new PerformanceCounter("System", "System Up Time");

                _processCounter = new PerformanceCounter("System", "Processes");

                _diskCounter = new PerformanceCounter();
                _diskCounter.CategoryName = "LogicalDisk";
                _diskCounter.CounterName = "Disk Bytes/sec";
                _diskCounter.InstanceName = "C:";

                _interruptsCounter = new PerformanceCounter();
                _interruptsCounter.CategoryName = "Processor";
                _interruptsCounter.CounterName = "Interrupts/sec";
                _interruptsCounter.InstanceName = "_Total";

                _mutexCounter = new PerformanceCounter();
                _mutexCounter.CategoryName = "Objects";
                _mutexCounter.CounterName = "Mutexes";
                

                pollingThread = new Thread(new ParameterizedThreadStart(RunPollingThread));
                pollingThread.Start();

                Console.WriteLine("Press a key to stop and exit");
                Console.ReadKey();

                Console.WriteLine("Stopping thread..");

                _running = false;

                pollingThread.Join(5000);

            }
            catch (Exception)
            {
                pollingThread.Abort();

                throw;
            }
        }

        static void RunPollingThread(object data)
        {
            
            DateTime lastPollTime = DateTime.MinValue;

            Console.WriteLine("Started polling...");

         
            while (_running)
            {
                
                if ((DateTime.Now - lastPollTime).TotalMilliseconds >= 1000)
                {
                    double cpuTime;
                    ulong memUsage, upTime, processes, disk, interrupts, mutexes, totalMemory;

                    
                    GetMetrics(out cpuTime, out memUsage, out upTime, out processes, out disk, out interrupts, out mutexes, out totalMemory);

                 
                    var postData = new
                    {
                        MachineName = System.Environment.MachineName,
                        Processor = cpuTime,
                        MemUsage = memUsage,
                        UpTime = upTime,
                        Processes = processes,
                        Disk = disk,
                        Interrupts = interrupts,
                        Mutexes = mutexes,
                        TotalMemory = totalMemory
                     
                    };

                    var json = JsonConvert.SerializeObject(postData);

                
                    var serverUrl = new Uri(ConfigurationManager.AppSettings["ServerUrl"]);

                    var client = new WebClient();
                    client.Headers.Add("Content-Type", "application/json");
                    client.UploadString(serverUrl, json);

                 
                    lastPollTime = DateTime.Now;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        static void GetMetrics(out double processorTime, out ulong memUsage, out ulong upTime, out ulong processes,
            out ulong disk, out ulong interrupts, out ulong mutexes, out ulong totalMemory)
        {
            processorTime = (double)_cpuCounter.NextValue();
            memUsage = (ulong)_memUsageCounter.NextValue();
            upTime = (ulong)_upTimeCounter.NextValue();
            processes = (ulong)_processCounter.NextValue();
            disk = (ulong)_diskCounter.NextValue();
            interrupts = (ulong)_interruptsCounter.NextValue();
            mutexes = (ulong)_mutexCounter.NextValue();
            totalMemory = 0;
            

           
            ObjectQuery memQuery = new ObjectQuery("SELECT * FROM CIM_OperatingSystem");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(memQuery);

            foreach (ManagementObject item in searcher.Get())
            {
                totalMemory = (ulong)item["TotalVisibleMemorySize"];
            }
        }
    }
}