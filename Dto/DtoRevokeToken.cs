namespace OS101TokenJwt.Dto
{
    public class DtoRevokeToken
    {
        public string? Token { get; set; } = null;
        public bool IsRevoked { get; set; } = true;
    }
}
