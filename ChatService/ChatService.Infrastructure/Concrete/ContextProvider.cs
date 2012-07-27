using System.Data.Entity;
using ChatService.Infrastructure.Abstract;

namespace ChatService.Infrastructure.Concrete
{
    public class ContextProvider : IContextProvider
    {
        public DbContext CurrentContext
        {
            get { return ContextHelper.GetContext(); }
        }
    }
}
