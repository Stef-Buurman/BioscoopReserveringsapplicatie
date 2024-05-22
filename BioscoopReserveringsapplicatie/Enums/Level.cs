using System.ComponentModel.DataAnnotations;

namespace BioscoopReserveringsapplicatie
{
    public enum Level
    {
        [Display(Name = "Niet ingevuld")]
        Undefined,
        [Display(Name = "Gebruiker")]
        User,
        [Display(Name = "Redacteur")]
        Editor,
        [Display(Name = "Beheerder")]
        Admin
    }
}