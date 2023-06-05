using System.Net;

namespace LinkStorage.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
            SuccessMessage= new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set;}
        public List<string> SuccessMessage { get; set;}
        public object Result { get; set; }
    }
}
  