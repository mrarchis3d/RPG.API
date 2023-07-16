using RPG.BuildingBlocks.Common.Repository;

namespace RPG.Character.Domain.CharacterAggregate
{
    public class RaceType : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Character Character { get; set; } = null;

    }
}
