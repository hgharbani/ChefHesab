  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Data.Persistant.Configurations
{
   public class EmployeeBreastfeddingConfiguration : IEntityTypeConfiguration<EmployeeBreastfedding>
  {
        public void Configure(EntityTypeBuilder<EmployeeBreastfedding> builder)
        {
            builder.ToTable("EmployeeBreastfedding", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_EmployeeBreastfedding").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();

            builder.Property(x => x.EmployeeId).HasColumnName(@"EmployeeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.BreastfeddingStartDate).HasColumnName(@"BreastfeddingStartDate").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.BreastfeddingEndDate).HasColumnName(@"BreastfeddingEndDate").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.IsBreastfeddingInStartShift).HasColumnName(@"IsBreastfeddingInStartShift").HasColumnType("bit").IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            //builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired();
            //builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.Employee).WithMany(b => b.EmployeeBreastfeddings).HasForeignKey(c => c.EmployeeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeeBreastfedding_Employee");
        }
    }
  }

