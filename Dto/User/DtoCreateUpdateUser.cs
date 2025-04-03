namespace WAPI_GS.Dto.User
{
    public class DtoCreateUpdateUser
    {
        public string Name { get; set; } = null!;
        public string MobilePhone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
