using System;

namespace RPG.BuildingBlocks.Common.SharedServices.Identity.Dto
{
    public class UserInfoDto
    {
        public Guid UserId {get;set;}
        public string Username {get;set;} 
        public string Fullname {get;set;}
        public DateTime CreatedDate { get; set; }
    }
}
