using System.Data.Entity;
using ChatService.Infrastructure.Entities;
using ChatService.Infrastructure.Mapping;

namespace ChatService.Infrastructure.Concrete
{
    public class ChatEntities : DbContext
    {
        static ChatEntities()
        {
            Database.SetInitializer<ChatEntities>(null);
        }

        public ChatEntities()
            : base("Name=Entities")
		{
            
		}

        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MessageMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
