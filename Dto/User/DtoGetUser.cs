namespace WAPI_GS.Dto.User
{
    public class DtoGetUser
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? Name { get; set; }
        public string? MobilePhone { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Color { get; set; }
        //public string? Password { get; set; }
    }
}
