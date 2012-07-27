using System.Data.Entity;

namespace ChatService.Infrastructure.Concrete
{
    public class ContextHelper
    {
        private static object _lockObject = new object();
        private static DbContext _context;
        public static DbContext GetContext()
        {
            if (_context == null)

                lock (_lockObject)
                {
                    _context = new ChatEntities();
                }
            return _context;
        }
    }
}
