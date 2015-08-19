using System;

namespace HyperMsg.Broker
{
    public interface IBrokerManager : IDisposable
    {
        void Run();
    }
}