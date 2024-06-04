using System.ComponentModel.DataAnnotations;

namespace BioscoopReserveringsapplicatie
{
    public enum RoomType
    {
        [Display(Name = "Onbekend")]
        Undefined,
        [Display(Name = "Vierkant")]
        Square,
        [Display(Name = "Rond")]
        Round
    }
}