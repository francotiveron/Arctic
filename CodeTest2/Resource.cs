using System;
using System.Collections.Generic;

namespace CodeTest2
{
    public class Resource
    {
        bool _locked, _inUse;

        internal bool IsAvailable => !_locked;
        public HashSet<Process> Visitors { get; } = new HashSet<Process>();

        public int LockNbr { get; private set; }

        internal void Lock(Process owner)
        {
            if (_locked) throw new InvalidOperationException("Lock Race condition");
            ++LockNbr;
            Visitors.Add(owner);
            _locked = true;
        }

        internal void Acquire()
        {
            if (_inUse) throw new InvalidOperationException("Acquire Race condition");
            _inUse = true;
        }

        internal void Release()
        {
            if (!_inUse) throw new InvalidOperationException("Release Race condition");
            _inUse = false;
        }

        internal void Unlock()
        {
            if (!_locked) throw new InvalidOperationException("Unlock Race condition");
            _locked = false;
        }
    }
}
