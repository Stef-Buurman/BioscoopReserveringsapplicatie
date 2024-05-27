namespace BioscoopReserveringsapplicatie
{
    public static class OptionGrid
    {
        public static Option<string>[,] GenerateOptionGrid(int width, int height, bool isRound)
        {
            Option<string>[,] options = new Option<string>[width, height];

            if (isRound)
            {
                double centerX = (width - 1) / 2.0;
                double centerY = (height - 1) / 2.0;
                double radiusX = width / 2.0;
                double radiusY = height / 2.0;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        double normalizedX = (i - centerX) / radiusX;
                        double normalizedY = (j - centerY) / radiusY;
                        double distance = Math.Sqrt(normalizedX * normalizedX + normalizedY * normalizedY);

                        if (distance <= 1.0)
                        {
                            options[i, j] = new Option<string>("X");
                        }
                        else
                        {
                            options[i, j] = null;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        options[i, j] = new Option<string>("X");
                    }
                }
            }

            return options;
        }

        public static void PrintShape(Option<string>[,] shape)
        {
            int width = shape.GetLength(0);
            int height = shape.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (shape[i, j] != null)
                    {
                        Console.Write(shape[i, j].Value);
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
