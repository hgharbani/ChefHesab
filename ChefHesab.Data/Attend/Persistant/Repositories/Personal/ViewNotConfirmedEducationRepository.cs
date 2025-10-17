using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Personal;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class ViewNotConfirmedEducationRepository : EfRepository<ViewNotConfirmedEducation>, IViewNotConfirmedEducationRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewNotConfirmedEducationRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<ViewNotConfirmedEducation> GetViewNotConfirmedEducationAsNoTracking()
        {
            return _kscHrContext.ViewNotConfirmedEducation.AsNoTracking();
        }
    }
}
