namespace HyperMock.Universal
{
    public interface IMock
    {
        object Object { get; }

        MockProxyDispatcher Dispatcher { get; }
    }
}