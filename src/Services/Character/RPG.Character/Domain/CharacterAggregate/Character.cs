using RPG.BuildingBlocks.Common.Repository;
using RPG.Character.Domain.UserAggregate;

namespace RPG.Character.Domain.CharacterAggregate
{
    public class Character: BaseEntity
    {
        public Guid UserId { get; set; }
        public string NickName { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public long Experience { get; set; }
        public int ProfessionLevel { get; set; }
        public int ProfessionExperience { get; set; }
        public Guid ClassTypeId { get; set; }
        public Guid RaceTypeId { get; set; }
        public User User { get; set; }
        public ClassType ClassType { get; set; }
        public RaceType RaceType { get; set; }
    }
}
