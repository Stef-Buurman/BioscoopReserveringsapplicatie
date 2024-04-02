namespace BioscoopReserveringsapplicatie
{
    static class UserDetails
    {
        private static UserModel? CurrentUser = UserLogic.CurrentUser;

        public static void Start()
        {
            UserInfo();
        }

        private static void UserInfo()
        {
            if(CurrentUser != null)
            {
                Console.Clear();
                Console.WriteLine(CurrentUser.FullName);
            }
        }
    }
}