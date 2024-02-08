namespace CowsAndBulls
{
    public static class GC
    {
        public static string GetPlayerInput()
        {
            string playerInput = Console.ReadLine();

            if (playerInput == null)
            {
                return string.Empty;
            }
            else
            {
                return playerInput;
            }

        }

        public static string GetPlayerInputWithPrompt(string promptToPlayer)
        {
            UI.DisplayGameMessage(promptToPlayer);

            string playerInput = Console.ReadLine();

            if (playerInput == null)
            {
                return string.Empty;
            }
            else
            {
                return playerInput;
            }
        }
    }
}
