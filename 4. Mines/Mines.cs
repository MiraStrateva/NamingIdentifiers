namespace Mines
{
    using System;
    using System.Collections.Generic;

    public static class Mines
    {
        private const int MAX_TURNS = 35;
        private const int RANKLIST_PLAYERS_COUNT = 6;
        private const int BOARD_ROWS = 5;
        private const int BOARD_COLUMNS = 10;
        private const int MINES_COUNT = 15;

        public static void PlayGame()
        {
            string command = string.Empty;
            char[,] emptyBoard = CreatePlayingBoard();
            char[,] minedBoard = PlaceMines();

            int turnCounter = 0;
            bool mineIsDetonated = false;
            List<Player> players = new List<Player>(RANKLIST_PLAYERS_COUNT);
            int row = 0;
            int column = 0;
            bool gameStarts = true;
            bool maxTurnsReached = false;

            do
            {
                if (gameStarts)
                {
                    Console.WriteLine("Let's play “Mines”. Try to find out all fields without mines." +
                    " Command 'top' displays the ranking list, 'restart' starts new game, 'exit' quits the game!");
                    DrawBoard(emptyBoard);
                    gameStarts = false;
                }

                Console.Write("Write row and column: ");
                command = Console.ReadLine().Trim();
                if (command.Length >= 3)
                {
                    if (int.TryParse(command[0].ToString(), out row) &&
                        int.TryParse(command[2].ToString(), out column) &&
                        row <= BOARD_ROWS && column <= BOARD_COLUMNS)
                    {
                        command = "turn";
                    }
                }

                switch (command)
                {
                    case "top":
                        DisplayRankingList(players);
                        break;
                    case "restart":
                        emptyBoard = CreatePlayingBoard();
                        minedBoard = PlaceMines();
                        DrawBoard(emptyBoard);
                        mineIsDetonated = false;
                        gameStarts = false;
                        break;
                    case "exit":
                        Console.WriteLine("Bye!");
                        break;
                    case "turn":
                        if (minedBoard[row, column] != '*')
                        {
                            if (minedBoard[row, column] == '-')
                            {
                                NextTurn(emptyBoard, minedBoard, row, column);
                                turnCounter++;
                            }

                            if (MAX_TURNS == turnCounter)
                            {
                                maxTurnsReached = true;
                            }
                            else
                            {
                                DrawBoard(emptyBoard);
                            }
                        }
                        else
                        {
                            mineIsDetonated = true;
                        }

                        break;
                    default:
                        Console.WriteLine("\nError! Invalid command!\n");
                        break;
                }

                if (mineIsDetonated)
                {
                    DrawBoard(minedBoard);
                    Console.Write("\nGame over! {0} points. Write your name: ", turnCounter);
                    string name = Console.ReadLine();
                    Player player = new Player(name, turnCounter);
                    if (players.Count < RANKLIST_PLAYERS_COUNT - 1)
                    {
                        players.Add(player);
                    }
                    else
                    {
                        for (int i = 0; i < players.Count; i++)
                        {
                            if (players[i].Points < player.Points)
                            {
                                players.Insert(i, player);
                                players.RemoveAt(players.Count - 1);
                                break;
                            }
                        }
                    }

                    players.Sort((Player p1, Player p2) => p2.Name.CompareTo(p1.Name));
                    players.Sort((Player p1, Player p2) => p2.Points.CompareTo(p1.Points));
                    DisplayRankingList(players);
                    emptyBoard = CreatePlayingBoard();
                    minedBoard = PlaceMines();
                    turnCounter = 0;
                    mineIsDetonated = false;
                    gameStarts = true;
                }

                if (maxTurnsReached)
                {
                    Console.WriteLine("\nCongratulations! You opened {0} fields before hit a mine!", MAX_TURNS);
                    DrawBoard(minedBoard);
                    Console.WriteLine("Write your name: ");
                    string name = Console.ReadLine();
                    Player playerPoints = new Player(name, turnCounter);
                    players.Add(playerPoints);
                    DisplayRankingList(players);
                    emptyBoard = CreatePlayingBoard();
                    minedBoard = PlaceMines();
                    turnCounter = 0;
                    maxTurnsReached = false;
                    gameStarts = true;
                }
            }
            while (command != "exit");
            Console.WriteLine("Made in Bulgaria!");
            Console.Read();
        }

        private static char[,] CreatePlayingBoard()
        {
            int rows = BOARD_ROWS;
            int columns = BOARD_COLUMNS;
            char[,] board = new char[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    board[i, j] = '?';
                }
            }

            return board;
        }

        private static char[,] PlaceMines()
        {
            int rows = BOARD_ROWS;
            int columns = BOARD_COLUMNS;
            char[,] board = new char[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    board[i, j] = '-';
                }
            }

            List<int> randomNumbers = new List<int>();
            while (randomNumbers.Count < MINES_COUNT)
            {
                int boardSize = rows * columns;
                Random random = new Random();
                int nextRandomNumber = random.Next(boardSize);
                if (!randomNumbers.Contains(nextRandomNumber))
                {
                    randomNumbers.Add(nextRandomNumber);
                }
            }

            foreach (int number in randomNumbers)
            {
                int mineColumn = number / columns;
                int mineRow = number % columns;
                if (mineRow == 0 && number != 0)
                {
                    mineColumn--;
                    mineRow = columns;
                }
                else
                {
                    mineRow++;
                }

                board[mineColumn, mineRow - 1] = '*';
            }

            return board;
        }

        private static void DisplayRankingList(List<Player> rankingList)
        {
            Console.WriteLine("\nRankint list:");
            if (rankingList.Count > 0)
            {
                for (int i = 0; i < rankingList.Count; i++)
                {
                    Console.WriteLine("{0}. {1} --> {2} points", i + 1, rankingList[i].Name, rankingList[i].Points);
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Empty Ranking List!\n");
            }
        }

        private static void DrawBoard(char[,] board)
        {
            Console.Write("\n    ");
            for (int i = 0; i < BOARD_COLUMNS; i++)
            {
                Console.Write("{0} ", i);    
            }

            Console.WriteLine();
            Console.WriteLine("   {0}", new string('-', BOARD_COLUMNS * 2));

            for (int i = 0; i < BOARD_ROWS; i++)
            {
                Console.Write("{0} | ", i);
                for (int j = 0; j < BOARD_COLUMNS; j++)
                {
                    Console.Write(string.Format("{0} ", board[i, j]));
                }

                Console.Write("|");
                Console.WriteLine();
            }

            Console.WriteLine("   {0}\n", new string('-', BOARD_COLUMNS * 2));
        }

        private static void NextTurn(char[,] emptyBoard, char[,] minedBoard, int row, int column)
        {
            int minesCount = GetBorderMinesCount(minedBoard, row, column);
            minedBoard[row, column] = char.Parse(minesCount.ToString());
            emptyBoard[row, column] = char.Parse(minesCount.ToString());
        }

        private static int GetBorderMinesCount(char[,] board, int row, int column)
        {
            int minesCount = 0;

            if (row - 1 >= 0)
            {
                if (board[row - 1, column] == '*')
                {
                    minesCount++;
                }
            }

            if (row + 1 < BOARD_ROWS)
            {
                if (board[row + 1, column] == '*')
                {
                    minesCount++;
                }
            }

            if (column - 1 >= 0)
            {
                if (board[row, column - 1] == '*')
                {
                    minesCount++;
                }
            }

            if (column + 1 < BOARD_COLUMNS)
            {
                if (board[row, column + 1] == '*')
                {
                    minesCount++;
                }
            }

            if ((row - 1 >= 0) && (column - 1 >= 0))
            {
                if (board[row - 1, column - 1] == '*')
                {
                    minesCount++;
                }
            }

            if ((row - 1 >= 0) && (column + 1 < BOARD_COLUMNS))
            {
                if (board[row - 1, column + 1] == '*')
                {
                    minesCount++;
                }
            }

            if ((row + 1 < BOARD_ROWS) && (column - 1 >= 0))
            {
                if (board[row + 1, column - 1] == '*')
                {
                    minesCount++;
                }
            }

            if ((row + 1 < BOARD_ROWS) && (column + 1 < BOARD_COLUMNS))
            {
                if (board[row + 1, column + 1] == '*')
                {
                    minesCount++;
                }
            }

            return minesCount;
        }
    }
}