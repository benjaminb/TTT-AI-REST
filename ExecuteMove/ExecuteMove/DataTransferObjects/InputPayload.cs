using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExecuteMove.DataTransferObjects
{
    /// <summary>
    /// Test class
    /// </summary>
    public class InputPayload
    {
        /// <summary>
        /// Example renaming a property using JsonProperyName
        /// </summary>
        public int move { get; set; }
        public string azurePlayerSymbol { get; set; }
        public string humanPlayerSymbol { get; set; }
        public List<string> gameBoard { get; set; }
        public string message { get; set; }
    }

    public class TicTacToe
    {
        private const string X = "X";
        private const string O = "O";
        private const string UNMARKED = "?";
        private const string GAME_NOT_DONE_STR = "inconclusive";
        private const string TIE_STR = "tie";
        private static HashSet<string> VALID_SYMBOLS = new HashSet<string>() { X, O, UNMARKED };

        public string[,] board;
        public WinStatus winStatus;
        public string azurePlayerSymbol;
        public string humanPlayerSymbol;

        public TicTacToe(List<string> inputStr, string azPlayerSymbol, string huPlayerSymbol)
        {
            board = new string[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    board[i, j] = inputStr[i * 3 + j];
            winStatus = GetWinStatus(board);
            azurePlayerSymbol = azPlayerSymbol;
            humanPlayerSymbol = huPlayerSymbol;
        }

        // Allow 2d indexing by tuples
        public string this[(int, int) coord]
        {
            get { return board[coord.Item1, coord.Item2]; }
            set { board[coord.Item1, coord.Item2] = value; }
        }

        // Converts 1d index to 2d index
        public static (int, int) MoveToTuple(int index)
        {
            return (index / 3, index % 3);
        }

        public static int TupleToMove((int, int) coords)
        {
            return 3 * coords.Item1 + coords.Item2;
        }

        // Validates board
        public static bool BoardIsValid(List<string> boardList)
        {
            if (boardList.Count != 9)
                return false;

            // Are all symbols valid?
            if (!(boardList.TrueForAll(t => VALID_SYMBOLS.Contains(t))))
                return false;
            int X_count = 0;
            int O_count = 0;
            for (int i = 0; i < 9; i++)
            {
                // All symbols valid?
                if (!(VALID_SYMBOLS.Contains(boardList[i])))
                    return false;
                if (boardList[i].Equals(X))
                    X_count++;
                else if (boardList[i].Equals(O))
                    O_count++;
            }

            // O's must equal X's or 1 fewer
            if (!(O_count == X_count || O_count == X_count - 1))
                return false;

            return true;
        }
        public static bool IsTerminal(string[,] board)
        {
            WinStatus ws = GetWinStatus(board);
            return (ws.winner.Equals(GAME_NOT_DONE_STR)) ? false : true;
        }

        // Chooses next move
        public (int, int) NextMove()
        {

            // Check for a winner or tie, if so next move is null
            if (winStatus.winner != GAME_NOT_DONE_STR)
                return (-1, -1); // sentinel value for no move

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == "?")
                    {
                        board[i, j] = azurePlayerSymbol;
                        return (i, j);
                    }
            return (-1, -1); // No moves left
        }
        

        // Determines whose turn it is
        public string Player()
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] != "?")
                        count++;
            return count % 2 == 0 ? X : O;
        }

        private static int Utility(string[,] board)
        {
            WinStatus ws = GetWinStatus(board);
            if (ws.winner.Equals(X))
                return 1;
            else if (ws.winner.Equals(O))
                return -1;
            else
                return 0;
        }
        public static WinStatus GetWinStatus(string[,] board)
        {

            // iterate over the board, look for a winner of some type
            // iterate over rows
            List<(int, int)> coords = new List<(int, int)>();

            // Check for wins on rows & columns
            for (int i = 0; i < 3; i++)
            {
                // Win on row i
                if (board[i, 0] != "?" && board[i, 0] == board[i, 1] && board[i, 0] == board[i, 2])
                {
                    // Get the win positions
                    List<int> winPositions = new List<int>();
                    for (int j = 0; j < 3; j++)
                        winPositions.Add(TupleToMove((i, j)));

                    // Return WinPositions object
                    return new WinStatus()
                    {
                        winner = board[i, 1],
                        winPositions = winPositions
                    };
                }

                // Win on column i
                if (board[0, i] != "?" && board[0, i] == board[1, i] && board[0, i] == board[2, i])
                {
                    // Get the win positions
                    List<int> winPositions = new List<int>();
                    for (int j = 0; j < 3; j++)
                        winPositions.Add(TupleToMove((j, i)));

                    return new WinStatus()
                    {
                        winner = board[0, i],
                        winPositions = winPositions,
                    };
                }
            }

            // Check for win on diagonal
            if (board[0, 0] != "?" && board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2])
            {
                return new WinStatus()
                {
                    winner = board[0, 0],
                    winPositions = new List<int>() { 0, 4, 8 },
                };
            }

            // Check for win on off-diagonal
            if (board[0, 2] != "?" && board[0, 2] == board[1, 1] && board[0, 2] == board[2, 0])
            {
                return new WinStatus()
                {
                    winner = board[0, 2],
                    winPositions = new List<int>() { 2, 4, 6 },
                };
            }

            // Check for game not yet over
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == "?")
                        return new WinStatus()
                        {
                            winner = GAME_NOT_DONE_STR,
                            winPositions = null,
                        };

            // Otherwise, all tiles filled & no winner: a tie
            return new WinStatus()
            {
                winner = TIE_STR,
                winPositions = null,
            };
        }

        public static List<string> BoardToList(string[,] board)
        {
            List<string> boardList = new List<string>();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    boardList.Add(board[i, j]);
            return boardList;
        }
    }

    public class WinStatus
    {
        public string winner { get; set; }
        public List<int> winPositions { get; set; }
    }
}
