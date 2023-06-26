using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LinkStorage.Models
{
    public class OutputMessages
    {
        public OutputMessages()
        {
            Messages = new List<string>();
        }
        public List<string> Messages { get; set; }  
        public object Result { get; set; }
    }
}
