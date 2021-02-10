using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExecuteMove.DataTransferObjects
{
    /// <summary>
    /// Structures the output for the response
    /// </summary>
    public class OutputPayload
    {
        /// <summary>
        /// The move returned, or null if the game was over
        /// </summary>
        public int? move { get; set; }

        /// <summary>
        /// The symbol used by the AI player
        /// </summary>
        public string azurePlayerSymbol { get; set; }

        /// <summary>
        /// The symbol used by the human player
        /// </summary>
        public string humanPlayerSymbol { get; set; }

        /// <summary>
        /// The symbol of the winning player, or null of no winner
        /// </summary>
        public string winner { get; set; }

        /// <summary>
        /// The positions indicating the win, or null if no win
        /// </summary>
        public List<string> winPositions { get; set; }

        /// <summary>
        /// The state of the game board
        /// </summary>
        public List<string> gameBoard { get; set; }
    }
}

