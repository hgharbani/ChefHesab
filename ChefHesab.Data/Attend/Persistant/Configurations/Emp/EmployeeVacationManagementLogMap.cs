using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
{
    public class EmployeeVacationManagementLogMap : IEntityTypeConfiguration<EmployeeVacationManagementLog>
    {
        public void Configure(EntityTypeBuilder<EmployeeVacationManagementLog> builder)
        {
            // Table & Column Mappings
            builder.ToTable("EmployeeVacationManagementLog", "dbo");
            builder.Property(t => t.Id).HasColumnName("Id");
            builder.Property(t => t.EmployeeVacationManagementId).HasColumnName("EmployeeVacationManagementId");
            builder.Property(t => t.VacationTitle).HasColumnName("VacationTitle");
            builder.Property(t => t.ValueDuration).HasColumnName("ValueDuration");
            builder.Property(t => t.Duration).HasColumnName("Duration");
            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser");
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            builder.Property(t => t.Remark).HasColumnName("Remark");



            builder.HasOne(t => t.EmployeeVacationManagement)
           .WithMany(t => t.EmployeeVacationManagementLogs)
         .HasForeignKey(d => d.EmployeeVacationManagementId);









        }
    }
}

