using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
using System.Net;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class ConfirmInterdictMap : IEntityTypeConfiguration<ConfirmInterdict>
    {
        public void Configure(EntityTypeBuilder<ConfirmInterdict> builder)
        {
            // Table & Column Mappings
            builder.ToTable("ConfirmInterdict", "Rule");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.EmployeeInterdictId).HasColumnName("EmployeeInterdictId");
            builder.Property(t => t.ConfirmInterdictStatusId).HasColumnName("ConfirmInterdictStatusId");
            builder.Property(t => t.IsConfirm).HasColumnName("IsConfirm");
            builder.Property(t => t.ConfrimDate).HasColumnName("ConfrimDate");
            builder.Property(t => t.IsPrint).HasColumnName("IsPrint");
            builder.Property(t => t.PrintDate).HasColumnName("PrintDate");
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser").IsRequired().HasMaxLength(50);
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            builder.Property(t => t.IsFinal).HasColumnName("IsFinal");
            builder.Property(t => t.Address).HasColumnName("Address").IsRequired(false);
            


            builder.HasOne(t => t.EmployeeInterdict)
           .WithMany(t => t.ConfirmInterdicts)
         .HasForeignKey(d => d.EmployeeInterdictId);

            builder.HasOne(t => t.ConfirmInterdictStatus)
           .WithMany(t => t.ConfirmInterdict)
         .HasForeignKey(d => d.ConfirmInterdictStatusId);










        }
    }
}

