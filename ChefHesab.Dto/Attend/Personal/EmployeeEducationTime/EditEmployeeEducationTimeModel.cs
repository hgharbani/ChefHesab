

using System;

namespace Ksc.HR.DTO.Personal.EmployeeEducationTime
{
    public class EditEmployeeEducationTimeModel
    {
        public string CurrentUserName { get; set; }
        public int Id { get; set; } // Id (Primary key)
        public int? EmployeeId { get; set; } // EmployeeId
        public int? TrainingTypeId { get; set; } // TrainingTypeId
        public string StartTime { get; set; } // StartTime (length: 5)
        public string EndTime { get; set; } // EndTime (length: 5)
        public DateTime? ClassDate { get; set; } // ClassDate
        public bool IsDeleted { get; set; } // IsDeleted
        public DateTime? CreateDateTime { get; set; } // CreateDateTime
        public string CreateUser { get; set; } // CreateUser (length: 50)
        public DateTime? DeletedDate { get; set; } // DeletedDate
        public string DeletedUser { get; set; } // DeletedUser (length: 50)
        public string EmployeeNumber { get; set; } // EmployeeNumber (length: 50)
        public int? WorkCalendarId { get; set; } // WorkCalendarId
    }

}

