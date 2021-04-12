using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CodeTest2
{
    public class Workflow
    {
        readonly HashSet<Resource> _resources;
        readonly List<Process> _processes;
        Channel<Process> _channel;

        public Workflow(HashSet<Resource> resources, List<Process> processes)
        {
            _resources = resources;
            _processes = processes;
        }

        public async Task Run()
        {
            _channel = Channel.CreateUnbounded<Process>();
            while (_processes.Count > 0)
            {
                foreach (var process in _processes)
                {
                    if (process.Resources.All(resource => resource.IsAvailable))
                    {
                        foreach (var resource in process.Resources) resource.Lock(process);
                        process.Start(_channel);
                    }
                }
                await _channel.Reader.WaitToReadAsync();
                while (_channel.Reader.TryRead(out var process))
                {
                    foreach (var resource in process.Resources) resource.Unlock();
                    _processes.Remove(process);
                }
            }
        }
    }
}
