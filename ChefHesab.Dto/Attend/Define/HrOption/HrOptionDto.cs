  using System;
  using System.ComponentModel;
  using System.ComponentModel.DataAnnotations;
namespace Ksc.Hr.DTO.HrOption
{
   public class HrOptionDto
  {
  public int Id { get; set; }

  public string Title { get; set; }
  public int? OptionTypeId { get; set; }
  public string ValueString { get; set; }

  public DateTime? ValueTime { get; set; }

  public double? ValueFloat { get; set; }

  public System.Byte? Code { get; set; }

  }
  }

