namespace BioscoopReserveringsapplicatie
{
    static class Preferences
    {
        public static AccountsLogic prefencesLogic = new AccountsLogic();
        public static void Start()
        {
            AccountsLogic prefencesLogic = new AccountsLogic();
            Console.Clear();
            Console.WriteLine("Welcome to the preferences page.\n");
            Console.WriteLine("Please enter your preferences.\n");

            List<string> selectedGenres = SelectGenres();
            int ageCategory = SelectAgeCategory();
            string intensity = SelectIntensity();
            string language = SelectLanguage();


            Console.Clear();
            Console.WriteLine("Your selected preferences are:");
            Console.WriteLine($"Genres: {string.Join(", ", selectedGenres)}");
            Console.WriteLine($"Age Category: {ageCategory}");
            Console.WriteLine($"Intensity: {intensity}");
            Console.WriteLine($"Language: {language}");

            prefencesLogic.addPreferencesToAccount(selectedGenres, ageCategory, intensity, language);

        }

        static List<string> SelectGenres()
        {
            List<string> genres = new List<string>();
            List<string> availableGenres = new List<string> { "Horror", "Comedy", "Action", "Drama", "Thriller", "Romance", "Sci-fi", "Fantasy", "Adventure", "Animation", "Crime", "Mystery", "Family", "War", "History", "Music", "Documentary", "Western", "TV Movie" };
            Console.WriteLine("Choose up to 3 genres from the following list:");
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
                    Console.WriteLine("Invalid genre, please select from the list.");
                    i--;
                }

                if (genres.Count == 3)
                {
                    if (!prefencesLogic.ValidateGenres(genres))
                    {
                        Console.WriteLine("Invalid genres selected. Please choose again.");
                        genres.Clear();
                        i = -1;
                    }
                }
            }

            return genres;
        }

        static int SelectAgeCategory()
        {
            Console.WriteLine("Choose an age category (6, 9, 12, 14, 16, 18):");
            int ageCategory = Convert.ToInt32(Console.ReadLine());

            while (!prefencesLogic.ValidateAgeCategory(ageCategory))
            {
                Console.WriteLine("Invalid age category, please choose from the list.");
            }

            return ageCategory;
        }

        public static string SelectIntensity()
        {
            Console.WriteLine("Choose intensity (Low/Medium/High):");
            string intensity;
            do
            {
                intensity = Console.ReadLine();
                if (prefencesLogic.ValidateIntensity(intensity))
                    break;

                Console.WriteLine("Invalid choice. Please select from (Low/Medium/High)");
            } while (true);

            return intensity;
        }

        static string SelectLanguage()
        {
            Console.WriteLine("Choose language (English/Dutch):");
            string language;
            do
            {
                language = Console.ReadLine();
                if (prefencesLogic.ValidateLanguage(language))
                    break;

                Console.WriteLine("Invalid choice. Please select English or Dutch.");
            } while (true);

            return language.ToLower();
        }
    }
}