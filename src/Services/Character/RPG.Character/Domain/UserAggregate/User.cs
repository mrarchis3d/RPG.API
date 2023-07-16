using RPG.BuildingBlocks.Common.BaseContext;
using CharacterEntity = RPG.Character.Domain.CharacterAggregate.Character;

namespace RPG.Character.Domain.UserAggregate
{
    public class User : BaseUserRelational
    {
        public CharacterEntity Character { get; set; } = null;
    }
}
