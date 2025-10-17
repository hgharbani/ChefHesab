  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
    {
   public class EmployeeVacationManagementMap : IEntityTypeConfiguration<EmployeeVacationManagement>
  {
  public void Configure(EntityTypeBuilder<EmployeeVacationManagement> builder)
 {
  // Table & Column Mappings
 builder.ToTable("EmployeeVacationManagement","dbo");
 builder.Property(t => t.Id).HasColumnName("Id"); 
 builder.Property(t => t.EmployeeId).HasColumnName("EmployeeId"); 
 builder.Property(t => t.VacationId).HasColumnName("VacationId"); 
 builder.Property(t => t.ValueDuration).HasColumnName("ValueDuration"); 
 builder.Property(t => t.Duration).HasColumnType("float").HasColumnName("Duration");
 builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
 builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
 builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
 builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
 builder.Property(x => x.Remark).HasColumnName(@"Remark").HasColumnType("nvarchar(MAX)").IsRequired(false);



            builder.HasOne(t => t.Employee)
   .WithMany(t => t.EmployeeVacationManagement)
 .HasForeignKey(d => d.EmployeeId);
 
    builder.HasOne(t => t.Vacation)
   .WithMany(t => t.EmployeeVacationManagement)
 .HasForeignKey(d => d.VacationId);
 
 
 
  }
  }
  }

