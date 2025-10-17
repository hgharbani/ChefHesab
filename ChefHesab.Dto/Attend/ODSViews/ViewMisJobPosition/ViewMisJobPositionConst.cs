using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Ksc.HR.DTO.ODSViews.ViewMisJobPosition
{
    public static class ViewMisJobPositionConst
    {
        public static string GetJobCategoryTitle(string JobCategoryCode, string JobStatusCode)
        {
            string title = "";
            switch (JobCategoryCode)
            {
                case "MA":
                    switch (JobStatusCode)
                    {
                        case "1":
                            title = EnumJobStatusCode.Boss.Name;
                            break;
                        default:
                            break;
                    }
                    break;
                case "TM":
                    switch (JobStatusCode) //مثال : 2= مدیرتاپ چارت ، 3 = مدیر میانی ، 12 = معاونت	

                    {
                        case "2":
                            title = EnumJobStatusCode.TopManager.Name;
                            break;
                        case "3":
                            title = EnumJobStatusCode.MiddleManager.Name;
                            break;
                        case "12":
                            title = EnumJobStatusCode.Assistance.Name;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return title;
        }
    }
}



