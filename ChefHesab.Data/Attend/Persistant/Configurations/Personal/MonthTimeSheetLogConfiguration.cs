using Ksc.HR.Domain.Entities.Personal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Configurations.Personal
{
    // MonthTimeSheetLog
    public class MonthTimeSheetLogConfiguration : IEntityTypeConfiguration<MonthTimeSheetLog>
    {
        public void Configure(EntityTypeBuilder<MonthTimeSheetLog> builder)
        {
            builder.ToTable("MonthTimeSheetLog", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_MonthTimeSheetLog").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.YearMonth).HasColumnName(@"YearMonth").HasColumnType("int").IsRequired(false);
            builder.Property(x => x.MonthTimeShitStepperId).HasColumnName(@"MonthTimeShitStepperId").HasColumnType("int").IsRequired();
            builder.Property(x => x.InsertDate).HasColumnName(@"InsertDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.InsertUser).HasColumnName(@"InsertUser").HasColumnType("nvarchar(50)").IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.Result).HasColumnName(@"Result").HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(x => x.ResultCount).HasColumnName(@"ResultCount").HasColumnType("int").IsRequired();
        }
    }
}
