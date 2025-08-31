using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Dtos;
using DotNetMessagingApplication.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMessagingApplication.Server.Controllers;

[ApiController]
[Route("api/controllers")]
	public class MessageController : ControllerBase
	{
		private readonly IMessageService _messageService;

		public MessageController(IMessageService messageService) {
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

		[HttpDelete("DeleteMessage/{messageId}")]
		public IActionResult DeleteMessage([FromBody] DeleteMessageDto deletedMessage)
		{
			if (deletedMessage is null)
			{
				return BadRequest("You sure there's a message ?? Couldn't find one D:");
			}

			//var chat = _messageService

			//if (messageId < 0)
			//{
			//	return BadRequest("Invalid message ID :(");
			//}

			//_messageService.DeleteMessage(messageId);
			return Ok(1);
		}

		[HttpPut("EditMessage/{messageId}")]
		public IActionResult EditMessage(int messageId)
		{
			if (messageId < 0)
			{
				return BadRequest("Invalid message id D:");
			}

			_messageService.UpdateMessage(messageId);
			return Ok(1);
		}
	}
