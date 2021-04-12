using CodeTest1;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Xunit;

namespace CodeTest1Tests
{
    public class Tests
    {
        [Fact]
        public void single_node()
        {
            Node root = new(1);
            Assert.Equal(1, root.Sum());
        }

        [Fact]
        public void simple_tree()
        {
            Node root = new(1);
            root.AddChildren(new Node[] { new(2), new(3) });
            Assert.Equal(6, root.Sum());
        }

        [Fact]
        public void specification()
        {
            Node root = 
                new(1, new Node[] { 
                                    new(2, new Node[] { new(4) }), 
                                    new(3, new Node[] { 
                                                        new(5),
                                                        new(6, new Node[] { new(7), new(8), new(9) }) 
                                                      }) 
                                  });
            
            
            
            Assert.Equal(45, root.Sum());
        }

        [Fact]
        public void random_tree()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            Queue<Node> q = new();
            int sum = 0;
            Node RandomNode() 
            {
                var value = rnd.Next(1, 100);
                sum += value;
                return new(value);
            }
            Node root = RandomNode();
            q.Enqueue(root);
            int nNodes = 1, totalNodes = 10000000;

            while(nNodes < totalNodes)
            {
                var curNode = q.Dequeue();
                var nChildren = rnd.Next(1, 100);
                for (int i = 0; i < nChildren && nNodes < totalNodes; i++, nNodes++)
                {
                    var node = RandomNode();
                    q.Enqueue(node);
                    curNode.AddChild(node);
                }
            };

            Assert.Equal(sum, root.Sum());
        }


    }
}
