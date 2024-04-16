namespace BioscoopReserveringsapplicatie
{
    public class SelectionMenuUtil2<T>
    {
        private static int MaxSelectionMenu = 0;

        private int Index = 0;
        private int VisibleIndex = 0;

        private bool moreOptionsThanVisibility = false;
        private int AmountOptionsAbove = 0;
        private bool HasOptionsAbove { get => AmountOptionsAbove > 0 && moreOptionsThanVisibility; }
        private bool HasOptionsBelow { get => AllOptions.Count - MaxVisibility != AmountOptionsAbove && moreOptionsThanVisibility; }
        private int HalfOfMaxVisibility = 0;
        private int MaxVisibility = 0;

        private List<Option<T>> AllOptions;
        private List<Option<T>> OptionsToShow = new List<Option<T>>();

        private int Top = 0;

        private bool CanBeEscaped;
        private Action? EscapeAction;
        private Action? EscapeActionWhenNotEscaping;
        private bool EscapabilityVisible = false;

        private string TextBeforeInputShown = "";
        private bool TextBeforeInputShownVisible = false;

        private bool VisibleSelectedArrows;
        public SelectionMenuUtil2(List<Option<T>> options, int maxVisibility = 9, bool canBeEscaped = false, Action escapeAction = null, Action escapeActionWhenNotEscaping = null, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
        {
            MaxVisibility = maxVisibility;
            AllOptions = options;
            OptionsToShow = GetOptionsToShow(AllOptions, MaxVisibility);
            moreOptionsThanVisibility = AllOptions.Count > maxVisibility;
            Index = 0;
            VisibleIndex = 0;
            CanBeEscaped = canBeEscaped;
            EscapeAction = escapeAction;
            EscapeActionWhenNotEscaping = escapeActionWhenNotEscaping;
            HalfOfMaxVisibility = Convert.ToInt32(Math.Round((double)MaxVisibility / 2, MidpointRounding.AwayFromZero));
            VisibleSelectedArrows = visibleSelectedArrows;
            if (textBeforeInputShown != default)
            {
                TextBeforeInputShown = textBeforeInputShown;
                TextBeforeInputShownVisible = true;
            }
        }
            if (textBeforeInputShown != default)
            {
                TextBeforeInputShown = textBeforeInputShown;
                TextBeforeInputShownVisible = true;
        }
        }

        public SelectionMenuUtil2(List<T> options, int maxVisibility, bool canBeEscaped = false, Action escapeAction = null, Action escapeActionWhenNotEscaping = null, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
        {
            // When you give a list of T, it will be converted to a list of Options.
            List<Option<T>> optionList = ConvertToOption(options);
            MaxVisibility = maxVisibility;
            AllOptions = optionList;
            OptionsToShow = GetOptionsToShow(AllOptions, MaxVisibility);
            moreOptionsThanVisibility = AllOptions.Count > maxVisibility;
            Index = 0;
            VisibleIndex = 0;
            CanBeEscaped = canBeEscaped;
            EscapeAction = escapeAction;
            EscapeActionWhenNotEscaping = escapeActionWhenNotEscaping;
            HalfOfMaxVisibility = Convert.ToInt32(Math.Round((double)MaxVisibility / 2, MidpointRounding.AwayFromZero));
            VisibleSelectedArrows = visibleSelectedArrows;
            if (textBeforeInputShown != default)
            {
                TextBeforeInputShown = textBeforeInputShown;
                TextBeforeInputShownVisible = true;
        }
        }

        public SelectionMenuUtil2(List<Option<T>> options, bool canBeEscaped = false, Action escapeAction = null, Action escapeActionWhenNotEscaping = null, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, 9, canBeEscaped, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil2(List<Option<T>> options, Action escapeAction, Action escapeActionWhenNotEscaping, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, 9, true, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil2(List<Option<T>> options, int maxVisibility, Action escapeAction, Action escapeActionWhenNotEscaping, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, maxVisibility, true, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil2(List<Option<T>> options)
            : this(options, 9, false) { }

        public SelectionMenuUtil2(List<Option<T>> options, bool canBeEscaped = false)
            : this(options, 9, canBeEscaped) { }

        public SelectionMenuUtil2(List<Option<T>> options, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, 9, false, null, null, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil2(List<T> options, bool canBeEscaped = false, Action escapeAction = null, Action escapeActionWhenNotEscaping = null, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, 9, canBeEscaped, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil2(List<T> options, Action escapeAction, Action escapeActionWhenNotEscaping, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, 9, true, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil2(List<T> options, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, 9, false, null, null, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil2(List<T> options)
            : this(options, 9, false) { }

        public SelectionMenuUtil2(List<T> options, bool canBeEscaped = false)
            : this(options, 9, canBeEscaped) { }

        public SelectionMenuUtil2(List<T> options, int maxVisibility, Action escapeAction, Action escapeActionWhenNotEscaping, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, maxVisibility, true, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown) { }


        public T Create()
        {
            Top = Console.GetCursorPosition().Top;
            if (AllOptions.Count == 0) return default;
            if (CanBeEscaped && EscapeAction == null) return default;
            Console.CursorVisible = false;

            WriteMenu(OptionsToShow, OptionsToShow[Index]);
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey(true);
                // When the user presses the down arrow, the selected option will move down
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    KeyDown();
                }
                // When the user presses the up arrow, this will be executed.
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    KeyUp();
                }
                // When the user presses the enter key, the selected option will be executed
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    AllOptions[Index].Select();
                    return AllOptions[Index].Value;
                }

                if (keyinfo.Key == ConsoleKey.Escape && CanBeEscaped && EscapeAction != null)
                {
                    //() => WriteMenu(GetOptionsToShow(Options, MaxVisibility, AmountOptionsAbove, (AmountOptionsAbove > 0))
                    ReadLineUtil.EscapeKeyPressed(() => { }, EscapeAction, EscapeActionWhenNotEscaping);
                }
            }
            while (keyinfo.Key != ConsoleKey.X);
            Console.CursorVisible = true;
            return default;
        }

        public void WriteMenu<T>(List<Option<T>> Options, Option<T> selectedOption, string textToShowEscapability = "*Klik op escape om dit onderdeel te verlaten*\n")
        {
            if (CanBeEscaped && !EscapabilityVisible)
            {
                ColorConsole.WriteLineInfo(textToShowEscapability + "\n");
                Console.SetCursorPosition(0, Top + 2);
                EscapabilityVisible = true;
            }
            else if (EscapabilityVisible)
            {
                Console.SetCursorPosition(0, Top + 2);
            }
            else
            {
                Console.SetCursorPosition(0, Top);
            }

            int maxOptionsLength = Options.Max(x => x.Name.Length);
            if (MaxSelectionMenu < maxOptionsLength) MaxSelectionMenu = maxOptionsLength;

            if (HasOptionsAbove)
            {
                string strintToPrintForArrowUp = "";
                if (VisibleSelectedArrows)
                {
                    strintToPrintForArrowUp = "   ";
                }
                while (strintToPrintForArrowUp.Length < TextBeforeInputShown.Length - (TextBeforeInputShownVisible ? 2 : 0)) strintToPrintForArrowUp += " ";
                strintToPrintForArrowUp += "⯅";
                while (strintToPrintForArrowUp.Length < MaxSelectionMenu + 3) strintToPrintForArrowUp += " ";
                Console.WriteLine(strintToPrintForArrowUp);
            }
            else if (HasOptionsBelow)
            {
                string strintToPrintForArrowDown = "";
                if (VisibleSelectedArrows)
                {
                    strintToPrintForArrowDown = "   ";
                }
                while (strintToPrintForArrowDown.Length < TextBeforeInputShown.Length - (TextBeforeInputShownVisible ? 2 : 0)) strintToPrintForArrowDown += " ";
                while (strintToPrintForArrowDown.Length < MaxSelectionMenu + 3) strintToPrintForArrowDown += " ";
                Console.WriteLine(strintToPrintForArrowDown);
            }
            foreach (Option<T> option in Options)
            {
                if (Options.IndexOf(option) == (MaxVisibility / 2) && TextBeforeInputShownVisible)
                {
                    ColorConsole.WriteColor(TextBeforeInputShown, Globals.ColorInputcClarification);
                }
                if (option == selectedOption)
                {
                    // This will print the selected option in blue
                    string strintToPrint = "";
                    if (Console.GetCursorPosition().Left == 0)
                        while (strintToPrint.Length < TextBeforeInputShown.Length - (TextBeforeInputShownVisible ? 2 : 0)) strintToPrint += " ";
                    if (VisibleSelectedArrows) strintToPrint += $">> {option.Name} <<";
                    else strintToPrint += $"{option.Name}";
                    while (strintToPrint.Length < MaxSelectionMenu + 3) strintToPrint += " ";

                    ColorConsole.WriteColorLine($"{strintToPrint}", ConsoleColor.Blue);
                }
                else
                {
                    string strintToPrint = "";
                    if (Console.GetCursorPosition().Left == 0)
                        while (strintToPrint.Length < TextBeforeInputShown.Length - (TextBeforeInputShownVisible ? 2 : 0)) strintToPrint += " ";
                    if (VisibleSelectedArrows) strintToPrint += $"   {option.Name}";
                    else strintToPrint += $"{option.Name}";
                    while (strintToPrint.Length < MaxSelectionMenu + 6) strintToPrint += " ";
                    Console.WriteLine($"{strintToPrint}");
                }
            }
            if (HasOptionsBelow)
            {
                string strintToPrintForArrowDown = "";
                if (VisibleSelectedArrows)
                {
                    strintToPrintForArrowDown = "   ";
                }
                while (strintToPrintForArrowDown.Length < TextBeforeInputShown.Length - (TextBeforeInputShownVisible ? 2 : 0)) strintToPrintForArrowDown += " ";
                strintToPrintForArrowDown += "⯆";
                while (strintToPrintForArrowDown.Length < MaxSelectionMenu + 3) strintToPrintForArrowDown += " ";
                Console.WriteLine(strintToPrintForArrowDown);
            }
            else if (HasOptionsAbove)
            {
                string strintToPrintForArrowDown = "";
                if (VisibleSelectedArrows)
                {
                    strintToPrintForArrowDown = "   ";
                }
                while (strintToPrintForArrowDown.Length < TextBeforeInputShown.Length - (TextBeforeInputShownVisible ? 2 : 0)) strintToPrintForArrowDown += " ";
                while (strintToPrintForArrowDown.Length < MaxSelectionMenu + 3) strintToPrintForArrowDown += " ";
                Console.WriteLine(strintToPrintForArrowDown);
            }
            if (moreOptionsThanVisibility)
            {
                string strintToPrint = $"*Navigeer met de pijltjestoetsen om meer opties te zien.*";
                if (MaxSelectionMenu < strintToPrint.Length) MaxSelectionMenu = strintToPrint.Length;
                foreach (char character in strintToPrint) Console.Write(" ");
                ColorConsole.WriteLineInfo($"\n{strintToPrint}");
                string x = "";
                foreach (char character in strintToPrint) Console.Write(" ");
            }
        }

        private List<Option<T>> GetOptionsToShow(List<Option<T>> Options, int MaxVisibility, int skipOptions = 0, bool hasSkipOptions = false)
        {
            List<Option<T>> optionsToShow = new List<Option<T>>();
            //Loops trough all Options.
            for (int i = 1; i <= Options.Count; i++)
            {
                // Checks if there are options which must be skipped.
                if (hasSkipOptions)
                {
                    // Checks wheter the index of the current option is higher of the index which must be skipped. If so, the option will be added to the list.
                    if (i > skipOptions) optionsToShow.Add(Options[i - 1]);
                }
                else
                {
                    // Adds the option to the list.
                    optionsToShow.Add(Options[i - 1]);
                }
                // WHen the amount of options to show is equal to the max visibility, the loop will be broken.
                if (optionsToShow.Count == MaxVisibility) break;
            }
            return optionsToShow;
        }
        private void KeyUp()
        {
            //When the option where the user wants to go to is higher than 0, so inside the possible Options, this will be executed.
            if (Index - 1 >= 0)
            {
                //Set the index and visible one lower
                Index--;
                VisibleIndex--;
                // This will be approved when the amount of options is higher than the max visibility.
                if (AllOptions.Count > MaxVisibility)
                {
                    // When the selected option is in the first half of or equal to the visible Options, this will be executed.
                    if (Index + 1 <= HalfOfMaxVisibility)
                    {
                        VisibleIndex = Index;
                        AmountOptionsAbove = 0;
                        OptionsToShow = GetOptionsToShow(AllOptions, MaxVisibility);
                        WriteMenu(OptionsToShow, OptionsToShow[Index]);
                    }
                    // When the selected option is higher than (the option count minus the half of the max visibility), this will be executed.
                    else if (Index >= AllOptions.Count - HalfOfMaxVisibility)
                    {
                        AmountOptionsAbove = AllOptions.Count - MaxVisibility;
                        OptionsToShow = GetOptionsToShow(AllOptions, MaxVisibility, AmountOptionsAbove, true);
                        WriteMenu(OptionsToShow, OptionsToShow[VisibleIndex - 1]);
                    }
                    // When the selected option is neither of the above, this will be executed.
                    else
                    {
                        if (AmountOptionsAbove > 0) AmountOptionsAbove--;
                        OptionsToShow = GetOptionsToShow(AllOptions, MaxVisibility, AmountOptionsAbove, true);
                        if (Index + 1 < AllOptions.Count - HalfOfMaxVisibility) VisibleIndex++;
                        WriteMenu(OptionsToShow, OptionsToShow[VisibleIndex]);
                    }
                }
                else
                {
                    WriteMenu(AllOptions, AllOptions[Index]);
                }
            }
        }
        private void KeyDown()
        {
            // When the option where the user wants to go to is lower than the amount of Options, so the highest index possible, this will be executed.
            if (Index + 1 < AllOptions.Count)
            {
                //Set the index and visible index one higher
                Index++;
                VisibleIndex++;
                // This will be approved when the amount of options is higher than the max visibility.
                if (AllOptions.Count > MaxVisibility)
                {
                    //When the visible index is higher than the max visibility, this will be executed.
                    if (VisibleIndex > MaxVisibility)
                    {
                        VisibleIndex = MaxVisibility;
                    }
                    // When the selected option is in the second half of or equal to the visible Options, this will be executed.
                    if (Index >= AllOptions.Count - HalfOfMaxVisibility)
                    {
                        AmountOptionsAbove = AllOptions.Count - MaxVisibility;
                        OptionsToShow = GetOptionsToShow(AllOptions, MaxVisibility, AmountOptionsAbove, true);
                        WriteMenu(OptionsToShow, OptionsToShow[VisibleIndex - 1]);
                    }
                    // When the selected option is higher than the half of the max visibility, this will be executed.
                    else if (VisibleIndex > Math.Floor((double)MaxVisibility / 2))
                    {
                        AmountOptionsAbove++;
                        OptionsToShow = GetOptionsToShow(AllOptions, MaxVisibility, AmountOptionsAbove, true);
                        VisibleIndex--;
                        WriteMenu(OptionsToShow, OptionsToShow[VisibleIndex]);
                    }
                    // When the selected option is neither of the above, this will be executed.
                    else
                    {
                        OptionsToShow = GetOptionsToShow(AllOptions, MaxVisibility);
                        WriteMenu(OptionsToShow, OptionsToShow[Index]);
                    }
                }
                else
                {
                    WriteMenu(AllOptions, AllOptions[Index]);
                }
            }
        }

        public static List<Option<T>> ConvertToOption(List<T> options)
        {
            List<Option<T>> optionList = new List<Option<T>>();
            foreach (T option in options)
            {
                optionList.Add(new Option<T>(option));
            }
            return optionList;
        }
    }
}