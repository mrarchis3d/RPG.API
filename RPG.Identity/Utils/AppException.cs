using System.Net;

namespace RPG.Identity.Utils
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
