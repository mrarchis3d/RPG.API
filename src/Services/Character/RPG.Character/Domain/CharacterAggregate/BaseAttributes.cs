using RPG.BuildingBlocks.Common.Repository;

namespace RPG.Character.Domain.CharacterAggregate
{
    public class BaseAttributes : BaseEntity
    {
        public Guid? ClassTypeId { get; set; }
        public Guid? RaceTypeId { get; set; }
        public int PhisycalDamageLow { get; set; }
        public int PhisycalDamageHigh { get; set; }
        public int CriticRatePercentage { get; set; }
        public int BaseHealth { get; set; }
        public int MagicalDamageLow { get; set; }
        public int MagicalDamageHigh { get; set; }
        public int Concentration { get; set; }
        public int DodgePercentage { get; set; }
        public int Armor { get; set; }
        public int MagicalArmor { get; set; }
        public RaceType? RaceType { get; set; } = null;
        public ClassType? ClassType { get; set; } = null;

    }
}
