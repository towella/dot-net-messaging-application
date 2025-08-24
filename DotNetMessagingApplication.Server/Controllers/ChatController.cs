using DotNetMessagingApplication.Server.Data.Dtos;
using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMessagingApplication.Server.Controllers
{
	public class ChatController : Controller
	{
		private readonly IMessageService _messageService;

		public ChatController(IMessageService messageService) {
			_messageService = messageService;
		}

		[HttpPost("SendMessage")]
		public IActionResult SendMessage([FromBody] SendMessageDto sentMessage)
		{
			if (sentMessage is null)
			{
				return BadRequest("Message data is missing");
			}


			// get chat by id
			//Chat chat = 

			Message message = new()
			{
				MessageBody = sentMessage.Message,
				SenderId = sentMessage.SenderId,
				TimeSent = sentMessage.TimeStamp,

			};

			return Ok(1);
		}
	}
}
