using System;
using System.Data.Entity;
using System.Linq;
using ChatService.Infrastructure.Abstract;
using ChatService.Infrastructure.Entities;

namespace ChatService.Infrastructure.Concrete
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(IContextProvider contextProvider)
            : base(contextProvider)
        {
        }

        public IQueryable<Message> GetMessagesByUser(User user)
        {
            return Filter(x => x.User.Id == user.Id);
        }

        public IQueryable<Message> GetMessagesByRangeDate(DateTime from, DateTime to)
        {
            return Filter(x => x.Timestamp >= from && x.Timestamp <= to);
        }
    }
}
