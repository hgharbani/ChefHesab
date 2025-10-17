  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Ksc.Hr.Domain.Entities.Personal;
namespace Ksc.Hr.Data.Persistant.Configurations.Personal
{
   public class OfficialMessageMap : IEntityTypeConfiguration<OfficialMessage>
  {
  public void Configure(EntityTypeBuilder<OfficialMessage> builder)
 {
  // Table & Column Mappings
 builder.ToTable("OfficialMessage","dbo");
 builder.Property(t => t.Id).HasColumnName("Id"); 
 builder.Property(t => t.StartDate).HasColumnName("StartDate"); 
 builder.Property(t => t.EndDate).HasColumnName("EndDate"); 
 builder.Property(t => t.Messages).HasColumnName("Messages").IsRequired() ; 
 builder.Property(t => t.InsertUser).HasColumnName("InsertUser").IsRequired().HasMaxLength(100) ; 
 

 
 
 
 
 
  }
  }
  }

