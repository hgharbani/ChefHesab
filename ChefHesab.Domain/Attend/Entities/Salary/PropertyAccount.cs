using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Salary
{
    public class PropertyAccount : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PropertyAccountTypeId { get; set; }
        public bool IsCheckedValidDate { get; set; }
        public int? StartValidDate { get; set; }
        public int? EndValidDate { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool IsActive { get; set; }
        public virtual PropertyAccountType PropertyAccountType { get; set; }

        // Reverse navigation

        /// <summary>
        /// Child PropertyAccountSettings where [PropertyAccountSetting].[PropertyAccountId] point to this entity (FK_PropertyAccountSetting_PropertyAccount)
        /// </summary>
        public virtual ICollection<PropertyAccountSetting> PropertyAccountSettings { get; set; } // PropertyAccountSetting.FK_PropertyAccountSetting_PropertyAccount

        public PropertyAccount()
        {
            PropertyAccountSettings = new List<PropertyAccountSetting>();
        }
    }
}
