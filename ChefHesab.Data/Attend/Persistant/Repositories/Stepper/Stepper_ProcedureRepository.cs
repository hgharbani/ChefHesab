using KSC.Infrastructure.Persistance;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Entities.Stepper;
using Ksc.HR.Data.Persistant.Context;
using KSC.Common;
using Ksc.HR.Data.Persistant.Repositories;

namespace Ksc.HR.Domain.Repositories.Stepper
{
    public partial class Stepper_ProcedureRepository : EfRepository<Stepper_Procedure, int>, IStepper_ProcedureRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Stepper_ProcedureRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<Stepper_Procedure> GetProceduresByParentID(int id)
        {
            return _kscHrContext.Stepper_Procedures.Where(x => x.ParentId == id && x.IsActive).OrderBy(x=>x.RunSequence).AsQueryable();
        }
        public async Task<Stepper_Procedure> GetOne(int id)
        {
            return await _kscHrContext.Stepper_Procedures.FindAsync(id);
        }

        public Tuple<int,string> CheckEventIsFinished(int? StepId, int? YearMonth)
        {               
     
        
            var GetEventListiners = _kscHrContext.EventListener.Where(a =>  a.YearMonth == YearMonth).ToList();
            if (StepId.HasValue)
            {
                GetEventListiners= GetEventListiners.Where(a=>a.StepId== StepId).ToList();
            }
            
            var GetEventListiner = GetEventListiners.FirstOrDefault();
            if (GetEventListiner == null)
            {           
                return new Tuple<int, string>(2, "");
            }

            if(GetEventListiner.IsFinished.Value == false && GetEventListiner.IsError.Value == false)
            {
                return new Tuple<int, string>(1, "متد شروع شده است");
            }
            else if (GetEventListiner.IsFinished.Value == true && GetEventListiner.IsError.Value == true)
            {
                return new Tuple<int, string>(3, "عملیات با خطا مواجه شده است، لطفا با تیم پرسنلی تماس بگیرید");

            }
            else 
            {
                return new Tuple<int, string>(4, "عملیات با موفقیت انجام شده است");

            }
         
        }
        public Tuple<int, string> CheckEventWithTitleIsFinished(int? YearMonth, string title)
        {


            var GetEventListiners = _kscHrContext.EventListener.Where(a => a.YearMonth == YearMonth).ToList();
          
            if (!string.IsNullOrEmpty(title))
            {
                GetEventListiners = GetEventListiners.Where(a => a.Title == title).ToList();
            }
            var GetEventListiner = GetEventListiners.FirstOrDefault();
            if (GetEventListiner == null)
            {
                return new Tuple<int, string>(2, "");
            }

            if (GetEventListiner.IsFinished.Value == false && GetEventListiner.IsError.Value == false)
            {
                return new Tuple<int, string>(1, "متد شروع شده است");
            }
            else if (GetEventListiner.IsFinished.Value == true && GetEventListiner.IsError.Value == true)
            {
                return new Tuple<int, string>(3, "عملیات با خطا مواجه شده است، لطفا با تیم پرسنلی تماس بگیرید");

            }
            else
            {
                return new Tuple<int, string>(4, "عملیات با موفقیت انجام شده است");

            }

        }
    }
}

