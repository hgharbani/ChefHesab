using Ksc.HR.Domain.Entities.ODSViews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ksc.HR.Data.Persistant.Configurations
{
    // View_ODS_TherapyBooklet_MIS
    public class ViewOdsTherapyBookletMisConfiguration : IEntityTypeConfiguration<ViewOdsTherapyBookletMis>
    {
        public void Configure(EntityTypeBuilder<ViewOdsTherapyBookletMis> builder)
        {
            builder.ToView("View_ODS_TherapyBooklet_MIS", "dbo");
            builder.HasNoKey();

            builder.Property(x => x.IsnNum).HasColumnName(@"ISN_NUM").HasColumnType("numeric(8,0)").HasPrecision(8,0).IsRequired(false);
            builder.Property(x => x.NumNnalThbkt).HasColumnName(@"NUM_NNAL_THBKT").HasColumnType("numeric(10,0)").HasPrecision(10,0).IsRequired(false);
            builder.Property(x => x.NumPrsnThbkt).HasColumnName(@"NUM_PRSN_THBKT").HasColumnType("numeric(8,0)").HasPrecision(8,0).IsRequired(false);
            builder.Property(x => x.NamThbkt).HasColumnName(@"NAM_THBKT").HasColumnType("nvarchar(20)").IsRequired(false).HasMaxLength(20);
            builder.Property(x => x.NamFamThbkt).HasColumnName(@"NAM_FAM_THBKT").HasColumnType("nvarchar(25)").IsRequired(false).HasMaxLength(25);
            builder.Property(x => x.CodSexThbkt).HasColumnName(@"COD_SEX_THBKT").HasColumnType("numeric(1,0)").HasPrecision(1,0).IsRequired(false);
            builder.Property(x => x.NumCrtThbkt).HasColumnName(@"NUM_CRT_THBKT").HasColumnType("numeric(10,0)").HasPrecision(10,0).IsRequired(false);
            builder.Property(x => x.DatBrtThbkt).HasColumnName(@"DAT_BRT_THBKT").HasColumnType("numeric(8,0)").HasPrecision(8,0).IsRequired(false);
            builder.Property(x => x.CodDpncyThbkt).HasColumnName(@"COD_DPNCY_THBKT").HasColumnType("numeric(2,0)").HasPrecision(2,0).IsRequired(false);
            builder.Property(x => x.NamFthrThbkt).HasColumnName(@"NAM_FTHR_THBKT").HasColumnType("nvarchar(20)").IsRequired(false).HasMaxLength(20);
            builder.Property(x => x.NumBookThbkt).HasColumnName(@"NUM_BOOK_THBKT").HasColumnType("nvarchar(12)").IsRequired().HasMaxLength(12);
            builder.Property(x => x.NumStrBookThbkt).HasColumnName(@"NUM_STR_BOOK_THBKT").HasColumnType("numeric(11,0)").HasPrecision(11,0).IsRequired(false);
            builder.Property(x => x.NumEndBookThbkt).HasColumnName(@"NUM_END_BOOK_THBKT").HasColumnType("numeric(11,0)").HasPrecision(11,0).IsRequired(false);
            builder.Property(x => x.DatEndBookThbkt).HasColumnName(@"DAT_END_BOOK_THBKT").HasColumnType("numeric(8,0)").HasPrecision(8,0).IsRequired(false);
            builder.Property(x => x.FlgExmptThbkt).HasColumnName(@"FLG_EXMPT_THBKT").HasColumnType("numeric(1,0)").HasPrecision(1,0).IsRequired(false);
            builder.Property(x => x.CodStaThbkt).HasColumnName(@"COD_STA_THBKT").HasColumnType("numeric(1,0)").HasPrecision(1,0).IsRequired(false);
            builder.Property(x => x.NumLevThbkt).HasColumnName(@"NUM_LEV_THBKT").HasColumnType("numeric(2,0)").HasPrecision(2,0).IsRequired(false);
            builder.Property(x => x.TypFrchThbkt).HasColumnName(@"TYP_FRCH_THBKT").HasColumnType("nvarchar(15)").IsRequired(false).HasMaxLength(15);
            builder.Property(x => x.PcnFrchThbkt).HasColumnName(@"PCN_FRCH_THBKT").HasColumnType("numeric(5,2)").HasPrecision(5,2).IsRequired(false);
            builder.Property(x => x.DatStrDpncyThbkt).HasColumnName(@"DAT_STR_DPNCY_THBKT").HasColumnType("numeric(8,0)").HasPrecision(8,0).IsRequired(false);
            builder.Property(x => x.DatEndDpncyThbkt).HasColumnName(@"DAT_END_DPNCY_THBKT").HasColumnType("numeric(8,0)").HasPrecision(8,0).IsRequired(false);
            builder.Property(x => x.CodEndThbkt).HasColumnName(@"COD_END_THBKT").HasColumnType("numeric(2,0)").HasPrecision(2,0).IsRequired(false);
            builder.Property(x => x.DatUpdThbkt).HasColumnName(@"DAT_UPD_THBKT").HasColumnType("numeric(8,0)").HasPrecision(8,0).IsRequired(false);
            builder.Property(x => x.CodUsrThbkt).HasColumnName(@"COD_USR_THBKT").HasColumnType("nvarchar(8)").IsRequired(false).HasMaxLength(8);
            builder.Property(x => x.FlgStaBookThbkt).HasColumnName(@"FLG_STA_BOOK_THBKT").HasColumnType("numeric(1,0)").HasPrecision(1,0).IsRequired(false);
            builder.Property(x => x.NumAcnBnkThbkt).HasColumnName(@"NUM_ACN_BNK_THBKT").HasColumnType("nvarchar(25)").IsRequired(false).HasMaxLength(25);
            builder.Property(x => x.CodCityThbkt).HasColumnName(@"COD_CITY_THBKT").HasColumnType("numeric(5,0)").HasPrecision(5,0).IsRequired(false);
            builder.Property(x => x.FkBktyp).HasColumnName(@"FK_BKTYP").HasColumnType("nvarchar(2)").IsRequired(false).HasMaxLength(2);
            builder.Property(x => x.CodBnkPensnThbkt).HasColumnName(@"COD_BNK_PENSN_THBKT").HasColumnType("nvarchar(2)").IsRequired(false).HasMaxLength(2);
            builder.Property(x => x.CodRngCityThbkt).HasColumnName(@"COD_RNG_CITY_THBKT").HasColumnType("nvarchar(2)").IsRequired(false).HasMaxLength(2);
            builder.Property(x => x.CodCntryCityThbkt).HasColumnName(@"COD_CNTRY_CITY_THBKT").HasColumnType("nvarchar(3)").IsRequired(false).HasMaxLength(3);
            builder.Property(x => x.Expr1).HasColumnName(@"Expr1").HasColumnType("numeric(2,0)").HasPrecision(2,0).IsRequired(false);
            builder.Property(x => x.Expr2).HasColumnName(@"Expr2").HasColumnType("numeric(2,0)").HasPrecision(2,0).IsRequired(false);
            builder.Property(x => x.NumCntrPensnThbkt).HasColumnName(@"NUM_CNTR_PENSN_THBKT").HasColumnType("numeric(8,0)").HasPrecision(8,0).IsRequired(false);
            builder.Property(x => x.HisFlgOdsdb).HasColumnName(@"HIS_FLG_ODSDB").HasColumnType("bit").IsRequired(false);
        }
    }

}
// </auto-generated>
