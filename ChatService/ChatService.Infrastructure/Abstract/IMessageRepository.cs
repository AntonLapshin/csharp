using System;
using System.Linq;
using ChatService.Infrastructure.Entities;

namespace ChatService.Infrastructure.Abstract
{
    public interface IMessageRepository : IRepository<Message>
    {
        IQueryable<Message> GetMessagesByUser(User user);
        IQueryable<Message> GetMessagesByRangeDate(DateTime from, DateTime to);
    }
}
