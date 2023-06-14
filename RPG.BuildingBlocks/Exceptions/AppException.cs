using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RPG.BuildingBlocks.Common.Exceptions
{
    public class AppException : Exception
    {
        public ActionResponse ActionResponse { get; set; }

        public AppException(string message) : base(message)
        {
            ActionResponse = new ActionResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Title = message,
                Status = "SUCCESS"
            };
        }

        public AppException(string message, HttpStatusCode httpStatusCode, bool isPubsub = false) : base(message)
        {
            ActionResponse = new ActionResponse
            {
                StatusCode = httpStatusCode,
                Title = message,
                Status = isPubsub ? "DROP" : "SUCCESS"
            };
        }

        public AppException(HttpStatusCode httpStatusCode) : base(string.Empty)
        {
            ActionResponse = new ActionResponse
            {
                StatusCode = httpStatusCode,
                Title = string.Empty,
                Status = "SUCCESS"
            };
        }
    }
}