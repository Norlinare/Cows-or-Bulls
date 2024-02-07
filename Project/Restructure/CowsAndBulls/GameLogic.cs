namespace CowsAndBulls
{
    public static class GameLogic
    {
        public static void RunGame()
        {
            //Set everything into game logic
            bool gameIsOngoing = true;
            UI.DisplayGameMessage("Enter your user name:\n");
            string userName = Console.ReadLine();

            while (gameIsOngoing)                      //Self: Create into main game loop function - Good!
            {                                          //Self: Split main loop into three sections?
                string targetNumberToGuess = SetTargetNumber();


                UI.DisplayGameMessage("New game:\n");
                //comment out or remove next line to play real games!
                UI.DisplayGameMessage("For practice, number is: " + targetNumberToGuess + "\n");
                string playerGuess = Console.ReadLine();


                string evaluationResult = EvaluatePlayerGuess(targetNumberToGuess, playerGuess);
                UI.DisplayGameMessage(evaluationResult + "\n");

                int numberOfPlayerGuesses = RepeatUntilCorrectAnswer(evaluationResult, targetNumberToGuess);

                StreamWriter fileWriter = new StreamWriter("result.txt", append: true);
                fileWriter.WriteLine(userName + "#&#" + numberOfPlayerGuesses);
                fileWriter.Close();
                SetLeaderboard();
                UI.DisplayGameMessage("Correct, it took " + numberOfPlayerGuesses + " guesses\nContinue?");
                string playerWantsToContinue = Console.ReadLine();
                if (playerWantsToContinue != null && playerWantsToContinue != "" && playerWantsToContinue.Substring(0, 1) == "n")
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
                UI.DisplayGameMessage(playerGuess + "\n");
                evaluationResult = EvaluatePlayerGuess(targetNumberToGuess, playerGuess);
                UI.DisplayGameMessage(evaluationResult + "\n");
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
        static void SetLeaderboard()
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
                    leaderboardData[leaderboardPosition].UpdatePlayerGuesses(currentLeaderboardGuesses);
                }


            }
            leaderboardData.Sort((playerOne, playerTwo) => playerOne.CalculateAverageGuessesPerGame().CompareTo(playerTwo.CalculateAverageGuessesPerGame()));
            UI.DisplayGameMessage("Player   games average");
            foreach (PlayerData playerEntry in leaderboardData)
            {
                UI.DisplayGameMessage(string.Format("{0,-9}{1,5:D}{2,9:F2}", playerEntry.name, playerEntry.totalGamesPlayed, playerEntry.CalculateAverageGuessesPerGame()));
            }
            fileReader.Close();
        }


    }
}
