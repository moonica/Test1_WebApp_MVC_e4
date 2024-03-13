namespace Test1_WebApp_MVC.Models
{
    public class UserViewModel<TUserData>
    {
        public TUserData userData { get; set; }
        public string controllerName { get; set; }
        public string actionName { get; set; }
    }
}
