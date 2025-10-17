using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Domain.Repositories.Personal

{
    public interface IViewNotConfirmedEducationRepository : IRepository<ViewNotConfirmedEducation>
    {
        IQueryable<ViewNotConfirmedEducation> GetViewNotConfirmedEducationAsNoTracking();
    }
}
