namespace BioscoopReserveringsapplicatie
{
    static class AddMovie
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start()
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[Film Toevoegen]\n", Globals.TitleColor);

            Console.WriteLine("Voer filmdetails in (druk op Esc om terug te gaan):\n");

            Console.Write("Voer de film titel in: ");
            string title = ReadLineUtil.EnterValue(() => AdminMenu.Start());

            Console.Write("Voer de film beschrijving in: ");
            string description = ReadLineUtil.EnterValue(() => AdminMenu.Start());

            List<string> genres = new List<string>();
            List<string> availableGenres = new List<string>
            {
                "Horror", "Komedie", "Actie", "Drama", "Thriller", "Romantiek", "Sci-fi",
                "Fantasie", "Avontuur", "Animatie", "Misdaad", "Mysterie", "Familie",
                "Oorlog", "Geschiedenis", "Muziek", "Documentaire", "Westers", "TV-film"
            };
            bool firstTime = true;
            while (genres.Count < 3)
            {
                string genre = "";
                if (firstTime)
                {
                    genre = SelectionMenu.Create(availableGenres, () =>
                    {
                        ColorConsole.WriteColorLine("[Voer film genre in]", Globals.TitleColor);
                        ColorConsole.WriteColorLine("[U kunt maximaal 3 verschillende genres kiezen.]\n", Globals.TitleColor);
                        ColorConsole.WriteColorLine("Kies een [genre]: \n", Globals.ColorInputcClarification);
                    }
                    );
                }
                else
                {
                    genre = SelectionMenu.Create(availableGenres, () => ColorConsole.WriteColorLine("Kies uw favoriete [genre]: \n", Globals.ColorInputcClarification));
                }

                if (!string.IsNullOrWhiteSpace(genre) && availableGenres.Contains(genre))
                {
                    availableGenres.Remove(genre);
                    genres.Add(genre);
                }
                else
                {
                    Console.WriteLine("Error. Probeer het opnieuw.");
                }
                firstTime = false;
            }

            Console.Write("Voer de film kijkwijzer in: ");
            string rating = ReadLineUtil.EnterValue(() => AdminMenu.Start());

            if (MoviesLogic.AddMovie(title, description, genres, rating))
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => {Console.Clear(); MovieOverview.Start();}),
                };
                SelectionMenu.Create(options, () => Print(title, description, genres, rating));
            }
            else
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => {Console.Clear(); AdminMenu.Start();}),
                };
                SelectionMenu.Create(options, () => Console.WriteLine("Er is een fout opgetreden tijdens het toevoegen van de film. Probeer het opnieuw.\n"));
            }
        }

        private static void Print(string title, string description, List<string> genres, string rating)
        {
            Console.WriteLine("De film is toegevoegd.");
            Console.WriteLine("\nDe film details zijn:");
            Console.WriteLine($"Film titel: {title}");
            Console.WriteLine($"Film beschrijving: {description}");
            Console.WriteLine($"Film genre(s): {string.Join(", ", genres)}");
            Console.WriteLine($"Film kijkwijzer: {rating}\n");
        }
    }
}