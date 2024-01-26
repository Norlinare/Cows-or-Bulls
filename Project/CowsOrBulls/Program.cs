namespace CowsOrBulls
{
    class MainClass
    {
        //Self: Set up IO Integration for all use of file writing and reading - Good!
        //Self: Set up UserInterface for console.WriteLine - Good!
        //Self: Set up GameController to ReadLines - Good!
        //Self: Check every name
        public static void Main(string[] args)
        {
            //Set everything into game logic
            bool gameIsOngoing = true;
            Console.WriteLine("Enter your user name:\n");
            string userName = Console.ReadLine();

            while (gameIsOngoing)                      //Self: Create into main game loop function - Good!
            {                                          //Self: Split main loop into three sections?
                string targetNumberToGuess = SetTargetNumber();


                Console.WriteLine("New game:\n");
                //comment out or remove next line to play real games!
                Console.WriteLine("For practice, number is: " + targetNumberToGuess + "\n");
                string playerGuess = Console.ReadLine();


                string evaluationResult = EvaluatePlayerGuess(targetNumberToGuess, playerGuess);
                Console.WriteLine(evaluationResult + "\n");

                int numberOfPlayerGuesses = RepeatUntilCorrectAnswer(evaluationResult, targetNumberToGuess);

                StreamWriter fileWriter = new StreamWriter("result.txt", append: true);
                fileWriter.WriteLine(userName + "#&#" + numberOfPlayerGuesses);
                fileWriter.Close();
                showLeaderboard();
                Console.WriteLine("Correct, it took " + numberOfPlayerGuesses + " guesses\nContinue?");
                string playerWantsToContinue = Console.ReadLine();
                if (playerWantsToContinue != null && playerWantsToContinue != "" && answer.Substring(0, 1) == "n")
                {
                    gameIsOngoing = false;
                }
            }
        }


        static int RepeatUntilCorrectAnswer(string evaluationResult, string targetNumberToGuess)
        //Repeats a loop until the player have provided the correct answer.
        {
            int numberOfPlayerGuesses = 1;
            string correctEvaluation = "BBBB,";
            while (evaluationResult != correctEvaluation)
            {
                numberOfPlayerGuesses++;
                string playerGuess = Console.ReadLine();
                Console.WriteLine(playerGuess + "\n");
                evaluationResult = EvaluatePlayerGuess(targetNumberToGuess, playerGuess);
                Console.WriteLine(evaluationResult + "\n");
            }

            return numberOfPlayerGuesses;
        }




        static string SetTargetNumber()
        {
            Random randomNumberGenerator = new Random();
            string targetNumberToGuess = "";
            for (int i = 0; i < 4; i++)
            {
                int random = randomNumberGenerator.Next(10);
                string randomDigit = "" + random;
                while (targetNumberToGuess.Contains(randomDigit))
                {
                    random = randomNumberGenerator.Next(10);
                    randomDigit = "" + random;
                }
                targetNumberToGuess = targetNumberToGuess + randomDigit;
            }
            return targetNumberToGuess;
        }

        static string EvaluatePlayerGuess(string targetNumberToGuess, string playerGuess)
        {
            int incorrectPositionNumberCount = 0, correctPositionNumberCount = 0;
            playerGuess += "    ";     // if player entered less than 4 chars
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (targetNumberToGuess[i] == playerGuess[j])
                    {
                        if (i == j)
                        {
                            correctPositionNumberCount++;
                        }
                        else
                        {
                            incorrectPositionNumberCount++;
                        }
                    }
                }
            }
            return "BBBB".Substring(0, correctPositionNumberCount) + "," + "CCCC".Substring(0, incorrectPositionNumberCount);
        }

        //Self: Break the leaderboard into two parts: update and show
        static void showLeaderboard()
        {
            StreamReader fileReader = new StreamReader("result.txt");
            List<PlayerData> leaderboardData = new List<PlayerData>();
            string currentLine;
            while ((currentLine = fileReader.ReadLine()) != null)
            {
                string[] currentLeaderboardEntry = currentLine.Split(new string[] { "#&#" }, StringSplitOptions.None);
                string currentLeaderboardName = currentLeaderboardEntry[0];
                int currentLeaderboardGuesses = Convert.ToInt32(currentLeaderboardEntry[1]);
                PlayerData playerLeaderboardEntry = new PlayerData(currentLeaderboardName, currentLeaderboardGuesses);
                int leaderboardPosition = leaderboardData.IndexOf(playerLeaderboardEntry);
                if (leaderboardPosition < 0)
                {
                    leaderboardData.Add(playerLeaderboardEntry);
                }
                else
                {
                    leaderboardData[leaderboardPosition].Update(currentLeaderboardGuesses);
                }


            }
            leaderboardData.Sort((p1, p2) => p1.Average().CompareTo(p2.Average()));
            Console.WriteLine("Player   games average");
            foreach (PlayerData playerEntry in leaderboardData)
            {
                Console.WriteLine(string.Format("{0,-9}{1,5:D}{2,9:F2}", playerEntry.Name, playerEntry.NGames, playerEntry.Average()));
            }
            fileReader.Close();
        }
    }

    class PlayerData
    {
        public string Name { get; private set; }
        public int NGames { get; private set; }
        int totalGuess;


        public PlayerData(string playerName, int guesses)
        {
            this.Name = playerName;
            NGames = 1;
            totalGuess = guesses;
        }

        public void Update(int guesses)
        {
            totalGuess += guesses;
            NGames++;
        }

        public double Average()
        {
            return (double)totalGuess / NGames;
        }


        public override bool Equals(Object p)
        {
            return Name.Equals(((PlayerData)p).Name);
        }


        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}