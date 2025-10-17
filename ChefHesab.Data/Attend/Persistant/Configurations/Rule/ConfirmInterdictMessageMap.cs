using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class ConfirmInterdictMessageMap : IEntityTypeConfiguration<ConfirmInterdictMessage>
    {
        public void Configure(EntityTypeBuilder<ConfirmInterdictMessage> builder)
        {
            // Table & Column Mappings
            builder.ToTable("ConfirmInterdictMessage", "Rule");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.ConfirmInterdictId).HasColumnName("ConfirmInterdictId");
            builder.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber").IsRequired().HasMaxLength(20);
            builder.Property(t => t.Message).HasColumnName("Message").IsRequired().HasMaxLength(500);
            builder.Property(t => t.SendTime).HasColumnName("SendTime");
            builder.Property(t => t.ConfirmCode).HasColumnName("ConfirmCode").IsRequired().HasMaxLength(5);



            builder.HasOne(t => t.ConfirmInterdict)
           .WithMany(t => t.ConfirmInterdictMessage)
         .HasForeignKey(d => d.ConfirmInterdictId);





        }
    }
}

