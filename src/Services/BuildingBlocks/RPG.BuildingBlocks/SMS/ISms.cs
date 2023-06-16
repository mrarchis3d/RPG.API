using System.Threading.Tasks;

namespace RPG.BuildingBlocks.Common.SMS
{
    public interface ISms
    {
        Task SendSMSAsync(string message, string toNumber);
    }
}
