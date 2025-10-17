

namespace Ksc.HR.DTO.Personal.EmployeeTeamWork
{
    public class SearchEmployeeTeamWorkModel
    {
        public class EmployeeTeamWorkModel
        {
            // EmployeeTeamWork
            public class EmployeeTeamWork 
            {
                public int Id { get; set; } // Id (Primary key)

                /// <summary>
                /// شماره پرسنلی
                /// </summary>
                public int EmployeeId { get; set; } // EmployeeId

                /// <summary>
                /// گروه کاری
                /// </summary>
                public int TeamWorkId { get; set; } // TeamWorkId



            }

        }
    }
}
