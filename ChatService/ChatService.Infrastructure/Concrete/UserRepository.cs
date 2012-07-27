using System.Data.Entity;
using System.Linq;
using ChatService.Infrastructure.Abstract;
using ChatService.Infrastructure.Entities;

namespace ChatService.Infrastructure.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IContextProvider contextProvider)
            : base(contextProvider)
        {
        }

        public User GetUserByName(string name)
        {
            return Filter(x => x.Name == name).FirstOrDefault();
        }
        public override void Add(User tEntity)
        {
            tEntity.SetHashPassword();
            base.Add(tEntity);
        }
        public bool Contains(string name)
        {
            return GetUserByName(name) != null;
        }
    }
}
