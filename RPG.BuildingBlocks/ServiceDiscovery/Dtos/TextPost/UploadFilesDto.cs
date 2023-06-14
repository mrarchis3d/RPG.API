using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace RPG.BuildingBlocks.Common.ServiceDiscovery.Dtos.TextPost
{
    public class UploadFilesDto
    {
        public Guid TextPostId { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
