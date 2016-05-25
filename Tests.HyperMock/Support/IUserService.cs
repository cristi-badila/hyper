namespace Tests.HyperMock.Universal.Support
{
    public interface IUserService
    {
        string Help { get; }
        string CurrentRole { get; set; }
        bool Save(string name);
        bool Save(string name, string role);
        void Delete(string name);
    }
}