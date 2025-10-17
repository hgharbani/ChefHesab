#nullable disable
using KSC.Domain;
using System;
using System.Collections.Generic;

namespace Ksc.HR.Domain.Entities.Workshift
{
    /// <summary>
    /// کشور
    /// </summary>
    public partial class Country : IEntityBase<int>
    {
        public Country()
        {
            Province = new HashSet<Province>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? EnglishTitle { get; set; }
        public string Code { get; set; } = null!;
        public DateTime? InsertDate { get; set; }
        public string? InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public int? OrderNo { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        public virtual ICollection<Province> Province { get; set; }
    }
}