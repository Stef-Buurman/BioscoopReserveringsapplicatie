using System.ComponentModel.DataAnnotations;

namespace BioscoopReserveringsapplicatie
{
    public enum Intensity
    {
        [Display(Name = "Niet ingevuld")]
        Undefined,
        [Display(Name = "Laag")]
        Low,
        [Display(Name = "Medium")]
        Medium,
        [Display(Name = "Hoog")]
        High,
        [Display(Name = "Alle intensiteiten")]
        All
    }
}