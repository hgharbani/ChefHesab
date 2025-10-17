using AutoMapper;

using Ksc.HR.Domain.Entities.Workshift;

using Ksc.HR.DTO.WorkShift;


namespace Ksc.HR.Appication.Profiles.WorkShift

{
    public class PaymentStatusProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public PaymentStatusProfile()
        {
            CreateMap<SearchPaymentStatusModel, PaymentStatus>().ReverseMap();
            CreateMap<AddPaymentStatusModel, PaymentStatus>().ReverseMap();
            CreateMap<EditPaymentStatusModel, PaymentStatus>().ReverseMap();

        }
    }
}
