using System.Runtime.CompilerServices;

namespace Test1_WebApp_MVC.Models
{
    public class UserViewModel<TUserData>
    {
        public UserViewModel() { }

        public UserViewModel(string sender)
        {
            Sender = sender;
        }

        public TUserData UserData { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public string Sender { get; set; }
    }
}
