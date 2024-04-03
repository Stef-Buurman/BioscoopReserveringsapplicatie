namespace BioscoopReserveringsapplicatie
{
    static class UserDetailsEdit
    {
        public static void Start()
        {
            if(UserLogic.CurrentUser != null)
            {
                Console.Clear();

                Console.WriteLine("Voer nieuwe profielgegevens in (druk op Enter om de huidige te behouden):");

                Console.Write("Voer uw naam in: ");
                string newName = EditDefaultValueUtil.EditDefaultValue(UserLogic.CurrentUser.FullName);

                Console.Write("Voer uw emailadres in: ");
                string newEmail = EditDefaultValueUtil.EditDefaultValue(UserLogic.CurrentUser.EmailAddress);

                List<Genre> newGenres = new List<Genre>();
                List<Genre> availableGenres = Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();

                while (newGenres.Count < 3)
                {
                    Genre newGenre = SelectionMenu.Create(availableGenres, () => ColorConsole.WriteColorLine("Kies een [genre]: \n", Globals.ColorInputcClarification));

                    if(availableGenres.Contains(newGenre))
                    {
                        availableGenres.Remove(newGenre);
                        newGenres.Add(newGenre);
                    }
                }

                List<Intensity> intensities = Enum.GetValues(typeof(Intensity)).Cast<Intensity>().ToList();
                Intensity newIntensity = SelectionMenu.Create(intensities, () => ColorConsole.WriteColorLine("Kies een [Intensiteit]: \n", Globals.ColorInputcClarification));

                List<AgeCategory> ageCategories = Enum.GetValues(typeof(AgeCategory)).Cast<AgeCategory>().ToList();
                AgeCategory newAgeCategory = SelectionMenu.Create(ageCategories, () => ColorConsole.WriteColorLine("Kies een [Kijkwijzer]: \n", Globals.ColorInputcClarification));

            }
        }
    }
}