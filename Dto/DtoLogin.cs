namespace WAPI_GS.Dto
{
    public class DtoLogin
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsAdmin { get; set; }
    }
}
