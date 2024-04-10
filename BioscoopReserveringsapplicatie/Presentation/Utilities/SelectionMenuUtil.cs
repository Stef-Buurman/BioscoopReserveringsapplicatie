namespace BioscoopReserveringsapplicatie
{
    static class SelectionMenuUtil
    {
        private static bool moreOptionsThanVisibility = false;
        public static T Create<T>(List<Option<T>> options, int maxVisibility, Action ActionBeforeMenu = null, bool canBeEscaped = false, Action escapeAction = null)
        {
            if (options.Count == 0) return default;
            moreOptionsThanVisibility = options.Count > maxVisibility;
            Console.CursorVisible = false;
            int index = 0;
            int visibleIndex = 0;

            int amountOptionsAbove = 0;
            List<Option<T>> optionsToShow = GetOptionsToShow(options, maxVisibility);

            int halfOfMaxVisibility = Convert.ToInt32(Math.Round((double)maxVisibility / 2, MidpointRounding.AwayFromZero));

            WriteMenu(optionsToShow, optionsToShow[index], ActionBeforeMenu, canBeEscaped);

            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey(true);
                // When the user presses the down arrow, the selected option will move down
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    // When the option where the user wants to go to is lower than the amount of options, so the highest index possible, this will be executed.
                    if (index + 1 < options.Count)
                    {
                        //Set the index and visible index one higher
                        index++;
                        visibleIndex++;
                        // This will be approved when the amount of options is higher than the max visibility.
                        if (options.Count > maxVisibility)
                        {
                            //When the visible index is higher than the max visibility, this will be executed.
                            if (visibleIndex > maxVisibility)
                            {
                                visibleIndex = maxVisibility;
                            }
                            // When the selected option is in the second half of or equal to the visible options, this will be executed.
                            if (index >= options.Count - halfOfMaxVisibility)
                            {
                                amountOptionsAbove = options.Count - maxVisibility;
                                optionsToShow = GetOptionsToShow(options, maxVisibility, amountOptionsAbove, true);
                                WriteMenu(optionsToShow, optionsToShow[visibleIndex - 1], ActionBeforeMenu, canBeEscaped);
                            }
                            // When the selected option is higher than the half of the max visibility, this will be executed.
                            else if (visibleIndex > Math.Floor((double)maxVisibility / 2))
                            {
                                amountOptionsAbove++;
                                optionsToShow = GetOptionsToShow(options, maxVisibility, amountOptionsAbove, true);
                                visibleIndex--;
                                WriteMenu(optionsToShow, optionsToShow[visibleIndex], ActionBeforeMenu, canBeEscaped);
                            }
                            // When the selected option is neither of the above, this will be executed.
                            else
                            {
                                optionsToShow = GetOptionsToShow(options, maxVisibility);
                                WriteMenu(optionsToShow, optionsToShow[index], ActionBeforeMenu, canBeEscaped);
                            }
                        }
                        else
                        {
                            WriteMenu(options, options[index], ActionBeforeMenu, canBeEscaped);
                        }
                    }
                }
                // When the user presses the up arrow, this will be executed.
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    //When the option where the user wants to go to is higher than 0, so inside the possible options, this will be executed.
                    if (index - 1 >= 0)
                    {
                        //Set the index and visible one lower
                        index--;
                        visibleIndex--;
                        // This will be approved when the amount of options is higher than the max visibility.
                        if (options.Count > maxVisibility)
                        {
                            // When the selected option is in the first half of or equal to the visible options, this will be executed.
                            if (index + 1 <= halfOfMaxVisibility)
                            {
                                visibleIndex = index;
                                amountOptionsAbove = 0;
                                optionsToShow = GetOptionsToShow(options, maxVisibility);
                                WriteMenu(optionsToShow, optionsToShow[index], ActionBeforeMenu, canBeEscaped);
                            }
                            // When the selected option is higher than (the option count minus the half of the max visibility), this will be executed.
                            else if (index >= options.Count - halfOfMaxVisibility)
                            {
                                amountOptionsAbove = options.Count - maxVisibility;
                                optionsToShow = GetOptionsToShow(options, maxVisibility, amountOptionsAbove, true);
                                WriteMenu(optionsToShow, optionsToShow[visibleIndex - 1], ActionBeforeMenu, canBeEscaped);
                            }
                            // When the selected option is neither of the above, this will be executed.
                            else
                            {
                                if (amountOptionsAbove > 0) amountOptionsAbove--;
                                optionsToShow = GetOptionsToShow(options, maxVisibility, amountOptionsAbove, true);
                                if (index + 1 < options.Count - halfOfMaxVisibility) visibleIndex++;
                                WriteMenu(optionsToShow, optionsToShow[visibleIndex], ActionBeforeMenu, canBeEscaped);
                            }
                        }
                        else
                        {
                            WriteMenu(options, options[index], ActionBeforeMenu, canBeEscaped);
                        }
                    }
                }
                // When the user presses the enter key, the selected option will be executed
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    options[index].Select();
                    return options[index].Value;
                }

                if (keyinfo.Key == ConsoleKey.Escape && canBeEscaped)
                {
                    ReadLineUtil.EscapeKeyPressed(ActionBeforeMenu, escapeAction, () => WriteMenu(GetOptionsToShow<T>(options, maxVisibility, amountOptionsAbove, (amountOptionsAbove > 0)), options[index], ActionBeforeMenu));
                }
            }
            while (keyinfo.Key != ConsoleKey.X);

            Console.Clear();
            Console.CursorVisible = true;

            return default;
        }

        public static T Create<T>(List<T> options, int maxVisibility, Action ActionBeforeMenu = null, bool canBeEscaped = false, Action escapeAction = null)
        {
            // When you give a list of T, it will be converted to a list of Options.
            List<Option<T>> optionList = new List<Option<T>>();
            foreach (T option in options)
            {
                optionList.Add(new Option<T>(option));
            }
            return Create(optionList, maxVisibility, ActionBeforeMenu, canBeEscaped, escapeAction);
        }

        public static T Create<T>(List<Option<T>> options, Action ActionBeforeMenu = null, bool canBeEscaped = false, Action escapeAction = null)
        {
            return Create(options, 9, ActionBeforeMenu, canBeEscaped, escapeAction);
        }

        public static T Create<T>(List<Option<T>> options, Action ActionBeforeMenu, Action escapeAction)
        {
            return Create(options, 9, ActionBeforeMenu, true, escapeAction);
        }

        public static T Create<T>(List<Option<T>> options, int maxVisibility, Action ActionBeforeMenu, Action escapeAction)
        {
            return Create(options, maxVisibility, ActionBeforeMenu, true, escapeAction);
        }

        public static T Create<T>(List<T> options, Action ActionBeforeMenu = null, bool canBeEscaped = false, Action escapeAction = null)
        {
            return Create(options, 9, ActionBeforeMenu, canBeEscaped, escapeAction);
        }

        public static T Create<T>(List<T> options, Action ActionBeforeMenu, Action escapeAction)
        {
            return Create(options, 9, ActionBeforeMenu, true, escapeAction);
        }

        public static T Create<T>(List<T> options, int maxVisibility, Action ActionBeforeMenu, Action escapeAction)
        {
            return Create(options, maxVisibility, ActionBeforeMenu, true, escapeAction);
        }

        static void WriteMenu<T>(List<Option<T>> options, Option<T> selectedOption, Action ActionBeforeMenu = null, bool canBeEscaped = false, string textToShowEscapability = "*Klik op escape om dit onderdeel te verlaten.*")
        {
            Console.Clear();
            if(canBeEscaped) ColorConsole.WriteLineInfo(textToShowEscapability);
            // When you give a function to the menu, it will execute it before the menu is printed
            if (ActionBeforeMenu != null) ActionBeforeMenu();
            


            foreach (Option<T> option in options)
            {
                if (option == selectedOption)
                {
                    // This will print the selected option in blue
                    ColorConsole.WriteColorLine($"[>> {option.Name} <<]", ConsoleColor.Blue);
                }
                else
                {
                    Console.WriteLine($"  {option.Name}");
                }
            }
            if (moreOptionsThanVisibility) ColorConsole.WriteLineInfo($"\n*Navigeer met de pijltjestoetsen om meer opties te zien.*");
        }

        private static List<Option<T>> GetOptionsToShow<T>(List<Option<T>> options, int maxVisibility, int skipOptions = 0, bool hasSkipOptions = false)
        {
            List<Option<T>> optionsToShow = new List<Option<T>>();
            //Loops trough all options.
            for (int i = 1; i <= options.Count; i++)
            {
                // Checks if there are options which must be skipped.
                if (hasSkipOptions)
                {
                    // Checks wheter the index of the current option is higher of the index which must be skipped. If so, the option will be added to the list.
                    if (i > skipOptions) optionsToShow.Add(options[i - 1]);
                }
                else
                {
                    // Adds the option to the list.
                    optionsToShow.Add(options[i - 1]);
                }
                // WHen the amount of options to show is equal to the max visibility, the loop will be broken.
                if (optionsToShow.Count == maxVisibility) break;
            }
            return optionsToShow;
        }
    }
}