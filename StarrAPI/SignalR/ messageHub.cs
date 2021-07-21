using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using StarrAPI.Data.Interfaces;
using StarrAPI.Data.Repositories;
using StarrAPI.DTOs;
using StarrAPI.Extensions;
using StarrAPI.Models;

namespace StarrAPI.SignalR
{
    public class messageHub : Hub
    {
        private readonly IMessagesRepository _messages;
        private readonly IMapper _mapper;
        private readonly UserRepository _userRepository;
        public messageHub(IMessagesRepository messages, IMapper mapper, UserRepository userRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _messages = messages;

        }

        public override async Task OnConnectedAsync()
        {
            var http = Context.GetHttpContext();
            var otherUser = http.Request.Query["user"].ToString();
            var groupName = GroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var messages = await _messages.GetMessageThread(Context.User.GetUsername(), otherUser);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessages(CreateMessageDTO createMessagesDTO)
        {
            var user = Context.User.GetUsername();

            if (user == createMessagesDTO.RecipientUsername.ToLower())
                throw new HubException("You can't send a message to yourself!");

            var sender = await _userRepository.GetUserByUsernameAsync(user);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessagesDTO.RecipientUsername);

            if (recipient == null) throw new HubException("User not found!");

            var Message = new Messages
            {
                Sender = sender,
                Recipient = recipient,
                SenderUserName = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessagesDTO.Content
            };

            _messages.AddMessage(Message);

            if (await _messages.SaveAllAsync())
            {
                var group = GroupName(sender.UserName,recipient.UserName); 
                await Clients.Group(group).SendAsync("NewMessage",_mapper.Map<MessagesDTO>(Message));
            }
        }

        private string GroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}