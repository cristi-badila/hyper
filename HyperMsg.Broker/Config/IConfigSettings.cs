namespace HyperMsg.Broker.Config
{
    public interface IConfigSettings
    {
        string DatabasePath { get; }
        string Address { get; }
    }
}
