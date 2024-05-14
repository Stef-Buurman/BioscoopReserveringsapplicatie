using System.ComponentModel.DataAnnotations;

namespace BioscoopReserveringsapplicatie
{
    public enum Language
    {
        [Display(Name = "Niet ingevuld")]
        Undefined,
        Nederlands,
        English
    }
}