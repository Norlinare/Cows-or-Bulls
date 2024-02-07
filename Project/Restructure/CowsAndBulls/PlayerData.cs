namespace CowsAndBulls
{
    public class PlayerData
    {
        public string name { get; private set; }
        public int totalGamesPlayed { get; private set; }
        int totalPlayerGuesses;


        public PlayerData(string playerName, int guesses)
        {
            this.name = playerName;
            totalGamesPlayed = 1;
            totalPlayerGuesses = guesses;
        }

        public void UpdatePlayerGuesses(int guesses)
        {
            totalPlayerGuesses += guesses;
            totalGamesPlayed++;
        }

        public double CalculateAverageGuessesPerGame()
        {
            return (double)totalPlayerGuesses / totalGamesPlayed;
        }


        public override bool Equals(Object playerData)
        {
            return name.Equals(((PlayerData)playerData).name);
        }


        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    }
}
