using Ksc.HR.Domain.Entities.Personal;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities
{
    // TableDefinition
    /// <summary>
    /// تعریف جداول پایه پرسنلی
    /// </summary>
    public class TableDefinition : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// نام فارسی جدول
        /// </summary>
        public string Title { get; set; } // Title (length: 150)
        public string UrlName { get; set; } // UrlName (length: 500)

        /// <summary>
        /// نام انگلیسی جدول
        /// </summary>
        public string TableName { get; set; } // TableName (length: 150)
        public bool IsActive { get; set; } // IsActive
    }
}

