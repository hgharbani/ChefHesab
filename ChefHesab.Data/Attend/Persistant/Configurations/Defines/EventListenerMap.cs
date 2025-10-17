using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class EventListenerMap : IEntityTypeConfiguration<EventListener>
    {
        public void Configure(EntityTypeBuilder<EventListener> builder)
        {
            // Table & Column Mappings
            builder.ToTable("EventListener", "dbo");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.Title).HasColumnName("Title");
            builder.Property(t => t.StepId).HasColumnName("StepId");
            builder.Property(t => t.IsError).HasColumnName("IsError");
            builder.Property(t => t.ErrorMessage).HasColumnName("ErrorMessage");
            builder.Property(t => t.IsFinished).HasColumnName("IsFinished");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser");
        }
    }
}

