using System;

namespace RPG.BuildingBlocks.Common.EventBus.Events.LegalAgreement
{
    public class LegalAgreementSigningEvent
    {
        public string LegalAgreementID { get; set; }
        public string LegalAgreementContentHash { get; set; }
        public string UserID { get; set; }
        public long CreatedTimestamp { get; set; }
    }
}
