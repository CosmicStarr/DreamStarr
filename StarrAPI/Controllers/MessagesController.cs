using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarrAPI.AutoMapperHelp;
using StarrAPI.Data.Interfaces;
using StarrAPI.DTOs;
using StarrAPI.Extensions;
using StarrAPI.Models;

namespace StarrAPI.Controllers
{
    [Authorize]
    public class MessagesController : BaseAPIController
    {
        public readonly IUserRepository _userRepository;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IMapper _mapper;
        public MessagesController(IUserRepository userRepository, IMessagesRepository messagesRepository, IMapper mapper)
        {
            _mapper = mapper;
            _messagesRepository = messagesRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessagesDTO>> CreateMessge(CreateMessageDTO createMessagesDTO)
        {
            var user = User.GetUsername();

            if (user == createMessagesDTO.RecipientUsername.ToLower())
                return BadRequest("You can't send a message to yourself!");

            var sender = await _userRepository.GetUserByUsernameAsync(user);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessagesDTO.RecipientUsername);

            if (recipient == null) return NotFound();

            var Message = new Messages
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.Username,
                RecipientUsername = recipient.Username,
                Content = createMessagesDTO.Content
            };

            _messagesRepository.AddMessage(Message);

            if(await _messagesRepository.SaveAllAsync()) return Ok(_mapper.Map<MessagesDTO>(Message));
            return BadRequest("Failed to send message!");
        }
        [HttpGet]
        public async Task<IEnumerable<MessagesDTO>> GetMessagesForUser([FromQuery]MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var Messages = await _messagesRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(Messages.CurrentPage,Messages.PageSize,Messages.TotalCount,Messages.TotalPages);
            return Messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessagesDTO>>> GetMessageThread(string username)
        {
            var currentUser = User.GetUsername();
            return Ok(await _messagesRepository.GetMessageThread(currentUser,username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {         
            var username = User.GetUsername();
            var msg = await _messagesRepository.GetMessage(id);
            if(msg == null) return NotFound();
            if(msg.Sender.Username != username && msg.Recipient.Username != username) return Unauthorized();
            if(msg.Sender.Username == username) msg.SenderDelete = true;
            if(msg.Recipient.Username == username) msg.RecipientDelete = true;
            if(msg.SenderDelete && msg.RecipientDelete) _messagesRepository.DeleteMessage(msg);
            if(await _messagesRepository.SaveAllAsync()) return Ok();
            return BadRequest("There was a problem trying to delete your message!");
        }
    }
}