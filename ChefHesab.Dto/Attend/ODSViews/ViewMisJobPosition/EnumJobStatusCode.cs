using Ksc.HR.Share.General;
using Ksc.HR.DTO.WorkFlow.Classes.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Ksc.HR.DTO.ODSViews.ViewMisJobPosition
{
    public class EnumJobStatusCode : Enumeration
    {
        public static readonly EnumJobStatusCode Boss = new EnumJobStatusCode(1, "رئیس", null);
        public static readonly EnumJobStatusCode MiddleManager = new EnumJobStatusCode(3, "مدیر میانی", null);
        public static readonly EnumJobStatusCode TopManager = new EnumJobStatusCode(2, "مدیر تاپ چارت", null);
        public static readonly EnumJobStatusCode Assistance = new EnumJobStatusCode(12, "معاونت", null);
        public static readonly EnumJobStatusCode Agent = new EnumJobStatusCode(4, "مامور", null);

        public EnumJobStatusCode(int id, string name, string group)
            : base(id, name, group)
        {
        }

    }
}