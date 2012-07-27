using System.Data.Entity;

namespace ChatService.Infrastructure.Abstract
{
    public interface IContextProvider
    {
        DbContext CurrentContext { get; }
    }
}
