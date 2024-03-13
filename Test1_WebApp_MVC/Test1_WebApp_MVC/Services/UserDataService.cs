using Test1_WebApp_MVC.DAL;
using Test1_WebApp_MVC.Models;

namespace Test1_WebApp_MVC.Models.Interfaces
{
    public class UserDataService
    {
        private IUserDataContext _userDataContext;

        public UserDataService(IUserDataContext userDataContext)
        {
            _userDataContext = userDataContext;
        }

        public bool CreateUser(User newUser)
        {
            return _userDataContext.CreateUser(newUser);
        }

        public bool UpdateUser(User existingUser)
        {
            return _userDataContext.UpdateUser(existingUser);
        }

        public bool DeleteUser(int userIdToDelete)
        {
            return _userDataContext.DeleteUser(userIdToDelete);
        }

        public User GetUser(int userId)
        {
            return _userDataContext.GetUser(userId);
        }

        public List<User> GetUsers()
        {
            return _userDataContext.GetUsers();
        }
    }
}
