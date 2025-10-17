using KSC.Domain; 
 using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;
  namespace Ksc.Hr.Domain.Entities.Personal
  {
  public class  OfficialMessage : IEntityBase<int>
  {
public int Id { get; set; }
public DateTime StartDate { get; set; }
public DateTime EndDate { get; set; }
public string Messages { get; set; }
public string InsertUser { get; set; }
}
}

