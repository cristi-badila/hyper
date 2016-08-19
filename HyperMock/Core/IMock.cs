namespace HyperMock.Universal.Core
{
    public interface IMock
    {
        object Object { get; }

        MockProxyDispatcher Dispatcher { get; }
    }
}