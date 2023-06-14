using Ourglass.Reactions.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.BuildingBlocks.Common.EventBus.Events.Comments
{
    public class CommentReactionEvent
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public ReactionType ReactionType { get; set; }        
        public Guid UserId { get; set; }
        public Guid ReactedUserId { get; set; }
    }
}
