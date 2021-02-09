using ExecuteMove.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExecuteMove.Controllers
{
    /// <summary>
    /// Defines the methods that implement the Tic-Tac-Toe resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/v1/ttt")] // the url to the resource, V1 is indicating vesion 1 of the resource using path based versioning
    [Produces("application/json")] // See: https://en.wikipedia.org/wiki/Media_type    
    [ApiController]
    public class ExecuteMoveController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteMove"/> class.
        /// </summary>
        public ExecuteMoveController()
        {
        }

        /// <summary>
        /// Provides an AI-generated optimal countermove to a tic-tac-toe game state by using minimax.
        /// </summary>
        /// <returns>
        /// The optimal move found by minimax, or null if the game is at a terminal state (win or tie).
        /// </returns>
        [HttpPost]
        [Route("executemove")]
        [ProducesResponseType(typeof(OutputPayload), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status400BadRequest)] // Tells swagger that the response format will be an int for a BadRequest (400)
        public ActionResult<OutputPayload> ProcessComplexInput([FromBody] InputPayload inputPayload)
        {
            // Ensure valid board
            if (!(TicTacToe.BoardIsValid(inputPayload.gameBoard)))
                return BadRequest(400);

            // Compute response
            TicTacToe ttt = new TicTacToe(inputPayload.gameBoard,
                inputPayload.azurePlayerSymbol, inputPayload.humanPlayerSymbol);
            WinStatus ws = ttt.winStatus;

            // Get the AI's next move if game isn't over
            int? nextMove;
            if (ws.winner.Equals(TicTacToe.GAME_NOT_DONE_STR))
            {
                int choice = ttt.MinimaxNextMove();
                ttt[TicTacToe.MoveToTuple(choice)] = inputPayload.azurePlayerSymbol;
                nextMove = choice;
                // Recalculate win status
                ws = TicTacToe.GetWinStatus(ttt.board);
            }
            else
                nextMove = null;

            // Assemble response payload
            OutputPayload complexOutput = new OutputPayload()
            {
                //move = (moveTuple.Item1 < 0) ? null : TicTacToe.TupleToMove(moveTuple),
                move = nextMove,
                azurePlayerSymbol = inputPayload.azurePlayerSymbol,
                humanPlayerSymbol = inputPayload.humanPlayerSymbol,
                winner = ws.winner,
                winPositions = (ws.winPositions == null) ? null : ws.winPositions.ConvertAll(i => i.ToString()),
                gameBoard = TicTacToe.BoardToList(ttt.board),
                message = "msg",
            };

            return complexOutput;
        }
    }
}
