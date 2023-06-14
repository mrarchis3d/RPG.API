namespace RPG.Identity.Domain.UserAggregate
{
    public class BaseUser
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
    }

    public class BaseUserRelational : BaseUser
    {
        public Guid UserId { get; set; }
    }

}
