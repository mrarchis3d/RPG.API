namespace RPGAPI.Dtos.Responses
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public required string Email { get; set; }
    }
}
