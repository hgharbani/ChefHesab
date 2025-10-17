using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.EmployeeBase
{
    public class FranchiseTypeConfiguration : IEntityTypeConfiguration<FranchiseType>
    {
        public void Configure(EntityTypeBuilder<FranchiseType> builder)
        {
            builder.ToTable("FranchiseType", "EmployeeBase");
            builder.HasKey(x => x.Id).HasName("PK_FranchiseType").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(x => x.DefualtValue).HasColumnName(@"DefualtValue").HasColumnType("float").IsRequired();
        }
    }
}
