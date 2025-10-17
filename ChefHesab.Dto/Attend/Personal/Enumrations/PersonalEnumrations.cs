using Ksc.HR.Share.General;
using Ksc.HR.Share.Model.SystemSequenceStatus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Entities.Enumrations
{
  

    public class EnumPersonalForm : Enumeration
    {

        public static readonly EnumPersonalForm Women = new EnumPersonalForm(2, "زن", null);//gender
        public static readonly EnumPersonalForm men = new EnumPersonalForm(1, "مرد ", null);//gender
        public static readonly EnumPersonalForm MilitaryStatusId = new EnumPersonalForm(1, "انجام داده", null);//MilitaryStatus
        public EnumPersonalForm(int id, string name, string group)
    : base(id, name, group)
        {
        }

    }

}
