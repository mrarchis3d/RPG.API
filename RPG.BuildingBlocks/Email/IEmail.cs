using System.Threading.Tasks;

namespace RPG.BuildingBlocks.Common.Email
{
    public interface IEmail
    {
        Task SendEmailAsync(string emailTo, string subject, string body);
    }
}
