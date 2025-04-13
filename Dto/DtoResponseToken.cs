namespace WAPI_GS.Dto
{
    public class DtoResponseToken
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
