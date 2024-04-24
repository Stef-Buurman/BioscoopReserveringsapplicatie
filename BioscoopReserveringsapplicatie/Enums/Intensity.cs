using System.ComponentModel.DataAnnotations;


public enum Intensity
{
    [Display(Name = "Onbekend")]
    Undefined,
    [Display(Name = "Laag")]
    Low,
    [Display(Name = "Medium")]
    Medium,
    [Display(Name = "Hoog")]
    High
}