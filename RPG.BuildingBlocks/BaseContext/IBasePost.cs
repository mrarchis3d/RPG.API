using System;
using RPG.BuildingBlocks.Common.Enums;

namespace RPG.BuildingBlocks.Common.BaseContext;

public interface IBasePost
{
    public TargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public Privacy Privacy { get; set; }
    public Guid UserId { get; set; }
}