using Test1_WebApp_MVC.Models;

namespace Test1_WebApp_MVC.DAL
{
    public interface IUserDataContext
    {
        public bool CreateUser(User newUser);
        public bool UpdateUser(User existingUser);
        public bool DeleteUser(int userIdToDelete);
        public User GetUser(int userId);
        public List<User> GetUsers();
    }
}
