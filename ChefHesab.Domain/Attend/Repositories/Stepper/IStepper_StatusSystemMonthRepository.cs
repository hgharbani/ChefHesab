using Ksc.HR.Domain.Entities.Stepper;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Stepper
{
    public interface IStepper_StatusSystemMonthRepository : IRepository<Stepper_StatusSystemMonth, int>
    {
        bool CheckYearMonth_SystemControlId(int yearMonth, int systemSequenceControlId, int systemSequenceStatusId);
        Stepper_StatusSystemMonth GetByYearMonth_SystemControlId(int yearMonth, int systemSequenceControlId);
    }
}
