using System.Collections.Generic;
using System.Threading.Tasks;
using StarrAPI.AutoMapperHelp;
using StarrAPI.DTOs;
using StarrAPI.Models;

namespace StarrAPI.Data.Interfaces
{
    public interface IMessagesRepository
    {
        Task<Connections> GetConnections(string ConnectionId);
        Task<Group> GetGroup(string GroupName);
        Task<Group> GetGroupForConnection(string ConnectionId);
        void RemoveConnection(Connections connection);
        void AddGroup(Group group);
        void AddMessage(Messages message);
        void DeleteMessage(Messages messages);
        Task<Messages> GetMessage(int Id);
        Task<PagerList<MessagesDTO>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessagesDTO>> GetMessageThread(string CurrentUsername, string RecipientUsername);
        Task<bool> SaveAllAsync(); 
    }
}