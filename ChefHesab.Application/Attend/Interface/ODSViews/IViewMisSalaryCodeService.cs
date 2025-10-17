using KSC.Common;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.DTO.ODSViews.ViewMisSalaryCode;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.ODSViews
{
    public interface IViewMisSalaryCodeService
    {
        FilterResult<SearchViewMisSalaryCodeModel> GetViewMisSalaryCodeByKendoFilter(FilterRequest Filter);
        List<SearchViewMisSalaryCodeModel> GetAllViewMisSalaryCodeByKendoFilter(FilterRequest Filter);

    }
}
