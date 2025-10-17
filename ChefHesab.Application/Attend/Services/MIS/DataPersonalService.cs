using AutoMapper;
using KSC.MIS.Service;
using KSC.Common.Filters.Contracts;
using System;
using Ksc.HR.Appication.Interfaces.MIS;
using Ksc.HR.DTO.MIS;
using Ksc.HR.Share.Model;
using Ksc.HR.DTO.Personal.EmployeeEducationTime;
using System.Collections.Generic;
using Ksc.HR.DTO.Pay.EmployeeOtherPayment.PrivatePortal;
using System.Linq;
using Ksc.HR.Domain.Entities.Personal;
using Microsoft.Extensions.Configuration;

namespace Ksc.HR.Appication.Services.MIS;

public class DataPersonalForOtherSystemService : IDataPersonalForOtherSystemService
{
    //private readonly IKscHrUnitOfWork _kscHrUnitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IFilterHandler _FilterHandler;
    public DataPersonalForOtherSystemService(
        // IKscHrUnitOfWork kscHrUnitOfWork, 
        IMapper mapper,
        IFilterHandler FilterHandler,
        IConfiguration configuration)
    {
        // _kscHrUnitOfWork = kscHrUnitOfWork;
        _mapper = mapper;
        _FilterHandler = FilterHandler;
        _configuration = configuration;
    }

    public RPC_PER GetPersonalDataForOtherSystem(InputDataPersonalForOtherSystemModel model)
    {
        try
        {
            ParamApi<RPC_PER> miscall = new ParamApi<RPC_PER>(
                 Enviroment: Enviroment.Load,
                 DomainEnum: KSC.MIS.Service.Domain.KSC,
                 LibraryName: LibraryName.PER,
                 Subprogram: "S6XML028",
                 ParamName: "RPC_PER",
                 Pheader: new PHeader()
                 {

                 },
                     InputModel: new RPC_PER { NUM_PRSN = model.PersonalId, NAM_FAM = model.NAM_FAM_EMPL, OPERATION = model.OPERATION }
                 );
            var result = miscall.GetResultDevelop<RPC_PER>(_configuration["MisServiceUrl"]);
            if (result.IsSuccess == false)
            {
                //throw new Exception(string.Join(",", result.Messages));
                throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
            }

            var userDataMIS = result.Data;
            var data = result.Data.DETAIL_PER;
            return userDataMIS;//userDataMIS;
        }
        catch (Exception ex)
        {
            RPC_PER e = new RPC_PER();
            e.IsError = true;
            e.MsgError = ex.Message;
            return e;
            //throw new Exception(ex.Message);
        }

    }
    public CALLING_RPC GetPersonalDataMis(InputMisApiModel model)
    {
        try
        {

            ParamApi<CALLING_RPC> miscall = new ParamApi<CALLING_RPC>(
                 Enviroment: Enviroment.Load,
                 DomainEnum: model.domain,
                 LibraryName: LibraryName.PER,
                 Subprogram: "S6XML025",
                 ParamName: "CALLING_RPC",
                 Pheader: new PHeader()
                 {

                 },
                     InputModel: new CALLING_RPC { NUM_PRSN_EMPL = model.NUM_PRSN_EMPL, USER_FK_JPOS = model.USER_FK_JPOS, FUNCTION = model.FUNCTION }
                 );
            var result = miscall.GetResultDevelop<CALLING_RPC>(_configuration["MisServiceUrl"]);
            if (result.IsSuccess == false)
            {
                //throw new Exception(string.Join(",", result.Messages));
                throw new Exception(string.Format("({0})", string.Join(",", result.Messages)));
            }



            var userDataMIS = result.Data;
            if (result.Data != null && result.Data.DETAIL != null)
            {
                if (string.IsNullOrWhiteSpace(result.Data.DETAIL.COD_JOB_ORGL))
                {
                    result.Data.DETAIL.COD_JOB_ORGL = null;
                }
            }
            return userDataMIS;
        }
        catch (Exception ex)
        {
            CALLING_RPC e = new CALLING_RPC();
            e.IsError = true;
            e.MsgError = ex.Message;
            return e;
            //throw new Exception(ex.Message);
        }
    }

    public OutOfSalaryPaymentResultModel GetDataOutOfSalaryPaymentFromMis(OutOfSalaryPaymentFilterRequest filter)
    {
        OutOfSalaryPaymentResultModel resultList = new OutOfSalaryPaymentResultModel() { ResultList= new List<OutOfSalaryPaymentViewModel>() };
        try
        {


            ParamApi<RPC015> miscall = new ParamApi<RPC015>(
                        Enviroment: Enviroment.Load,
                        DomainEnum: filter.Domain,
                        LibraryName: LibraryName.PER,
                        Subprogram: "S6XML023",
                        ParamName: "RPC015",
       Pheader: new PHeader()
       {
       },
                            InputModel: new RPC015() { NUM_PRSN_EMPL = filter.EmployeeNumber, DAT_PYM_EPAYO = string.IsNullOrEmpty(filter.Month) ? "0" : filter.Month }
       );

            var returnData = miscall.GetResultDevelop<RPC015>(_configuration["MisServiceUrl"]);
            if (returnData.IsSuccess == false)
                throw new Exception(string.Join(",", returnData.Messages));

            resultList.ResultList = returnData.Data.DETAIL.Select(x => new OutOfSalaryPaymentViewModel()
            {
                PaymentDate = x.DAT_FINAL_PAID_EPAYO,
                PaymentAmount = x.AMN_NET_EPAYO,
                PaymentTitle = x.DES_ACN_ACNDF,
                TaxAmount = x.AMN_TAX_EPAYO,
                SendTo = x.DES_TYP_ACN
            }).ToList();
            resultList.Month = returnData.Data.DAT_PYM_EPAYO;
        }
        catch
        {


        }
        return resultList;
    }

}
