using System.ComponentModel.DataAnnotations;

namespace BioscoopReserveringsapplicatie
{
    public enum Genre
    {
        [Display(Name = "Niet ingevuld")]
        Undefined,
        Horror,
        Comedy,
        Action,
        Drama,
        Thriller,
        Romance,
        ScienceFiction,
        Fantasy,
        Adventure,
        Animation,
        Crime,
        Mystery,
        Family,
        War,
        History,
        Music,
        Documentary,
        Western,
        TVMovie
    }
}