using ChatService.Infrastructure.Entities;

namespace ChatService.Infrastructure.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByName(string name);
        bool Contains(string name);
    }
}
