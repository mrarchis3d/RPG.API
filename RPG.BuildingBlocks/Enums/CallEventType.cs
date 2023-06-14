using System.ComponentModel;

namespace RPG.BuildingBlocks.Common.Enums
{
    public enum CallEventType
    {
        [Description("AnswerCall")]
        ANSWER = 1,
        [Description("Candidate")]
        CANDIDATE = 2,
        [Description("FinishCall")]
        FINISH = 3
    }
}