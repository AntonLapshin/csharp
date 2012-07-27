using System.Data.Entity.ModelConfiguration;
using ChatService.Infrastructure.Entities;

namespace ChatService.Infrastructure.Mapping
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.MessageText)
                .IsRequired()
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Message");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserNum).HasColumnName("User");
            this.Property(t => t.MessageText).HasColumnName("MessageText");
            this.Property(t => t.Timestamp).HasColumnName("Timestamp");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Messages)
                .HasForeignKey(d => d.UserNum);

        }
    }
}
