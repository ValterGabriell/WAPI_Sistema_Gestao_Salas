namespace WAPI_GS.Interfaces
{
    public interface ICS_Auth
    {
        public Task<string> Login(string username, string password, bool isAdmin);
    }
}
