namespace BioscoopReserveringsapplicatie
{
    public class SelectionMenuUtil<T>
    {
        private int MaxSelectionMenu = 0;

        private int Index = 0;
        private int VisibleIndex = 0;

        private bool moreOptionsThanVisibility = false;
        private int AmountOptionsAbove = 0;
        private bool HasOptionsAbove { get => AmountOptionsAbove > 0 && moreOptionsThanVisibility; }
        private bool HasOptionsBelow { get => AllOptions.Count - MaxVisibility != AmountOptionsAbove && moreOptionsThanVisibility; }
        private int HalfOfMaxVisibility { get => Convert.ToInt32(Math.Round((double)MaxVisibility / 2, MidpointRounding.AwayFromZero)); }
        private int _maxVisibility = 0;
        private int MaxVisibility
        {
            get => _maxVisibility;
            set
            {
                if (value < 0) _maxVisibility = 9;
                else if (value > 29) _maxVisibility = 9;
                else _maxVisibility = (value % 2 == 0) ? (value + 1) : value;
            }
        }

        private ConsoleColor SelectedColor = ConsoleColor.Blue;
        private ConsoleColor HighLightedColor = ConsoleColor.Yellow;
        private ConsoleColor UnselectableColor = ConsoleColor.Red;

        private List<Option<T>> AllOptions;
        private List<Option<T>> OptionsToShow = new List<Option<T>>();
        private Option<T> SelectedOption;

        private int Top = 0;

        private bool CanBeEscaped;
        private Action? EscapeAction;
        private Action? EscapeActionWhenNotEscaping;
        private bool EscapabilityVisible = false;
        private bool MultiSelectFuncVisible = false;

        private string TextBeforeInputShown = "";
        private bool TextBeforeInputShownVisible = false;
        private bool VisibleSelectedArrows;
        private bool ShowEscapeabilityText;

        private (int, int) GridIndex = (0, 0);
        private bool IsGridSelect = false;
        private string GridSeperator = null;
        private bool TableFormat = false;
        private Option<T>[,] GridOptions;
        private List<(int, int)> UnSelectableOptions;

        private bool IsMultiSelect = false;
        private List<Option<T>> SelectedOptions = new List<Option<T>>();

        private List<KeyAction> KeyActions = new List<KeyAction>();
        private List<ConsoleKey> KeysInUse = new List<ConsoleKey>(){
            ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Enter
        };

        private SelectionMenuUtil(List<Option<T>> options = null, int maxVisibility = 9, bool canBeEscaped = false,
            Action escapeAction = null, Action escapeActionWhenNotEscaping = null,
            bool visibleSelectedArrows = true, string textBeforeInputShown = default,
            Option<T> selectedOption = default, bool isMultiSelect = false,
            List<Option<T>> selectedOptions = null, bool showEscapeabilityText = true,
            Option<T>[,] gridOptions = default, List<(int, int)> selectedGridOptions = default, string gridSeperator = null, bool tableFormat = false,
            List<(int, int)> unSelectableOptions = null,
            bool hasKeyAction = false, List<KeyAction> keyActions = null, List<ConsoleKey> additionalKeysInUse = null)
        {
            MaxVisibility = maxVisibility;
            if(options != null)
            {
                AllOptions = options;
                OptionsToShow = GetOptionsToShow(AllOptions, MaxVisibility);
                moreOptionsThanVisibility = AllOptions.Count > maxVisibility;
            }
            Index = 0;
            VisibleIndex = 0;

            ShowEscapeabilityText = showEscapeabilityText;
            if ((escapeAction != null && escapeActionWhenNotEscaping == null)
                || (escapeAction == null && escapeActionWhenNotEscaping != null))
            {
                CanBeEscaped = false;
                EscapeAction = () => { };
                EscapeActionWhenNotEscaping = () => { };
            }
            else
            {
                CanBeEscaped = canBeEscaped;
                KeysInUse.Add(ConsoleKey.Escape);
                EscapeAction = escapeAction;
                EscapeActionWhenNotEscaping = escapeActionWhenNotEscaping;
            }

            if(gridOptions != default && isMultiSelect)
            {
                KeysInUse.Add(ConsoleKey.LeftArrow);
                KeysInUse.Add(ConsoleKey.RightArrow);
                IsGridSelect = true;
                GridOptions = gridOptions;
                UnSelectableOptions = unSelectableOptions;
                GridSeperator = gridSeperator;
                TableFormat = tableFormat;

                if(selectedGridOptions != default)
                {
                    foreach ((int, int) index in selectedGridOptions)
                    {
                        Option<T> option = GridOptions[index.Item1, index.Item2];
                        if (option != null)
                        {
                            option.InvertSelecttion();
                        }
                    }
                }
                if(UnSelectableOptions.Contains(GridIndex))
                {
                    Move(0, 1);
                    while(IsUnselectable(GridIndex.Item1, GridIndex.Item2) || !IsOptionAvailable(GridIndex.Item1, GridIndex.Item2))
                    {
                        GridIndex.Item1++;
                        if (!IsUnselectable(GridIndex.Item1, GridIndex.Item2) && IsOptionAvailable(GridIndex.Item1, GridIndex.Item2)) break;
                        Move(0, 1);
                        if(GridIndex.Item1 >= GridOptions.GetLength(0))
                        {
                            IsMultiSelect = false;
                            IsGridSelect = false;
                            break;
                        }
                    }
                }
            }


            if (isMultiSelect)
            {
                IsMultiSelect = isMultiSelect;
                KeysInUse.Add(ConsoleKey.Spacebar);
                VisibleSelectedArrows = false;
                if (selectedOptions != null && selectedOptions.Count > 0)
                {
                    foreach (Option<T> HighLightedOption in selectedOptions)
                    {
                        Option<T>? option = AllOptions.Find(opt => opt.Equals(HighLightedOption));
                        if (option != null)
                        {
                            option.InvertSelecttion();
                            SelectedOptions.Add(option);
                        }
                    }
                }
            }
            else
            {
                VisibleSelectedArrows = visibleSelectedArrows;
                if (selectedOption != default && AllOptions.Contains(selectedOption))
                {
                    SelectedOption = selectedOption;
                }
            }

            CanBeEscaped = canBeEscaped;
            EscapeAction = escapeAction;
            EscapeActionWhenNotEscaping = escapeActionWhenNotEscaping;

            if (textBeforeInputShown != default)
            {
                TextBeforeInputShown = textBeforeInputShown;
                TextBeforeInputShownVisible = true;
            }
            if (hasKeyAction && keyActions != null)
            {
                KeyActions = keyActions;
                if (additionalKeysInUse != null){
                    foreach (ConsoleKey key in additionalKeysInUse)
                    {
                        KeysInUse.Add(key);
                    }
                }
            }
        }

        private SelectionMenuUtil(List<T> options, int maxVisibility, bool canBeEscaped = false, Action escapeAction = null,
            Action escapeActionWhenNotEscaping = null, bool visibleSelectedArrows = true, string textBeforeInputShown = default,
            Option<T> selectedOption = default, bool isMultiSelect = false, List<Option<T>> selectedOptions = null)
            : this(ConvertToOption(options), maxVisibility, canBeEscaped, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown, selectedOption, isMultiSelect, selectedOptions) { }

        public SelectionMenuUtil(List<Option<T>> options, bool canBeEscaped = false, Action escapeAction = null, Action escapeActionWhenNotEscaping = null, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, 9, canBeEscaped, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil(List<Option<T>> options, Action escapeAction, Action escapeActionWhenNotEscaping, bool visibleSelectedArrows = true, string textBeforeInputShown = default, bool showEscapeabilityText = true)
            : this(options, 9, true, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown, null, false, null, showEscapeabilityText) { }
        public SelectionMenuUtil(List<Option<T>> options, Action escapeAction, Action escapeActionWhenNotEscaping, List<KeyAction> keyActions, bool visibleSelectedArrows = true, string textBeforeInputShown = default, bool showEscapeabilityText = true, bool hasKeyAction = true)
        : this(options, 9, true, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown, null, false, null, showEscapeabilityText, default, default, null, false, default, hasKeyAction, keyActions) { }

        public SelectionMenuUtil(List<Option<T>> options, int maxVisibility, Action escapeAction, Action escapeActionWhenNotEscaping, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, maxVisibility, true, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil(List<Option<T>> options, int maxVisibility, Action escapeAction, Action escapeActionWhenNotEscaping, string textBeforeInputShown, List<Option<T>> selectedOptions)
            : this(options, maxVisibility, true, escapeAction, escapeActionWhenNotEscaping, false, textBeforeInputShown, null, true, selectedOptions) { }

        public SelectionMenuUtil(List<Option<T>> options, int maxVisibility, Action escapeAction, Action escapeActionWhenNotEscaping, Option<T> selectedOption)
            : this(options, maxVisibility, true, escapeAction, escapeActionWhenNotEscaping, true, default, selectedOption) { }

        public SelectionMenuUtil(List<Option<T>> options, Action escapeAction, Action escapeActionWhenNotEscaping, Option<T> selectedOption)
            : this(options, 9, true, escapeAction, escapeActionWhenNotEscaping, true, default, selectedOption) { }

        public SelectionMenuUtil(List<T> options, Action escapeAction, Action escapeActionWhenNotEscaping, Option<T> selectedOption)
            : this(options, 9, true, escapeAction, escapeActionWhenNotEscaping, true, default, selectedOption) { }

        public SelectionMenuUtil(List<Option<T>> options, Option<T> selectedOption)
            : this(options, 9, false, default, default, true, default, selectedOption) { }

        public SelectionMenuUtil(Option<T>[,] options, List<(int, int)> unSelectableOptions, string gridSeperator = null, bool tableFormat = false, List<(int, int)> SelectedOptions = default)
            : this(null, 9, false, default, default, true, default, default, true, default, true, options, SelectedOptions, gridSeperator, tableFormat, unSelectableOptions) { }

        public SelectionMenuUtil(Option<T>[,] options, List<(int, int)> unSelectableOptions, bool tableFormat, List<(int, int)> SelectedOptions = default)
            : this(null, 9, false, default, default, true, default, default, true, default, true, options, SelectedOptions, null, tableFormat, unSelectableOptions) { }

        public SelectionMenuUtil(List<Option<T>> options)
            : this(options, 9, false) { }

        public SelectionMenuUtil(List<Option<T>> options, List<Option<T>> selectedOptions)
            : this(options, 9, false, null, null, true, default, null, true, selectedOptions) { }

        public SelectionMenuUtil(List<Option<T>> options, int maxVisibility)
            : this(options, maxVisibility, false) { }

        public SelectionMenuUtil(List<Option<T>> options, int maxVisibility, Action escapeAction, Action escapeActionWhenNotEscaping, string textBeforeInputShown)
            : this(options, maxVisibility, true, escapeAction, escapeActionWhenNotEscaping, false, textBeforeInputShown, null, true) { }

        public SelectionMenuUtil(List<Option<T>> options, int maxVisibility, string textBeforeInputShown = default)
            : this(options, maxVisibility, false, null, null, true, textBeforeInputShown) { }

        public SelectionMenuUtil(List<Option<T>> options, int maxVisibility, bool visibleSelectedArrows, string textBeforeInputShown)
            : this(options, maxVisibility, false, null, null, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil(List<Option<T>> options, bool canBeEscaped = false)
            : this(options, 9, canBeEscaped) { }

        public SelectionMenuUtil(List<Option<T>> options, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, 9, false, null, null, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil(List<T> options, bool canBeEscaped = false, Action escapeAction = null, Action escapeActionWhenNotEscaping = null, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, 9, canBeEscaped, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil(List<T> options, Action escapeAction, Action escapeActionWhenNotEscaping, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, 9, true, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil(List<T> options, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, 9, false, null, null, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil(List<T> options)
            : this(options, 9, false) { }

        public SelectionMenuUtil(List<T> options, List<Option<T>> selectedOptions)
            : this(options, 9, false, null, null, true, default, null, true, selectedOptions) { }

        public SelectionMenuUtil(List<T> options, Option<T> selectedOption)
            : this(options, 9, false, null, null, true, default, selectedOption) { }
        public SelectionMenuUtil(List<T> options, int maxVisibility)
            : this(options, maxVisibility, false) { }

        public SelectionMenuUtil(List<T> options, bool canBeEscaped = false)
            : this(options, 9, canBeEscaped) { }

        public SelectionMenuUtil(List<T> options, int maxVisibility, Action escapeAction, Action escapeActionWhenNotEscaping, bool visibleSelectedArrows = true, string textBeforeInputShown = default)
            : this(options, maxVisibility, true, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown) { }

        public SelectionMenuUtil(List<T> options, int maxVisibility, Action escapeAction, Action escapeActionWhenNotEscaping, bool visibleSelectedArrows = true, string textBeforeInputShown = default, Option<T> selectedOption = default)
            : this(options, maxVisibility, true, escapeAction, escapeActionWhenNotEscaping, visibleSelectedArrows, textBeforeInputShown, selectedOption) { }

        public T Create()
        {
            Index = 0;
            VisibleIndex = 0;
            Top = Console.GetCursorPosition().Top;
            if (AllOptions.Count == 0) return default;
            if (CanBeEscaped && EscapeAction == null) return default;
            Console.CursorVisible = false;

            if (SelectedOption != null)
            {
                Index = AllOptions.IndexOf(AllOptions.Find(opt => opt.Equals(SelectedOption)));
                if (Index == -1) Index = 0;
                else if (Index < HalfOfMaxVisibility || AllOptions.Count < MaxVisibility)
                {
                    VisibleIndex = Index;
                    WriteMenu(OptionsToShow, OptionsToShow[Index]);
                }
                else if (Index > AllOptions.Count - HalfOfMaxVisibility)
                {
                    AmountOptionsAbove = AllOptions.Count - MaxVisibility;
                    VisibleIndex = (Index + 1) - AmountOptionsAbove;
                    OptionsToShow = GetOptionsToShow(AllOptions, MaxVisibility, AmountOptionsAbove, true);
                    WriteMenu(OptionsToShow, OptionsToShow[VisibleIndex - 1]);
                }
                else if (Index >= HalfOfMaxVisibility)
                {
                    AmountOptionsAbove = Index - HalfOfMaxVisibility + 1;
                    VisibleIndex = HalfOfMaxVisibility - 1;
                    OptionsToShow = GetOptionsToShow(AllOptions, MaxVisibility, AmountOptionsAbove, true);
                    WriteMenu(OptionsToShow, OptionsToShow[VisibleIndex]);
                }
            }
            else
            {
                WriteMenu(OptionsToShow, OptionsToShow[Index]);
            }

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
                    AllOptions[Index].SelectFunction();
                    return AllOptions[Index].Value;
                }

                if (keyinfo.Key == ConsoleKey.Escape && CanBeEscaped && EscapeAction != null)
                {
                    ReadLineUtil.EscapeKeyPressed(EscapeAction, EscapeActionWhenNotEscaping);
                }

                specialKey(keyinfo);
                WaitTime();
            }
            while (keyinfo != null && keyinfo.Key != null);
            Console.CursorVisible = true;
            return default;
        }

        public List<T> CreateMultiSelect()
        {
            Index = 0;
            VisibleIndex = 0;
            Top = Console.GetCursorPosition().Top;
            if (AllOptions.Count == 0) return default;
            if (CanBeEscaped && EscapeAction == null) return default;
            if (!IsMultiSelect) return default;
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
                    return AllOptions.FindAll(x => x.IsSelected).ConvertAll(x => x.Value);
                }

                if (keyinfo.Key == ConsoleKey.Spacebar)
                {
                    AllOptions[Index].InvertSelecttion();
                    if (Index < HalfOfMaxVisibility)
                    {
                        WriteMenu(OptionsToShow, OptionsToShow[Index]);
                    }
                    else if (Index >= AllOptions.Count - HalfOfMaxVisibility)
                    {
                        WriteMenu(OptionsToShow, OptionsToShow[VisibleIndex - 1]);
                    }
                    else if (Index >= HalfOfMaxVisibility)
                    {
                        WriteMenu(OptionsToShow, OptionsToShow[VisibleIndex]);
                    }
                }

                if (keyinfo.Key == ConsoleKey.Escape && CanBeEscaped && EscapeAction != null)
                {
                    ReadLineUtil.EscapeKeyPressed(EscapeAction, EscapeActionWhenNotEscaping);
                }

                specialKey(keyinfo);
                WaitTime();
            }
            while (keyinfo != null && keyinfo.Key != null);
            Console.CursorVisible = true;
            return new List<T>();
        }

        public List<(int, int)> CreateGridSelect()
        {
            Index = 0;
            VisibleIndex = 0;
            Top = Console.GetCursorPosition().Top;
            if (GridOptions.Length == 0) return default;
            if (CanBeEscaped && EscapeAction == null) return default;
            if (!IsGridSelect) return default;
            Console.CursorVisible = false;

            WriteGridMenu();

            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey(true);
                // When the user presses the down arrow, the selected option will move down
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    Move(1, 0);
                    WriteGridMenu();
                }

                // When the user presses the up arrow, this will be executed.
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    Move(-1, 0);
                    WriteGridMenu();
                }

                // When the user presses the left arrow, this will be executed.
                if (keyinfo.Key == ConsoleKey.LeftArrow)
                {
                    Move(0, -1);
                    WriteGridMenu();
                }

                // When the user presses the right arrow, this will be executed.
                if (keyinfo.Key == ConsoleKey.RightArrow)
                {
                    Move(0, 1);
                    WriteGridMenu();
                }

                // When the user presses the enter key, the selected option will be executed
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    List<(int, int)> selectedIndexes = new List<(int, int)>();

                    for (int i = 0; i < GridOptions.GetLength(0); i++)
                    {
                        for (int j = 0; j < GridOptions.GetLength(1); j++)
                        {
                            if (GridOptions[i, j] != null && GridOptions[i, j].IsSelected)
                            {
                                selectedIndexes.Add((i, j));
                            }
                        }
                    }

                    return selectedIndexes;
                }

                if (keyinfo.Key == ConsoleKey.Spacebar)
                {
                    GridOptions[GridIndex.Item1, GridIndex.Item2].InvertSelecttion();
                    WriteGridMenu();
                }

                if (keyinfo.Key == ConsoleKey.Escape && CanBeEscaped && EscapeAction != null)
                {
                    ReadLineUtil.EscapeKeyPressed(EscapeAction, EscapeActionWhenNotEscaping);
                }

                specialKey(keyinfo);
                WaitTime();
            }
            while (keyinfo != null && keyinfo.Key != null);
            Console.CursorVisible = true;
            return new List<(int, int)>();
        }

        private void WriteGridMenu(string textToShowEscapability = "*Klik op escape om dit onderdeel te verlaten*")
        {
            SetCursorPosition(textToShowEscapability);
            if (TableFormat)
            {
                string stringtToPrint = "   ";
                for (int i = 0; i < GridOptions.GetLength(0); i++)
                {
                    string strToAdd = $"{i + 1}";
                    while (strToAdd.Length <= GetMaxColWith(i)) strToAdd += " ";
                    if(GridSeperator != null) strToAdd += "  ";
                    stringtToPrint += strToAdd;
                }
                ColorConsole.WriteColorLine(stringtToPrint, Console.ForegroundColor, ConsoleColor.DarkGray);
            }
                
            for (int i = 0; i < GridOptions.GetLength(0); i++)
            {
                string stringtToPrint = "";
                if (TableFormat) stringtToPrint += $"[{Console.ForegroundColor}:{ConsoleColor.DarkGray}]{i+1}:[/] ";
                for (int j = 0; j < GridOptions.GetLength(1); j++)
                {
                    string strToAdd = "";
                    string spaces = "";
                    if (GridOptions[i, j] == null)
                    {
                        strToAdd = " ";
                        while (spaces.Length <= (GetMaxColWith(j) - strToAdd.Length)) spaces += " ";
                        stringtToPrint += $"{strToAdd}";
                    }
                    else
                    {
                        strToAdd = $"{GridOptions[i, j].Name}";
                        while (spaces.Length <= (GetMaxColWith(j) - strToAdd.Length)) spaces += " ";

                        if (i == GridIndex.Item1 && j == GridIndex.Item2)
                        {
                            if (GridOptions[i, j].IsSelected)
                            {
                                stringtToPrint += $"[{ConsoleColor.White}:{SelectedColor}]{strToAdd}[/]";
                            }
                            else
                            {
                                stringtToPrint += $"[{ConsoleColor.Black}:{ConsoleColor.White}]{strToAdd}[/]";
                            }
                        }
                        else if (IsUnselectable(i, j))
                        {
                            stringtToPrint += $"[{UnselectableColor}]{strToAdd}[/]";
                        }
                        else if (GridOptions[i, j].IsSelected)
                        {
                            stringtToPrint += $"[{SelectedColor}]{strToAdd}[/]";
                        }
                        else
                        {
                            stringtToPrint += $"{strToAdd}";
                        }

                        if (j != GridOptions.GetLength(1) - 1 && GridSeperator != null)
                        {
                            stringtToPrint += $"{GridSeperator} ";
                        }
                    }
                    stringtToPrint += spaces;
                }
                MaxSelectionMenu = stringtToPrint.Length;
                while (stringtToPrint.Length < MaxSelectionMenu) stringtToPrint += " ";
                ColorConsole.WriteColorLine(stringtToPrint);
            }
        }

        private void Move(int rowDelta, int colDelta)
        {
            if (rowDelta == 0 && colDelta == 0) return;
            int oldRow = GridIndex.Item1;
            int oldCol = GridIndex.Item2;
            int newRow = oldRow;
            int newCol = oldCol;

            while (true)
            {
                newRow += rowDelta;
                newCol += colDelta;

                if (newRow >= 0 && newRow < GridOptions.GetLength(0) &&
                    newCol >= 0 && newCol < GridOptions.GetLength(1) &&
                    IsOptionAvailable(newRow, newCol))
                {
                    GridIndex = (newRow, newCol);
                    break;
                }

                if (newRow < 0 || newRow >= GridOptions.GetLength(0) ||
                    newCol < 0 || newCol >= GridOptions.GetLength(1))
                {
                    GridIndex = (oldRow, oldCol);
                    break;
                }
            }
        }

        //private Option<T> GetOption(int row, int col)
        //{
        //    if (GridOptions == null) return new Option<T>(" ");
        //    if (row < 0 || row >= GridOptions.GetLength(0) || col < 0 || col >= GridOptions.GetLength(1)) return null;
        //    return GridOptions[row, col];
        //}

        private bool IsOptionAvailable(int Yas, int Xas)
        {
            if (Yas >= GridOptions.GetLength(0) || Xas >= GridOptions.GetLength(1)) return false;
            if (GridOptions[Yas, Xas] == null) return false;
            return !IsUnselectable(Yas, Xas);
        }

        private bool IsUnselectable(int Yas, int Xas)
        {
            if (UnSelectableOptions == null) return true;
            foreach ((int, int) unSelectableOption in UnSelectableOptions)
            {
                if (unSelectableOption.Item1 == Yas && unSelectableOption.Item2 == Xas) return true;
            }
            return false;
        }

        public int GetMaxColWith(int col = -1, int row = -1) 
        {
            int max = 0;
            if (GridOptions == null || col != -1 && row != -1 || row > GridOptions.GetLength(0) || col > GridOptions.GetLength(1)) return max;
            for (int i = 0; i < GridOptions.GetLength(0); i++)
            {
                if (row != -1 && i == row)
                {
                    for (int j = 0; j < GridOptions.GetLength(1); j++)
                    {
                        if (col != -1 && j == col)
                        {
                            return GridOptions[i, j].Name.Length;
                        }
                        else
                        {
                            if (GridOptions[i, j].Name.Length > max) max = GridOptions[i, j].Name.Length;
                        }
                    }
                }
                else if(col != -1)
                {
                    if (GridOptions[i, col] != null && GridOptions[i, col].Name.Length > max) max = GridOptions[i, col].Name.Length;
                }
            }
            return max;
        }

        private void SetCursorPosition(string textToShowEscapability)
        {
            int top = Top;
            if (CanBeEscaped && !EscapabilityVisible && ShowEscapeabilityText)
            {
                ColorConsole.WriteLineInfo(textToShowEscapability + "\n");
                EscapabilityVisible = true;
                top += 2;
            }
            else if (EscapabilityVisible)
            {
                top += 2;
            }
            if (IsMultiSelect && !MultiSelectFuncVisible)
            {
                ColorConsole.WriteLineInfo("Klik op Spatie om een optie te selecteren.");
                ColorConsole.WriteLineInfo("Klik op Enter deze opties uit te kiezen.\n");
                top += 3;
                MultiSelectFuncVisible = true;
            }
            else if (MultiSelectFuncVisible)
            {
                top += 3;
            }
            Console.SetCursorPosition(0, top);
        }

        public void specialKey(ConsoleKeyInfo keyinfo)
        {
            foreach (KeyAction keyAction in KeyActions)
            {
                if (!KeysInUse.Contains(keyAction.Key))
                {
                    if (keyinfo.Key == keyAction.Key)
                    {
                        Console.CursorVisible = true;
                        keyAction.Action();
                    }
                }
            }
        }

        public void WriteMenu(List<Option<T>> Options, Option<T> selectedOption, string textToShowEscapability = "*Klik op escape om dit onderdeel te verlaten*")
        {
            int HowMuchOverrideForArrowUpAndDown = TextBeforeInputShown.Length
                    - (TextBeforeInputShownVisible ? 2 : 0)
                    + (TextBeforeInputShownVisible && MaxVisibility == 1 ? selectedOption.Name.Length / 2 : 0)
                    + (VisibleSelectedArrows ? 3 : 0)
                    + (IsMultiSelect ? 1 : 0);
            SetCursorPosition(textToShowEscapability);

            int maxOptionsLength = Options.Max(x => x.Name.Length);
            if (MaxSelectionMenu < maxOptionsLength) MaxSelectionMenu = maxOptionsLength;

            if (HasOptionsAbove)
            {
                string strintToPrintForArrowUp = "";
                if (VisibleSelectedArrows)
                {
                    strintToPrintForArrowUp = "   ";
                }
                // To override the text shown, there must be enough spaces to override the text.
                while (strintToPrintForArrowUp.Length < HowMuchOverrideForArrowUpAndDown) strintToPrintForArrowUp += " ";
                strintToPrintForArrowUp += "⯅";
                // To override the text shown, there must be enough spaces to override the text.
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

                // To override the text shown, there must be enough spaces to override the text.
                while (strintToPrintForArrowDown.Length < MaxSelectionMenu + 3) strintToPrintForArrowDown += " ";
                Console.WriteLine(strintToPrintForArrowDown);
            }

            if (IsMultiSelect) WriteMultiSelectOptions(Options, selectedOption);
            else WriteOptions(Options, selectedOption);

            if (HasOptionsBelow)
            {
                string strintToPrintForArrowDown = "";
                if (VisibleSelectedArrows)
                {
                    strintToPrintForArrowDown = "   ";
                }
                // To override the text shown, there must be enough spaces to override the text.
                while (strintToPrintForArrowDown.Length < HowMuchOverrideForArrowUpAndDown) strintToPrintForArrowDown += " ";
                strintToPrintForArrowDown += "⯆";
                // To override the text shown, there must be enough spaces to override the text.
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
                // To override the text shown, there must be enough spaces to override the text.
                while (strintToPrintForArrowDown.Length < MaxSelectionMenu + 3) strintToPrintForArrowDown += " ";
                Console.WriteLine(strintToPrintForArrowDown);
            }
            if (moreOptionsThanVisibility)
            {
                string strintToPrint = $"*Navigeer met de pijltjestoetsen om meer opties te zien.*";
                if (MaxSelectionMenu < strintToPrint.Length) MaxSelectionMenu = strintToPrint.Length;
                foreach (char character in strintToPrint) Console.Write(" ");
                ColorConsole.WriteLineInfo($"\n{strintToPrint}");
            }
        }

        public void WriteOptions(List<Option<T>> Options, Option<T> selectedOption)
        {
            foreach (Option<T> option in Options)
            {
                // When the selected option is in the middle of the visible Options and the TextBeforeInputShownVisible is true the TextBeforeInputShown will be shown.
                if (Options.IndexOf(option) == (MaxVisibility / 2) && TextBeforeInputShownVisible)
                {
                    ColorConsole.WriteColor(TextBeforeInputShown, Globals.ColorInputcClarification);
                }
                if (option == selectedOption)
                {
                    // This will print the selected option in blue
                    string strintToPrint = "";
                    if (Console.GetCursorPosition().Left == 0)
                        // To override the text shown, there must be enough spaces to override the text.
                        while (strintToPrint.Length < TextBeforeInputShown.Length - (TextBeforeInputShownVisible ? 2 : 0)) strintToPrint += " ";

                    if (VisibleSelectedArrows) strintToPrint += $">> {option.Name} <<";
                    else strintToPrint += $"{option.Name}";
                    // To override the text shown, there must be enough spaces to override the text.
                    while (strintToPrint.Length < MaxSelectionMenu + 3) strintToPrint += " ";

                    ColorConsole.WriteColorLine($"{strintToPrint}", SelectedColor);
                }
                else
                {
                    string strintToPrint = "";
                    if (Console.GetCursorPosition().Left == 0)
                        // To override the text shown, there must be enough spaces to override the text.
                        while (strintToPrint.Length < TextBeforeInputShown.Length - (TextBeforeInputShownVisible ? 2 : 0)) strintToPrint += " ";
                    if (VisibleSelectedArrows) strintToPrint += $"   {option.Name}";
                    else strintToPrint += $"{option.Name}";
                    // To override the text shown, there must be enough spaces to override the text.
                    while (strintToPrint.Length < MaxSelectionMenu + 6) strintToPrint += " ";

                    Console.WriteLine($"{strintToPrint}");
                }
            }
        }

        public void WriteMultiSelectOptions(List<Option<T>> Options, Option<T> selectedOption)
        {
            foreach (Option<T> option in Options)
            {
                // When the selected option is in the middle of the visible Options and the TextBeforeInputShownVisible is true the TextBeforeInputShown will be shown.
                if (Options.IndexOf(option) == (MaxVisibility / 2) && TextBeforeInputShownVisible)
                {
                    ColorConsole.WriteColor(TextBeforeInputShown, Globals.ColorInputcClarification);
                }
                string strintToPrint = "";
                if (Console.GetCursorPosition().Left == 0)
                    // To override the text shown, there must be enough spaces to override the text.
                    while (strintToPrint.Length < TextBeforeInputShown.Length - (TextBeforeInputShownVisible ? 2 : 0)) strintToPrint += " ";
                // When at the higlighted option, the |X| will be blue and the option name will be yellow.
                if (option.IsSelected && option == selectedOption) strintToPrint += $"[{SelectedColor}]|X|[/] [{HighLightedColor}]>{option.Name}[/]";
                // When the option is selected, the |X| will be blue.
                else if (option.IsSelected) strintToPrint += $"[{SelectedColor}]|X|  {option.Name}[/]";
                // When the option is higlighted, the option name will be yellow.
                else if (option == selectedOption) strintToPrint += $"[{HighLightedColor}]| | >{option.Name}[/]";
                // When the option is not selected or higlighted, the option will be printen normally.
                else strintToPrint += $"| |  {option.Name}";
                // To override the text shown, there must be enough spaces to override the text.
                while (strintToPrint.Length < MaxSelectionMenu + 6) strintToPrint += " ";
                
                ColorConsole.WriteColorLine($"{strintToPrint}");
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

        public static void WaitTime()
        {
            WaitUtil.WaitTime(50);
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
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