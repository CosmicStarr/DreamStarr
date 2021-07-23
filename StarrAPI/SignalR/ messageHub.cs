using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<PresentHub> _presents;
        private readonly PresentTracker _tracker;
        public messageHub(
        IMessagesRepository messages, 
        IMapper mapper, 
        IUserRepository userRepository,
        IHubContext<PresentHub> presents,PresentTracker tracker)
        {
            _userRepository = userRepository;
            _presents = presents;
            _mapper = mapper;
            _messages = messages;
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var http = Context.GetHttpContext();
            var otherUser = http.Request.Query["user"].ToString();
            var groupName = GroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddtoGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdateGroup",group);

            var messages = await _messages.GetMessageThread(Context.User.GetUsername(), otherUser);
            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveConnection();
            await Clients.Group(group.GroupName).SendAsync("UpdateGroup",group);
            await base.OnDisconnectedAsync(exception);
        }

        private async Task<Group> AddtoGroup(string GroupName)
        {
            var group = await _messages.GetGroup(GroupName);
            var connection = new Connections(Context.ConnectionId,Context.User.GetUsername());
            if(group == null) 
            {
                group = new Group(GroupName);
                _messages.AddGroup(group);
            }
            group.Connections.Add(connection);

            if (await _messages.SaveAllAsync()) return group;
            throw new HubException("Failed to join!");
        }

        private async Task<Group> RemoveConnection()
        {
            var obj = await _messages.GetGroupForConnection(Context.ConnectionId);
            var connection = obj.Connections.FirstOrDefault(x =>x.ConnectionId == Context.ConnectionId);
            _messages.RemoveConnection(connection);
           if(await _messages.SaveAllAsync()) return obj;
           throw new HubException("Failed to disconnect!");
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
               var groupName = GroupName(sender.UserName,recipient.UserName); 
               var group = await _messages.GetGroup(groupName);

               if(group.Connections.Any(x => x.Username == recipient.UserName))
               {
                   Message.DateRead = DateTime.UtcNow;
               }
               else
               {
                   var connection = await _tracker.GetConnectionForUser(recipient.UserName);
                   if(connection != null)
                   {
                       await _presents.Clients.Clients(connection).SendAsync("NewMessageReceived", new {username=sender.UserName,alsoknowas=sender.AlsoKnownAs});
                   }
               }
            _messages.AddMessage(Message);

            if (await _messages.SaveAllAsync())
            {
             
                await Clients.Group(groupName).SendAsync("NewMessage",_mapper.Map<MessagesDTO>(Message));
            }
        }

        private string GroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}