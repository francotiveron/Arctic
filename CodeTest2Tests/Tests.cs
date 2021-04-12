using CodeTest2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace CodeTest2Tests
{
    public class Tests
    {
        [Fact]
        public async Task specification()
        {
            var resources = new[] { new Resource(), new Resource(), new Resource() };
            var processes = new[]
            {
                new Process(new[] { resources[0] }.ToHashSet())
                , new Process(new[] { resources[1] }.ToHashSet())
                , new Process(new[] { resources[0], resources[1] }.ToHashSet())
                , new Process(new[] { resources[2] }.ToHashSet())
                , new Process(new[] { resources[1] }.ToHashSet())
            };
            Workflow workflow = new(resources.ToHashSet(), processes.ToList());
            await workflow.Run();
            Assert.All(processes, process => Assert.True(process.Complete));
            Assert.Equal(2, resources[0].LockNbr);
            Assert.Equal(3, resources[1].LockNbr);
            Assert.Equal(1, resources[2].LockNbr);
            Assert.True(resources[0].Visitors.SetEquals(new[] { processes[0], processes[2] }.ToHashSet()));
            Assert.True(resources[1].Visitors.SetEquals(new[] { processes[1], processes[2], processes[4] }.ToHashSet()));
            Assert.True(resources[2].Visitors.SetEquals(new[] {processes[3] }.ToHashSet()));
        }

        [Fact]
        public async Task multiple_random()
        {
            var resources = Enumerable.Range(0, 10000).Select(_ => new Resource()).ToArray();
            Random rnd = new Random((int)DateTime.Now.Ticks);
            Process RandomProcess()
            {
                int nResources = rnd.Next(1, 1000);
                HashSet<int> indexes = new();
                while (indexes.Count < nResources) indexes.Add(rnd.Next() % 1000);
                var pResources = indexes.Select(i => resources[i]).ToHashSet();
                return new Process(pResources);
            }
            var processes = Enumerable.Range(0, 10000).Select(_ => RandomProcess()).ToArray();
            Workflow workflow = new(resources.ToHashSet(), processes.ToList());
            await workflow.Run();
            Assert.All(processes, process => Assert.True(process.Complete));
            Assert.Equal(processes.Sum(process => process.Resources.Count), resources.Sum(resource => resource.LockNbr));
        }
    }
}
