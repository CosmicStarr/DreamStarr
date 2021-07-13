using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StarrAPI.AutoMapperHelp;
using StarrAPI.Data.Interfaces;
using StarrAPI.DTOs;
using StarrAPI.Models;

namespace StarrAPI.Data.Repositories
{
    public class MessageRepository : IMessagesRepository
    {
        private readonly ApplicationDbContext _Context;
        private readonly IMapper _mapper;
        public MessageRepository(ApplicationDbContext Context, IMapper mapper)
        {
            _mapper = mapper;
            _Context = Context;

        }
        public void AddMessage(Messages message)
        {
            _Context.Messages.Add(message);
        }

        public void DeleteMessage(Messages messages)
        {
            _Context.Messages.Remove(messages);
        }

        public async Task<Messages> GetMessage(int Id)
        {
            var Msg = await _Context.Messages
            .Include(x => x.Sender)
            .Include(x => x.Recipient)
            .FirstOrDefaultAsync(x => x.Messageid == Id);
            return Msg;
        }

        public async Task<PagerList<MessagesDTO>> GetMessagesForUser(MessageParams messageParams)
        {
            var messageQuery = _Context.Messages.OrderByDescending(m => m.MessageSent).AsQueryable();
            messageQuery = messageParams.Container switch
            {
                "Inbox" => messageQuery.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDelete == false),
                "Outbox" => messageQuery.Where(u => u.SenderUserName == messageParams.Username && u.SenderDelete == false),
                _ => messageQuery.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDelete == false && u.DateRead == null)
            };

            var messagesource = messageQuery.ProjectTo<MessagesDTO>(_mapper.ConfigurationProvider);

            return await PagerList<MessagesDTO>.CreateAsync(messagesource,messageParams.PageNumber,messageParams.PageSize);
        }

        public async Task<IEnumerable<MessagesDTO>> GetMessageThread(string CurrentUsername, string RecipientUsername)
        {
            var Messages = await _Context.Messages
            .Include(s => s.Sender).ThenInclude(p =>p.Photos)
            .Include(s => s.Recipient).ThenInclude(p =>p.Photos)
            .Where(m =>m.RecipientUsername == CurrentUsername && m.RecipientDelete == false
            && m.SenderUserName == RecipientUsername 
            || m.RecipientUsername == RecipientUsername 
            && m.Sender.Username == CurrentUsername && m.SenderDelete == false)
            .OrderBy(m =>m.MessageSent)
            .ToListAsync();

            var unreadMsg = Messages.Where(m => m.DateRead == null && m.Recipient.Username == CurrentUsername).ToList();
            if(unreadMsg.Any())
            {
                foreach (var item in unreadMsg)
                {
                    item.DateRead = DateTime.Now;
                }
                await _Context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessagesDTO>>(Messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _Context.SaveChangesAsync() > 0;
        }
    }
}