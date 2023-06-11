namespace RPGAPI.Domain.UserAggregate
{
    public class User: BaseUserRelational
    {
        public required string Email { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] Salt { get; set; }
    }
}
