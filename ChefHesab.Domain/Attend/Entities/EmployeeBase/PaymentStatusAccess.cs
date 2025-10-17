using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Workshift;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.Hr.Domain.Entities
{
    public class PaymentStatusAccess : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public int? PaymentStatusId { get; set; } // PaymentStatusId
        public int AccessLevelId { get; set; } // AccessLevelId

        // Foreign keys

        /// <summary>
        /// Parent AccessLevel pointed by [PaymentStatusAccess].([AccessLevelId]) (FK_PaymentStatusAccess_AccessLevel)
        /// </summary>
        public virtual AccessLevel AccessLevel { get; set; } // FK_PaymentStatusAccess_AccessLevel
    }
}

