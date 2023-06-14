namespace RPG.Identity.Dtos.Requests
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
