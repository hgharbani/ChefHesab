using Ksc.HR.Domain.Entities.Workshift;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Stepper
{
    public class Stepper_ProcedureAccessLevel : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public int AccessLevelId { get; set; } // AccessLevelId
        public int ProcedureId { get; set; } // ProcedureId

        // Foreign keys

        /// <summary>
        /// Parent AccessLevel pointed by [ProcedureAccessLevel].([AccessLevelId]) (FK_ProcedureAccessLevel_AccessLevel)
        /// </summary>
        public virtual AccessLevel AccessLevel { get; set; } // FK_ProcedureAccessLevel_AccessLevel

        /// <summary>
        /// Parent Stepper_Procedure pointed by [ProcedureAccessLevel].([ProcedureId]) (FK_ProcedureAccessLevel_Procedure)
        /// </summary>
        public virtual Stepper_Procedure Stepper_Procedure { get; set; } // FK_ProcedureAccessLevel_Procedure


    }
}
