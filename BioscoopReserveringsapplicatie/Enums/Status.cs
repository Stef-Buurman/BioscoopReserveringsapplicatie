using System.ComponentModel.DataAnnotations;

namespace BioscoopReserveringsapplicatie
{
    public enum Status
    {
        [Display(Name = "Onbekend")]
        Undefined,
        [Display(Name = "Inactief")]
        Inactive,
        [Display(Name = "Actief")]
        Active,
        [Display(Name = "Gearchiveerd")]
        Archived
    }
}