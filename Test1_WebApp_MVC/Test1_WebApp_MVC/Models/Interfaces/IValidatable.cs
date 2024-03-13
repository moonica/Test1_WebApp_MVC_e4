namespace Test1_WebApp_MVC.Models.Interfaces
{
    public interface IValidatable
    {
        public List<string> ValidationErrors { get; set; }

        public bool IsValid();        
    }
}
