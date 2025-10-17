using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.Personal.MaritalStatus
{
    public class SearchMaritalStatusDto
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// وضعیت تاهل
        /// </summary>
        public string Title { get; set; } // Title (length: 50)
        public bool IsActive { get; set; } // IsActive
    }
}
