namespace BioscoopReserveringsapplicatie
{
    static class AddMovie
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();
        private static Action actionWhenEscapePressed = AdminMenu.Start;

        public static void Start()
        {
            Console.Clear();
            string title = ReadLineUtil.EnterValue(() =>
            {
                ColorConsole.WriteColorLine("Film Toevoegen\n", Globals.TitleColor);
                Console.Write("Voer de film titel in: ");
            }, 
            actionWhenEscapePressed);

            string description = ReadLineUtil.EnterValue(() =>
            {
                ColorConsole.WriteColorLine("Film Toevoegen\n", Globals.TitleColor);
                Console.WriteLine($"Voer de film titel in: {title}");
                Console.Write("Voer de film beschrijving in: ");
            }, 
            actionWhenEscapePressed);

            List<Genre> genres = new List<Genre>();
            List<Genre> availableGenres = Globals.GetAllEnum<Genre>();
            bool firstTime = true;
            while (genres.Count < 3)
            {
                Genre genre;
                if (firstTime)
                {
                    genre = SelectionMenuUtil.Create(availableGenres, () =>
                    {
                        ColorConsole.WriteColorLine("Voer film genre in", Globals.TitleColor);
                        ColorConsole.WriteColorLine("U kunt maximaal 3 verschillende genres kiezen.\n", Globals.TitleColor);
                        ColorConsole.WriteColorLine("Kies een [genre]: \n", Globals.ColorInputcClarification);
                    }, actionWhenEscapePressed
                    );
                }
                else
                {
                    genre = SelectionMenuUtil.Create(availableGenres, () => ColorConsole.WriteColorLine("Kies uw favoriete [genre]: \n", Globals.ColorInputcClarification), actionWhenEscapePressed);
                }

                if (genre != default && availableGenres.Contains(genre))
                {
                    availableGenres.Remove(genre);
                    genres.Add(genre);
                }
                else
                {
                    ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                }
                firstTime = false;
            }

            List<AgeCategory> AgeGenres = Globals.GetAllEnum<AgeCategory>();
            List<string> EnumDescription = AgeGenres.Select(o => o.GetDisplayName()).ToList();
            string selectedDescription = SelectionMenuUtil.Create(EnumDescription, () => ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification),actionWhenEscapePressed);
            AgeCategory rating = AgeGenres.First(o => o.GetDisplayName() == selectedDescription);


            if (MoviesLogic.AddMovie(title, description, genres, rating))
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => {Console.Clear(); MovieOverview.Start();}),
                };
                SelectionMenuUtil.Create(options, () => Print(title, description, genres, rating), actionWhenEscapePressed);
            }
            else
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => {Console.Clear(); AdminMenu.Start();}),
                };
                SelectionMenuUtil.Create(options, () => Console.WriteLine("Er is een fout opgetreden tijdens het toevoegen van de film. Probeer het opnieuw.\n"), actionWhenEscapePressed);
            }
        }

        private static void Print(string title, string description, List<Genre> genres, AgeCategory rating)
        {
            Console.WriteLine("De film is toegevoegd.");
            Console.WriteLine("\nDe film details zijn:");
            Console.WriteLine($"Film titel: {title}");
            Console.WriteLine($"Film beschrijving: {description}");
            Console.WriteLine($"Film genre(s): {string.Join(", ", genres)}");
            Console.WriteLine($"Film kijkwijzer: {rating.GetDisplayName()}\n");
        }
    }
}