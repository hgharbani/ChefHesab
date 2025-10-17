using Ksc.HR.Domain.Entities.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Ksc.HR.Data.Persistant.Configurations
{
    // EmployeeWorkGroup
    public class EmployeeWorkGroupConfiguration : IEntityTypeConfiguration<EmployeeWorkGroup>
    {
        public void Configure(EntityTypeBuilder<EmployeeWorkGroup> builder)
        {
            builder.ToTable("EmployeeWorkGroup", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_EmployeeWorkGroup").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.EmployeeId).HasColumnName(@"EmployeeId").HasColumnType("int").IsRequired();
            builder.Property(x => x.WorkGroupId).HasColumnName(@"WorkGroupId").HasColumnType("int").IsRequired();
            builder.Property(x => x.StartDate).HasColumnName(@"StartDate").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EndDate).HasColumnName(@"EndDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.TransferRequestId).HasColumnName(@"TransferRequestId").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.UpdateDate).HasColumnName(@"UpdateDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.UpdateUser).HasColumnName(@"UpdateUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
            builder.Property(x => x.WorkTimeChange).HasColumnName(@"WorkTimeChange").HasColumnType("bit").IsRequired();
            builder.Property(x => x.RowVersion).HasColumnName(@"RowVersion").HasColumnType("timestamp(8)").IsRequired().IsFixedLength().HasMaxLength(8).IsRowVersion().IsConcurrencyToken();

            // Foreign keys
            builder.HasOne(a => a.Employee).WithMany(b => b.EmployeeWorkGroups).HasForeignKey(c => c.EmployeeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeeWorkGroup_Employee");
            builder.HasOne(a => a.Transfer_Request).WithMany(b => b.EmployeeWorkGroups).HasForeignKey(c => c.TransferRequestId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeeWorkGroup_Request");
            builder.HasOne(a => a.WorkGroup).WithMany(b => b.EmployeeWorkGroups).HasForeignKey(c => c.WorkGroupId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeeWorkGroup_WorkGroup");
        }
    }

}
// </auto-generated>
