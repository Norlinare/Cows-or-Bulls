namespace CowsAndBulls
{
    public static class GC
    {
        public static string GetPlayerInput()
        {
            return Console.ReadLine();
        }

        public static string GetPlayerInput(string promptToPlayer)
        {
            UI.DisplayGameMessage(promptToPlayer);

            return Console.ReadLine();
        }
    }
}
