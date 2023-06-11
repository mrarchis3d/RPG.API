namespace RPGAPI.Dtos.Requests
{
    public class UserCredentialsDto
    {
        public string UserName { get; set; }
        public required string Password { get; set; }
    }
}
