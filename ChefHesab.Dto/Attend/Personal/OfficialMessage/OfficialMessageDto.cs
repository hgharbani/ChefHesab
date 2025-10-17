  using System;
  using System.ComponentModel;
  using System.ComponentModel.DataAnnotations;
    using Ksc.HR.Resources.Personal;
namespace Ksc.Hr.DTO.Personal.OfficialMessage
{
   public class OfficialMessageDto
  {
  [Display(Name=nameof(Id),ResourceType = typeof(OfficialMessageResource))]
  public int Id { get; set; }

  [Display(Name=nameof(StartDate),ResourceType = typeof(OfficialMessageResource))]
  public DateTime StartDate { get; set; }

  [Display(Name=nameof(EndDate),ResourceType = typeof(OfficialMessageResource))]
  public DateTime EndDate { get; set; }

  [Display(Name=nameof(Messages),ResourceType = typeof(OfficialMessageResource))]
  public string Messages { get; set; }

  [Display(Name=nameof(InsertUser),ResourceType = typeof(OfficialMessageResource))]
  public string InsertUser { get; set; }

  }
  }

