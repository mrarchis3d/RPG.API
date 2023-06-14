using RPG.BuildingBlocks.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RPG.BuildingBlocks.Common.IPFS
{
    public class IPFSMock : IIPFS
    {
        public Task<Stream> GetFile(string cid)
        {
            System.IO.Stream stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes("whatever"));            
            return Task.FromResult(stream);
        }

        public Task<string> UploadFileToIpfsCluster(IFormFile file)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
