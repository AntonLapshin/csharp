using System.Data.Entity.ModelConfiguration;
using ChatService.Infrastructure.Entities;

namespace ChatService.Infrastructure.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("User");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.HashPassword).HasColumnName("HashPassword");
        }
    }
}
