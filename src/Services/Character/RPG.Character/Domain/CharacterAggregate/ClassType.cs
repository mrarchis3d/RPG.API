using RPG.BuildingBlocks.Common.Repository;

namespace RPG.Character.Domain.CharacterAggregate
{
    public class ClassType : BaseEntity
    {
        public string Name { get; set; }
        public Character Character { get; set; } = null;

    }
}
