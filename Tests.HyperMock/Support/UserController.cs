using System.Threading.Tasks;

namespace Tests.HyperMock.Universal.Support
{
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public bool Save(string name)
        {
            return _userService.Save(name);
        }

        public async Task<bool> SaveAsync(string name)
        {
            return await _userService.SaveAsync(name);
        }

        public bool SaveWithRole(string name, string role)
        {
            return _userService.Save(name, role);
        }

        public void Delete(string name)
        {
            _userService.Delete(name);
        }

        public string GetHelp()
        {
            return _userService.Help;
        }

        public void SetCurrentRole(string role)
        {
            _userService.CurrentRole = role;
        }

        public async Task DeleteAsync(string name)
        {
            await _userService.DeleteAsync(name);
        }
    }
}