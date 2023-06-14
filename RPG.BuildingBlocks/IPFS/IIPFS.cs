using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RPG.BuildingBlocks.Common.IPFS
{
    public interface IIPFS
    {
        Task<string> UploadFileToIpfsCluster(IFormFile file);
        Task<Stream> GetFile(string cid);
    }
}
