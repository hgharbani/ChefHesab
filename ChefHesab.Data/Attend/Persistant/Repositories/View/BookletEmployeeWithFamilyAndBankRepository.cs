using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;

namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class BookletEmployeeWithFamilyAndBankRepository : EfRepository<BookletEmployeeWithFamilyAndBank>, IBookletEmployeeWithFamilyAndBankRepository
    {
        public BookletEmployeeWithFamilyAndBankRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
        }
    }
}

