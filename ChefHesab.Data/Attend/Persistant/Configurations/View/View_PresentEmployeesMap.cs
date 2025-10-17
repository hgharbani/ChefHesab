  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
    {
   public class View_PresentEmployeesMap : IEntityTypeConfiguration<View_PresentEmployees>
  {
  public void Configure(EntityTypeBuilder<View_PresentEmployees> builder)
 {
  // Table & Column Mappings
 builder.ToView("View_PresentEmployees","dbo");
 builder.Property(t => t.Id).HasColumnName("Id"); 
 builder.Property(t => t.EmployeeId).HasColumnName("EmployeeId"); 
 builder.Property(t => t.EmployeeNumber).HasColumnName("EmployeeNumber").IsRequired().HasMaxLength(50) ; 
 builder.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(500) ; 
 builder.Property(t => t.Family).HasColumnName("Family").IsRequired().HasMaxLength(500) ; 
 builder.Property(t => t.TeamWorkId).HasColumnName("TeamWorkId"); 
 builder.Property(t => t.JobPositionId).HasColumnName("JobPositionId"); 
 builder.Property(t => t.WorkDay).HasColumnName("WorkDay"); 
 builder.Property(t => t.IsPresent).HasColumnName("IsPresent"); 
 builder.Property(t => t.TeamWorkCode).HasColumnName("TeamWorkCode").IsRequired().HasMaxLength(50) ; 
 builder.Property(t => t.TeamWorkTitle).HasColumnName("TeamWorkTitle").IsRequired().HasMaxLength(500) ; 
 builder.Property(t => t.MisJobPositionCode).HasColumnName("MisJobPositionCode"); 
 builder.Property(t => t.JobPositionTitle).HasColumnName("JobPositionTitle"); 
 builder.Property(t => t.CostCenter).HasColumnName("CostCenter"); 
 builder.Property(t => t.IsKscEmployee).HasColumnName("IsKscEmployee"); 
 builder.Property(t => t.ContractCode).HasColumnName("ContractCode").IsRequired().HasMaxLength(12) ; 
 builder.Property(t => t.ContractDescription).HasColumnName("ContractDescription").IsRequired().HasMaxLength(100) ; 
 builder.Property(t => t.Contractor).HasColumnName("Contractor"); 
 builder.Property(t => t.ContractorID).HasColumnName("ContractorID"); 
 

 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
  }
  }
  }

