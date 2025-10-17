using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Transfer;
using Ksc.HR.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Domain.Repositories.Transfer;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Share.Model.Relocation;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.WorkFlow.Classes.Enumerations;

namespace Ksc.HR.Data.Persistant.Repositories.Transfer
{
    public partial class RelocationRepository : EfRepository<Relocation, int>, IRelocationRepository
    {

        private readonly KscHrContext _kscHrContext;

        public RelocationRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        }




        public async Task<Relocation> GetOneEdit(int id)
        {
            return await GetAllByRelatedGrid().FirstAsync(a => a.Id == id && a.IsEnd == false);
        }

        public IQueryable<Relocation> GetAllByRelatedGrid()
        {
            var result = _kscHrContext.Relocations
                .Include(x => x.RelocationType)
                .Include(x => x.Employee)

                .ThenInclude(a => a.EmploymentType)
                  .Include(x => x.EmployeeEducationDegree)
                .ThenInclude(a => a.Education)
                .Include(x => x.SourceJobPosition)
                .Include(x => x.DestinationJobPosition)
                .Include(x => x.Chart_Structure)
                .AsQueryable();
            return result;
        }

        public IQueryable<Relocation> GetAllByRelatedReport()
        {
            var result = _kscHrContext.Relocations
                .Include(x => x.RelocationType)
                .Include(x => x.Employee)
                .Include(x => x.SourceJobPosition)
                .Include(x => x.DestinationJobPosition)
                .Include(x => x.RelocationStatus)
                .Include(x => x.WF_Request).ThenInclude(x => x.WF_Status)
                .Include(x => x.WF_Request).ThenInclude(x => x.WF_Process)
                .AsQueryable();
            
            return result;
        }

        public async Task<Relocation> GetOne(int id)
        {
            return await GetAllByRelatedGrid()
                .Include(x => x.WF_Request)
                .FirstAsync(a => a.Id == id);
        }

        public Relocation GetRelocationConfirmRequest(int id)
        {
            return GetAllByRelatedGrid()
                .Include(x => x.WF_Request)

                .FirstOrDefault(i => i.WfRequestId == id);

        }
        public Relocation GetRelocationRequest(SearchRelocationDto model)
        {
            Relocation result = new Relocation();
            var query = GetAllByRelatedGrid()
                .Include(x => x.WF_Request)
                .ThenInclude(x => x.WF_Status);
            
            if (model.WfRequestId > 0)
                result =
                    query.FirstOrDefault(i => i.WfRequestId == model.WfRequestId);
            if (model.Id > 0)
                result = query
                .FirstOrDefault(i => i.Id == model.Id);
            if (model.EmployeeId > 0)
                result = query
                .FirstOrDefault(i => i.EmployeeId == model.EmployeeId);
            if (!string.IsNullOrEmpty(model.EmployeeNumber))
                result = query
                .FirstOrDefault(i => i.Employee.EmployeeNumber == model.EmployeeNumber);

            return result;
        }
        /// <summary>
        /// درخواست های جابجایی خاتمه نیافته یک فرد که در انتظار تایید واحد حقوق دستمزد باشد
        /// </summary>
        public IQueryable<Relocation> GetRelocationByEmployeeIdForInterdict(int employeeId)
        {
            var query = _kscHrContext.Relocations
                .Where(x => x.IsActive == true && x.IsEnd == false && x.EmployeeId == employeeId);

            query = query.Include(x => x.WF_Request)
            .Where(x => x.WF_Request.StatusId == EnumWorkFlowStatus.WaitingForSalary.Id) 
            // در انتظار تایید واحد حقوق دستمزد باشد = 126
            .AsQueryable();

            return query;
        }
        /// <summary>
        ///executeDate پر شود و  برای بروز رسانی وضعیت و فیلد مختومه است کاربرد دارد
        /// </summary>
        /// <param name="entityDB"></param>
        public void UpdateForInterdict(Relocation entityDB)
        {
            entityDB.RelocationStatusId =  EnumRelocationStatus.EndByInterdict.Id;
            entityDB.IsEnd = true;
            _kscHrContext.Relocations.Update(entityDB);
        }



    }
}
