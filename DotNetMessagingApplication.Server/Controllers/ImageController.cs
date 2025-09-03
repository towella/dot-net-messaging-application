using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories;
using DotNetMessagingApplication.Server.Dtos;
using DotNetMessagingApplication.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DotNetMessagingApplication.Server.Controllers
{
	[ApiController]
	[Route("api/images")]
	public class MessageController : ControllerBase
	{
		private readonly IBlobService _blobService;
		private readonly IMessageService _messageService;
		private readonly IChatService _chatService;
		private readonly IUserRepository _userRepository;

        public MessageController(IBlobService blobService, IMessageService messageService, IChatService chatService, IUserRepository userRepository)
		{
			_blobService = blobService;
			_messageService = messageService;
			_chatService = chatService;
			_userRepository = userRepository;
        }

		[HttpPost("upload")]
		public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request)
		{
			if (request.Image == null || request.Image.Length == 0)
				return BadRequest("No image uploaded.");
			try
			{
				var imageUrl = await _blobService.UploadImage(request.Image);

				var chat = await _chatService.GetChatById(request.ChatId);

                var message = new Message
				{
					ChatId = request.ChatId,
					SenderId = request.SenderId,
					MessageBody = request.MessageBody,
					ImageUrl = imageUrl,
					Chat = chat!,
					RecipientChat = chat!,
					RecipientChatId = chat!.ChatId,
                };

				var savedMessage = _messageService.SendMessage(message);
				return Ok(savedMessage);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image: " + ex.Message);
			}
		}

		[HttpGet("{fileName}")]
		public async Task<IActionResult> GetImage(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
				return BadRequest("Filename is required.");

			var imageStream = await _blobService.GetImage(fileName);

			if (imageStream == null)
				return NotFound("Image not found.");

			// You can set the content type dynamically if needed
			return File(imageStream, "image/jpeg"); // or "image/png" depending on your blob type
		}
	}
}
