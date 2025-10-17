  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Ksc.Hr.Domain.Entities;
namespace Ksc.Hr.Data.Persistant.Configurations
    {
   public class HrOptionMap : IEntityTypeConfiguration<HrOption>
  {
  public void Configure(EntityTypeBuilder<HrOption> builder)
 {
  // Table & Column Mappings
 builder.ToTable("HrOption","dbo");
 builder.Property(t => t.Id).HasColumnName("Id"); 
 builder.Property(t => t.Title).HasColumnName("Title"); 
 builder.Property(t => t.OptionTypeId).HasColumnName("OptionTypeId"); 
 builder.Property(t => t.ValueString).HasColumnName("ValueString"); 
 builder.Property(t => t.ValueTime).HasColumnName("ValueTime"); 
 builder.Property(t => t.ValueFloat).HasColumnName("ValueFloat"); 
 builder.Property(t => t.Code).HasColumnName("Code"); 
 

 
 
 
 
 
 
 
  }
  }
  }

