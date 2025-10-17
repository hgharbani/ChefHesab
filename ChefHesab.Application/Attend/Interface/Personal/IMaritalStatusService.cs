using KSC.Common.Filters.Models;
using KSC.Common;
using Ksc.Hr.Domain.Entities;
using Ksc.HR.DTO.EmployeeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.Personal.MaritalStatus;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IMaritalStatusService
    {
        Task<KscResult> AddMaritalStatus(AddMaritalStatusDto model);
        Task<bool> ExistsByTitle(int id, string title);
        Task<bool> ExistsByTitle(string title);
        Task<EditMaritalStatusDto> GetForEdit(int id);
        Task<MaritalStatus> GetOne(int id);
        List<ListMaritalStatusDto> GetMaritalStatuss();
        FilterResult<ListMaritalStatusDto> GetMaritalStatusByFilter(FilterRequest Filter);
        List<SearchMaritalStatusDto> GetMaritalStatusByKendoFilter(FilterRequest Filter);
        Task<KscResult> RemoveMaritalStatus(EditMaritalStatusDto model);
        Task<KscResult> UpdateMaritalStatus(EditMaritalStatusDto model);
    }
}
