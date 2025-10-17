using KSC.Domain;
using Ksc.HR.Domain.Entities.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.Relocation;

namespace Ksc.HR.Domain.Repositories.Transfer
{
    public interface IRelocationRepository : IRepository<Relocation, int>
    {
        IQueryable<Relocation> GetAllByRelatedGrid();
        Task<Relocation> GetOne(int id);
        Task<Relocation> GetOneEdit(int id);
        Relocation GetRelocationConfirmRequest(int id);
        Relocation GetRelocationRequest(SearchRelocationDto model);
        /// <summary>
        /// درخواست های جابجایی خاتمه نیافته یک فرد که در انتظار تایید واحد حقوق دستمزد باشد
        /// </summary>
        IQueryable<Relocation> GetRelocationByEmployeeIdForInterdict(int employeeId);
        // <summary>
        ///executeDate پر شود و  برای بروز رسانی وضعیت و فیلد مختومه است کاربرد دارد
        /// </summary>
        void UpdateForInterdict(Relocation entityDB);
        IQueryable<Relocation> GetAllByRelatedReport();
    }
}
