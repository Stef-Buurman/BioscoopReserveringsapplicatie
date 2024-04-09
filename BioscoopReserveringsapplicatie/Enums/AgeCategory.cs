using System.ComponentModel.DataAnnotations;
public enum AgeCategory
{
    [Display(Name = "Alle leeftijden")]
    ALL = -1,
    [Display(Name = "Onbekend")]
    Undefined = 0,
    [Display(Name = "PG 6")]
    AGE_6 = 6,
    [Display(Name = "PG 9")]
    AGE_9 = 9,
    [Display(Name = "PG 12")]
    AGE_12 = 12,
    [Display(Name = "PG 14")]
    AGE_14 = 14,
    [Display(Name = "PG 16")]
    AGE_16 = 16,
    [Display(Name = "PG 18")]
    AGE_18 = 18
}