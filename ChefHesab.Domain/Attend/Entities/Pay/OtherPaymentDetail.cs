using KSC.Domain; 
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;
  namespace Ksc.HR.Domain.Entities.Pay
{
  public class  OtherPaymentDetail : IEntityBase<int>
  {
public int Id { get; set; }
public int OtherPaymentHeaderId { get; set; }
public int OtherPaymentStatusId { get; set; }
public string InsertUser { get; set; }
public DateTime? InsertDate { get; set; }
public string RemoteIpAddress { get; set; }
public string AuthenticateUserName { get; set; }
 public virtual OtherPaymentHeader OtherPaymentHeader  { get; set; }
 public virtual OtherPaymentStatus OtherPaymentStatus  { get; set; }
}
}

