using Microsoft.AspNetCore.Http;

namespace user_management.domain.Models
{
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attechments { get; set; }
    }
}
