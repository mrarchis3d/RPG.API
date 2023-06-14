namespace RPG.Identity.Domain.SpaceAggregate
{
    public class Template: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string NewUserDefaultRoleName { get; set; }
        public string CreatorRoleName { get; set; }
    }
}
