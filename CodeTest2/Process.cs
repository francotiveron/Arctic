using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CodeTest2
{
    public class Process
    {
        public readonly HashSet<Resource> Resources;
        public bool Complete { get; private set; }
        public Process(HashSet<Resource> resources)
        {
            Resources = resources;
        }

        internal void Start(Channel<Process> channel)
        {
            foreach (var resource in Resources) resource.Acquire();
            Task.Delay(100);
            Complete = true;
            foreach (var resource in Resources) resource.Release();
            channel.Writer.TryWrite(this);
        }
    }
}
