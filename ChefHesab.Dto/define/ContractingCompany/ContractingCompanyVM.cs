using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.define.ContractingCompany
{
    public class ContractingCompanyVM : filterDataRequest
    {
        public Guid Id { get; set; }
        [StringLength(300)]
        public string CompanyName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? AgreementDate { get; set; }
        public int? AgreementPeriod { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; }
        public string AgreementNumber { get; set; }
        public bool? IsActive { get; set; }
        public Guid? PersonalId { get; set; }
    }
}
