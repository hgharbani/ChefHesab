using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class HrFormulas : IEntityBase<int>
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public string Experssion { get; set; }
        public string ExperssionPersianTitle { get; set; }
        public string Parameters { get; set; }
    }
}

