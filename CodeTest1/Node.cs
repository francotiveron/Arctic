using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeTest1
{
    public class Node
    {
        private int _value;
        private HashSet<Node> _children;

        public Node(int value, Node[] children = null) { _value = value; _children = children?.ToHashSet() ?? new HashSet<Node>(); }

        int Value => _value;
        public void AddChild(Node node) => _children.Add(node);
        public void AddChildren(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes) AddChild(node);
        }

        public int Sum()
        {
            return Value + _children.Sum(child => child.Sum());
        }
    }
}
