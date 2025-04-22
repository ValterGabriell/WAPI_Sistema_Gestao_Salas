namespace WAPI_GS.Dto
{
    public class DtoResponseToken
    {
        public string Token { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
        public DateTime Expiration { get; set; }
    }
}
