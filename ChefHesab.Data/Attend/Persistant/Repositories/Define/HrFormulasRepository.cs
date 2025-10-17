using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using NCalc;

namespace Ksc.Hr.Data.Persistant.Repositories
{

    public partial class HrFormulasRepository : EfRepository<HrFormulas, int>, IHrFormulasRepository
    {
        private readonly KscHrContext _kscHrContext;
        public HrFormulasRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public double GetValueAsFormula(int CodeFormula,Dictionary<string,object> parameters)
        {
            var selectformula = _kscHrContext.HrFormulas.FirstOrDefault(a => a.Code == CodeFormula);

            var experssion = new Expression(selectformula.Experssion);
            foreach(var parameter in parameters)
            {
                experssion.Parameters[parameter.Key]= parameter.Value;
            }
            var result=experssion.Evaluate();
            return Convert.ToDouble(result);
           
        }
    }
}

