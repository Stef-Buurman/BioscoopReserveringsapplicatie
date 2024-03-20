static class SelectionMenu
{
    public static T Create<T>(List<Option<T>> options, int maxVisibility, string message = "")
    {
        Console.CursorVisible = false;
        int index = 0;

        int amountOptionsAbove = 0;
        int amountOptionsUnder = 0;

        WriteMenu(options, options[index], message);

        ConsoleKeyInfo keyinfo;
        do
        {
            keyinfo = Console.ReadKey();
            if (keyinfo.Key == ConsoleKey.DownArrow)
            {
                if (index + 1 < options.Count)
                {
                    index++;
                    if(options.Count > maxVisibility)
                    {
                        List<Option<T>> optionsToShow = new List<Option<T>>();
                        if (index > maxVisibility / 2)
                        {
                            amountOptionsAbove++;
                        }
                    }
                    else
                    {
                        WriteMenu(options, options[index], message);
                    }
                }
            }
            if (keyinfo.Key == ConsoleKey.UpArrow)
            {
                if (index - 1 >= 0)
                {
                    index--;
                    WriteMenu(options, options[index], message);
                }
            }
            if (keyinfo.Key == ConsoleKey.Enter)
            {
                options[index].Select();
                Console.CursorVisible = true;
                return options[index].Value;
            }
        }
        while (keyinfo.Key != ConsoleKey.X);

        Console.Clear();
        Console.CursorVisible = true;

        return default;
    }

    public static T Create<T>(List<T> options, int maxVisibility, string message = "")
    {
        List<Option<T>> optionList = new List<Option<T>>();
        foreach (T option in options)
        {
            optionList.Add(new Option<T>(option));
        }
        return Create(optionList, maxVisibility, message);
    }

    public static T Create<T>(List<Option<T>> options, string message = "")
    {
        return Create(options, 10, message);
    }

    public static T Create<T>(List<T> options, string message = "")
    {
        return Create(options, 10, message);
    }

    static void WriteMenu<T>(List<Option<T>> options, Option<T> selectedOption, string message = "")
    {
        Console.Clear();

        if (message != "")
        {
            Console.WriteLine(message);
        }

        foreach (Option<T> option in options)
        {
            if (option == selectedOption)
            {
                Console.Write("> ");
            }
            else
            {
                Console.Write(" ");
            }

            Console.WriteLine(option.Name);
        }
    }
}
