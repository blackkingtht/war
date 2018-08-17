using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Tcp
{
    internal interface ISTcpConnectionUpdate
    {
        void Initialize();
        void Update(long tick);
        void Stop();
    }
}
