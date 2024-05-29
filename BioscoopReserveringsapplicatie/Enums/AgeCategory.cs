using System.ComponentModel.DataAnnotations;

namespace BioscoopReserveringsapplicatie
{
    public enum AgeCategory
    {
        [Display(Name = "Niet ingevuld")]
        Undefined,
        [Display(Name = "6")]
        AGE_6 = 6,
        [Display(Name = "9")]
        AGE_9 = 9,
        [Display(Name = "12")]
        AGE_12 = 12,
        [Display(Name = "14")]
        AGE_14 = 14,
        [Display(Name = "16")]
        AGE_16 = 16,
        [Display(Name = "18")]
        AGE_18 = 18,
        [Display(Name = "Alle leeftijden")]
        All,
    }
}