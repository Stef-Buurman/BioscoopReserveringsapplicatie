namespace BioscoopReserveringsapplicatie
{
    public static class TableFormatUtil
    {
        public static int[] CalculateColumnWidths<T>(List<string> columnHeaders, List<T> data)
        {
            int[] columnWidths = new int[columnHeaders.Count];

            // Loop door elke header om de breedte te initialiseren op basis van de lengte van de header
            foreach (string header in columnHeaders)
            {
                int index = columnHeaders.IndexOf(header);
                columnWidths[index] = header.Length;
            }

            // loop door elke data om de breedte te updaten als de data langer is
            foreach (T item in data)
            {
                string[] info = item.ToString().Split(",");

                // Update de kolombreedte als de data langer is dan de huidige breedte
                 for (int i = 0; i < info.Length; i++)
                 {
                     int infoLength = info[i].Length;
                     if (infoLength > 30)
                     {
                         columnWidths[i] = 30;
                     }
                     else if (infoLength > columnWidths[i])
                     {
                        columnWidths[i] = infoLength;
                     }
                 }
            }
            return columnWidths;
        }
    }
}