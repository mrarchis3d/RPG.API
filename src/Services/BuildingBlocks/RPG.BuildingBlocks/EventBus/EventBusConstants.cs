using System;
namespace RPG.BuildingBlocks.Common.EventBus
{
    public class EventBusConstants
    {
        #region pubsub components
        public const string COMPONENT_PUBSUB = "pubsub";
        public const string TOPIC_USERCREATED = "usercreated";
        public const string TOPIC_USERUPDATEDTHUMBNAILCID = "userupdatedthumbnailcid";
        public const string TOPIC_USERFOLLOWER = "userfollower";
        public const string TOPIC_USERUPDATED = "userupdated";

        public const string TOPIC_CHANGELIVEEVENTSTATUSTOACTIVE = "streammingprepublish";
        public const string TOPIC_CHANGELIVEEVENTSTATUSTOINACTIVE = "streammingdone";

        public const string TOPIC_CHANNELCREATED = "channelcreated";
        public const string TOPIC_CHANNELUPDATEDTHUMBNAILCID = "channelupdatedthumbnailcid";
        public const string TOPIC_CHANNELSUBSCRIPTION = "channelsubscription";

        public const string TOPIC_CHATGROUPUPDATEDTHUMBNAILCID = "chatgroupupdatedthumbnailcid";
        public const string TOPIC_CHATGROUPCREATED = "chatgroupcreated";
        public const string TOPIC_CHATSYSTEMMESSAGE = "chatsystemmessage";
        public const string TOPIC_CHATINCOMMINGCLOUDSETTINGSUPDATE = "chatincommingcloudsettingsupdate";
        public const string TOPIC_CHATOUTGOINGCLOUDSETTINGSUPDATE = "chatoutgoingcloudsettingsupdate";

        public const string TOPIC_FRIENDSHIPREQUESTED = "friendshiprequested";
        public const string TOPIC_FRIENDSHIPACCEPTED = "friendshipaccepted";
        public const string TOPIC_FRIENDSHIPCANCELED = "friendshipcanceled";
        public const string TOPIC_FRIENDSHIPREJECTED = "friendshiprejected";

        public const string TOPIC_LIVECONCERTUPDATEDTHUMBNAILCID = "liveconcertupdatedthumbnailcid";

        public const string TOPIC_LIVECONCERTTIERUPDATEDTHUMBNAILCID = "liveconcerttierupdatedthumbnailcid";

        public const string TOPIC_POSTVIDEOUPDATEDTHUMBNAILCID = "postvideoupdatedthumbnailcid";
        public const string TOPIC_POSTVIDEOCREATED = "postvideocreated";
        public const string TOPIC_POSTVIDEOUPDATED = "postvideoupdated";

        public const string TOPIC_PUSHNOTIFICATIONCREATED = "pushnotificationcreated";

        public const string TOPIC_LEGALAGREEMENTSIGNING = "legalagreementsigning";

        public const string TOPIC_INCOMMINGMESSAGE = "incommingmessage";
        public const string TOPIC_OUTGOINGMESSAGE = "outgoingmessage";

        public const string TOPIC_VIDEOREACTIONCREATED = "videoreactioncreated";
        public const string TOPIC_POSTVIDEOUPDATEFINGERPRINTING = "postvideoupdatedfingerprinting";

        public const string TOPIC_POSTAUDIOCREATED = "postaudiocreated";

        public const string TOPIC_TEXTPOSTCREATED = "textpostcreated";

        public const string TOPIC_LEGALAGREEMENTSIGNINGSTATUS = "legalagreementsigningstatus";
        public const string TOPIC_INCOMMINGCALL = "incomingcall";
        public const string TOPIC_NOTIFYCALL = "notificationreceived";

        public const string TOPIC_CIRCLEUPDATEDTHUMBNAILCID = "circleupdatedthumbnailcid";
        public const string TOPIC_UPDATEDATE_NEWPOST = "datepost";
        public const string TOPIC_CIRCLECREATED = "circlecreated";
        public const string TOPIC_CIRCLEPARTICIPANTADDED = "circleparticipantadded";
        public const string TOPIC_CIRCLEUPDATED = "circleupdated";
        public const string TOPIC_CIRCLEPRIVACYUPDATED = "circleprivacyupdated";
        public const string TOPIC_CIRCLECHATCREATED = "circlechatcreated";
        public const string TOPIC_ADMINLEAVE_CIRCLE = "adminleavecircle";
        
        public const string TOPIC_USERUNFOLLOW_CONTENTINDEX = "userunfollow";
        public const string TOPIC_USERUNSUBSCRIBE_CONTENTINDEX = "userunsubscribe";

        public const string TOPIC_AUDIOREACTIONCREATED = "audioreactioncreated";
        public const string TOPIC_TEXTPOSTREACTIONCREATED = "textpostreactioncreated";
        public const string TOPIC_COMMENTREACTIONCREATED = "commentreactioncreated";


        public const string TOPIC_SPACETHUMBNAILUPDATED = "spacethumbnailupdated";

        #endregion


    }
}
