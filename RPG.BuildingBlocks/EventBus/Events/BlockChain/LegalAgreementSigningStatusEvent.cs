namespace RPG.BuildingBlocks.Common.EventBus.Events.BlockChain
{
    public class LegalAgreementSigningStatusEvent
    {
        public string UserID { get; set; }
        public string LegalAgreementSigningStatus { get; set; }
    }
}