using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Salary
{
    public class PaymentType : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool IsActive { get; set; }

        // Reverse navigation

        /// <summary>
        /// Child PaymentAccountCodes where [PaymentAccountCode].[PaymentTypeId] point to this entity (FK_PaymentAccountCode_PaymentType)
        /// </summary>
        public virtual ICollection<PaymentAccountCode> PaymentAccountCodes { get; set; } // PaymentAccountCode.FK_PaymentAccountCode_PaymentType

        public PaymentType()
        {
            PaymentAccountCodes = new List<PaymentAccountCode>();
        }
    }
}
