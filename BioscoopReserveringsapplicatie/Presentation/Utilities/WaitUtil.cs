using System.Security.Cryptography.X509Certificates;

namespace BioscoopReserveringsapplicatie
{
    public static class WaitUtil
    {
      
        public static void WaitTime(int MSTime)
        {
            Thread.Sleep(MSTime);
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
    }
}