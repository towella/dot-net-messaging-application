using Microsoft.AspNetCore.Mvc;

namespace DotNetMessagingApplication.Server.Dtos
{
    public class UploadImageRequest
    {
        [FromForm(Name = "image")]
        public IFormFile Image { get; set; }

        [FromForm(Name = "chatId")]
        public int ChatId { get; set; }

        [FromForm(Name = "senderId")]
        public int SenderId { get; set; }
    }

}
