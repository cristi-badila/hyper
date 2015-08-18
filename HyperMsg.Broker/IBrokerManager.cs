using System;

namespace HyperMsg.Broker
{
    public interface IBrokerManager : IDisposable
    {
        IBrokerManager Init();
        void Run();
    }
}