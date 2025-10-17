using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Workshift;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.EmployeeBase
{
    public class PersonalTypeConfiguration : IEntityTypeConfiguration<PersonalType>
    {
        public void Configure(EntityTypeBuilder<PersonalType> builder)
        {
            builder.ToTable("PersonalType", "EmployeeBase");
            builder.HasKey(x => x.Id).HasName("PK_PersonalType").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar(Max)").IsRequired(false);
            builder.Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(Max)").IsRequired(false);

            builder.Property(t => t.InsertDate).HasColumnName("InsertDate");
            builder.Property(t => t.InsertUser).HasColumnName("InsertUser").IsRequired(false);
            builder.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired(false);
            builder.Property(t => t.UpdateUser).HasColumnName("UpdateUser").IsRequired(false);
            builder.Property(t => t.IsActive).HasColumnName("IsActive");

        }
    }
}
