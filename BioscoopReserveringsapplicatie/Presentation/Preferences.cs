static class Preferences
{
    public static AccountsLogic prefencesLogic = new AccountsLogic();
    public static void Start()
    {
        AccountsLogic prefencesLogic = new AccountsLogic();
        Console.Clear();
        Console.WriteLine("Welkom op de voorkeuren pagina.\n");
        Console.WriteLine("Voer alstublieft uw voorkeuren in.\n");

        List<string> selectedGenres = SelectGenres();
        int ageCategory = SelectAgeCategory();
        string intensity = SelectIntensity();
        string language = SelectLanguage();

       
        Console.Clear();
        Console.WriteLine("Uw geselecteerde voorkeuren zijn:");
        Console.WriteLine($"Genres: {string.Join(", ", selectedGenres)}");
        Console.WriteLine($"Leeftijdscategorie: {ageCategory}");
        Console.WriteLine($"Intensiteit: {intensity}");
        Console.WriteLine($"Taal: {language}");

        prefencesLogic.addPreferencesToAccount(selectedGenres, ageCategory, intensity, language);

    }

    static List<string> SelectGenres()
    {
        List<string> genres = new List<string>();
        List<string> availableGenres = new List<string>{ "Horror", "Komedie", "Actie", "Drama", "Thriller", "Romantiek", "Science Fiction", "Fantasie", "Avontuur", "Animatie", "Misdaad", "Mysterie", "Familie", "Oorlog", "Geschiedenis", "Muziek", "Documentaire", "Western", "Televisiefilm" };
        Console.WriteLine("Kies maximaal 3 genres uit de volgende lijst:");
        Console.WriteLine(string.Join(", ", availableGenres) + "\n");

        for (int i = 0; i < 3; i++)
        {
            string genre = Console.ReadLine() ?? "";
            if (availableGenres.Contains(genre))
            {
                genres.Add(genre);
            }
            else
            {
                Console.WriteLine("Ongeldig genre, kies alstublieft uit de lijst.");
                i--;
            }

            if (genres.Count == 3)
            {
                if (!prefencesLogic.ValidateGenres(genres))
                {
                    Console.WriteLine("Ongeldige genres geselecteerd. Kies alstublieft opnieuw.");
                    genres.Clear();
                    i = -1;
                }
            }
        }

        return genres;
    }

    static int SelectAgeCategory()
    {
        Console.WriteLine("Kies een leeftijdscategorie (6, 9, 12, 14, 16, 18):");
        int ageCategory = Convert.ToInt32(Console.ReadLine());

        while (!prefencesLogic.ValidateAgeCategory(ageCategory))
        {
            Console.WriteLine("Ongeldige leeftijdscategorie, kies alstublieft uit de lijst.");
        }

        return ageCategory;
    }

    public static string SelectIntensity()
    {
        Console.WriteLine("Kies intensiteit (Laag/Medium/Hoog):");
        string intensity;
        do
        {
            intensity = Console.ReadLine();
            if (prefencesLogic.ValidateIntensity(intensity))
                break;

            Console.WriteLine("Ongeldige keuze. Selecteer alstublieft uit (Laag/Medium/Hoog).");
        } while (true);

        return intensity;
    }

    static string SelectLanguage()
    {
        Console.WriteLine("Kies een taal (English/Nederlands):");
        string language;
        do
        {
            language = Console.ReadLine();
            if (prefencesLogic.ValidateLanguage(language))
                break;

            Console.WriteLine("Ongeldige keuze. Selecteer alstublieft Engels of Nederlands.");
        } while (true);

        return language.ToLower();
    }
}
