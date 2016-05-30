using System.Threading.Tasks;

namespace Tests.HyperMock.Universal.Support
{
    public interface IUserService
    {
        string Help { get; }
        string CurrentRole { get; set; }
        bool Save(string name);
        Task<bool> SaveAsync(string name);
        bool Save(string name, string role);
        void Delete(string name);
        Task DeleteAsync(string name);
    }
}