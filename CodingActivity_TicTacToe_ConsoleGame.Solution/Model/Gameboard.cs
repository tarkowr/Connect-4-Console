using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

//using TicTacToe.ConsoleApp.Model;

namespace CodingActivity_TicTacToe_ConsoleGame
{
    public class Gameboard
    {
        #region ENUMS

        public enum PlayerPiece
        {
            X,
            O,
            None
        }

        public enum GameboardState
        {
            NewRound,
            PlayerXTurn,
            PlayerOTurn,
            PlayerXWin,
            PlayerOWin,
            CatsGame
        }

        private enum PositionMovement
        {
            Up,
            UpRight,
            Right,
            DownRight,
            Down,
            DownLeft,
            Left,
            UpLeft
        }

        #endregion

        #region FIELDS

        private const int MAX_NUM_OF_ROWS = 6;

        private const int MAX_NUM_OF_COLUMNS = 7;

        private List<int> _rows;

        private List<int> _columns;

        private PlayerPiece[,] _positionState;

        private GameboardState _currentRoundState;

        #endregion

        #region PROPERTIES

        public int MaxNumOfRows
        {
            get { return MAX_NUM_OF_ROWS; }
        }

        public int MaxNumOfColumns
        {
            get { return MAX_NUM_OF_COLUMNS; }
        }

        private List<int> Rows
        {
            get { return _rows; }
        }

        private List<int> Columns
        {
            get { return _columns; }
        }

        public PlayerPiece[,] PositionState
        {
            get { return _positionState; }
            set { _positionState = value; }
        }

        public GameboardState CurrentRoundState
        {
            get { return _currentRoundState; }
            set { _currentRoundState = value; }
        }
        #endregion

        #region CONSTRUCTORS

        public Gameboard()
        {
            _positionState = new PlayerPiece[MAX_NUM_OF_ROWS, MAX_NUM_OF_COLUMNS];
        }

        #endregion

        #region METHODS

        /// <summary>
        /// fill the game board array with "None" enum values
        /// </summary>
        public void InitializeGameboard()
        {
            _currentRoundState = GameboardState.NewRound;
            _rows = new List<int>();
            _columns = new List<int>();

            //
            // Set all PlayerPiece array values to "None"
            //
            for (int row = 0; row < MAX_NUM_OF_ROWS; row++)
            {
                _rows.Add(row);

                for (int column = 0; column < MAX_NUM_OF_COLUMNS; column++)
                {
                    _columns.Add(column);
                    _positionState[row, column] = PlayerPiece.None;
                }
            }
        }

        /// <summary>
        /// Determine if a single column has one or more open spots
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <returns>true if column has one or more open spots</returns>
        public bool GameboardColumnAvailable(int column)
        {
            return _rows.Count(row => _positionState[row, column] == PlayerPiece.None) > 0 ? true : false;
        }

        /// <summary>
        /// Calculate the next available spot in a column
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <returns>The next available spot (row) in a column</returns>
        public int NextAvailableRowInColumn(int column)
        {
            return _rows.Where(x => _positionState[x, column] == PlayerPiece.None).Max();
        }

        /// <summary>
        /// Calculate the last move made in a column
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <returns>The next available spot (row) in a column</returns>
        public int LastMoveInColumn(int column)
        {
            return _rows.Where(x => _positionState[x, column] == PlayerPiece.X || _positionState[x, column] == PlayerPiece.O).Min();
        }

        /// <summary>
        /// Return the PlayerPiece at the gameboardPosition
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <returns></returns>
        public PlayerPiece GetPlayerPieceByGameBoardPosition (GameboardPosition gameboardPosition)
        {
            return _positionState[gameboardPosition.Row, gameboardPosition.Column];
        }

        /// <summary>
        /// Return a list of column indexes that have one or more open spots 
        /// </summary>
        /// <returns></returns>
        public List<int> OpenColumns()
        {
            return _columns.Where(col => GameboardColumnAvailable(col)).ToList();
        }

        /// <summary>
        /// Update the game board state if a player wins or a cat's game happens.
        /// </summary>
        public void UpdateGameboardState(int column)
        {
            //Get the row index of the most recent move in the column
            int row = LastMoveInColumn(column);

            //Create a gameboard position for the most recent move
            GameboardPosition gameboardPosition = new GameboardPosition(row, column);

            //Get the piece (X or O) of the most recent move
            PlayerPiece piece = GetPlayerPieceByGameBoardPosition(gameboardPosition);

            //Check for a win
            if (FourInARow(piece, gameboardPosition))
            {
                if(piece == PlayerPiece.X)
                {
                    _currentRoundState = GameboardState.PlayerXWin;
                }
                else
                {
                    _currentRoundState = GameboardState.PlayerOWin;
                }
            }

            //Check if all positions are filled
            else if (IsCatsGame())
            {
                _currentRoundState = GameboardState.CatsGame;
            }
        }
        
        public bool IsCatsGame()
        {
            //
            // All positions on board are filled and no winner
            //
            for (int row = 0; row < MAX_NUM_OF_ROWS; row++)
            {
                for (int column = 0; column < MAX_NUM_OF_COLUMNS; column++)
                {
                    if (_positionState[row, column] == PlayerPiece.None)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Check for any four in a row.
        /// </summary>
        /// <param name="playerPieceToCheck">Player's game piece to check</param>
        /// <returns>true if a player has won</returns>
        private bool FourInARow(PlayerPiece playerPieceToCheck, GameboardPosition gameboardPosition)
        {
            //Define linear checks for a win
            PositionMovement[] upDown = new PositionMovement[] { PositionMovement.Up, PositionMovement.Down };
            PositionMovement[] leftRight = new PositionMovement[] { PositionMovement.Left, PositionMovement.Right };
            PositionMovement[] UrightDleft = new PositionMovement[] { PositionMovement.UpRight, PositionMovement.DownLeft };
            PositionMovement[] UleftDright = new PositionMovement[] { PositionMovement.UpLeft, PositionMovement.DownRight };

            int counter = 0;

            //Check up and down
            counter = ConsecutivePieces(playerPieceToCheck, gameboardPosition, upDown[0], upDown[1]);

            if (CheckForFourPieces(counter)) return true;

            //Check left and right
            counter = ConsecutivePieces(playerPieceToCheck, gameboardPosition, leftRight[0], leftRight[1]);

            if (CheckForFourPieces(counter)) return true;

            //Check up right and down left
            counter = ConsecutivePieces(playerPieceToCheck, gameboardPosition, UrightDleft[0], UrightDleft[1]);

            if (CheckForFourPieces(counter)) return true;

            //Check up left and down right
            counter = ConsecutivePieces(playerPieceToCheck, gameboardPosition, UleftDright[0], UleftDright[1]);

            if (CheckForFourPieces(counter)) return true;

            return false;
        }

        /// <summary>
        /// Check if the count of consecutive linear pieces is greater than or equal to 4
        /// </summary>
        /// <param name="total"></param>
        /// <returns></returns>
        private bool CheckForFourPieces(int total)
        {
            return total >= 4 ? true : false;
        }

        /// <summary>
        /// Count linear consecutive pieces
        /// </summary>
        private int ConsecutivePieces(PlayerPiece piece, GameboardPosition gameboardPosition, PositionMovement movement1, PositionMovement movement2)
        {
            //Max number of pieces to check in any direction from last move
            const int piecesTocheck = 3;
            int counter = 1;

            //Check consecutive linear pieces in one direction
            for (int i = 0; i < piecesTocheck; i++)
            {
                if (CheckNextPiece(piece, gameboardPosition, movement1, i + 1))
                {
                    counter = counter + 1;
                }
                else
                {
                    break;
                }
            }

            //Check consecutive linear pieces in the other direction
            for (int i = 0; i < piecesTocheck; i++)
            {
                if (CheckNextPiece(piece, gameboardPosition, movement2, i + 1))
                {
                    counter = counter + 1;
                }
                else
                {
                    break;
                }
            }

            return counter;
        }

        /// <summary>
        /// Check if the next piece in line is a valid consecutive piece
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="gameboardPosition"></param>
        /// <param name="movement"></param>
        /// <param name="numberOfMoves"></param>
        /// <returns></returns>
        private bool CheckNextPiece(PlayerPiece piece, GameboardPosition gameboardPosition, PositionMovement movement, int numberOfMoves)
        {
            //Define constraints
            const int top = -1;
            const int bottom = 6;
            const int right = 7;
            const int left = -1;

            //The next position that we are checking
            GameboardPosition newPosition = new GameboardPosition(-1, -1);

            //Based on the given Movement, set newPosition to the next position in line 
            switch (movement)
            {
                case PositionMovement.Up:
                    newPosition = MovePositionUp(gameboardPosition, numberOfMoves);
                    break;
                case PositionMovement.UpRight:
                    newPosition = MovePositionUpRight(gameboardPosition, numberOfMoves);
                    break;
                case PositionMovement.Right:
                    newPosition = MovePositionRight(gameboardPosition, numberOfMoves);
                    break;
                case PositionMovement.DownRight:
                    newPosition = MovePositionDownRight(gameboardPosition, numberOfMoves);
                    break;
                case PositionMovement.Down:
                    newPosition = MovePositionDown(gameboardPosition, numberOfMoves);
                    break;
                case PositionMovement.DownLeft:
                    newPosition = MovePositionDownLeft(gameboardPosition, numberOfMoves);
                    break;
                case PositionMovement.Left:
                    newPosition = MovePositionLeft(gameboardPosition, numberOfMoves);
                    break;
                case PositionMovement.UpLeft:
                    newPosition = MovePositionUpLeft(gameboardPosition, numberOfMoves);
                    break;
                default:
                    break;
            }

            //Check if new position is within the borders
            if(newPosition.Row < bottom && newPosition.Row > top && newPosition.Column < right && newPosition.Column > left){

                //Check if the new position is the right piece
                if (GetPlayerPieceByGameBoardPosition(newPosition) == piece)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Move position up one row
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private GameboardPosition MovePositionUp(GameboardPosition gameboardPosition, int number)
        {
            gameboardPosition.Row = gameboardPosition.Row - number;

            return gameboardPosition;
        }

        /// <summary>
        /// Move position up one row, one column to the right
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private GameboardPosition MovePositionUpRight(GameboardPosition gameboardPosition, int number)
        {
            gameboardPosition.Column = gameboardPosition.Column + number;
            gameboardPosition.Row = gameboardPosition.Row - number;

            return gameboardPosition;
        }

        /// <summary>
        /// Move position one column to the right
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private GameboardPosition MovePositionRight(GameboardPosition gameboardPosition, int number)
        {
            gameboardPosition.Column = gameboardPosition.Column + number;

            return gameboardPosition;
        }

        /// <summary>
        /// Move position down one row, one column to the right
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private GameboardPosition MovePositionDownRight(GameboardPosition gameboardPosition, int number)
        {
            gameboardPosition.Column = gameboardPosition.Column + number;
            gameboardPosition.Row = gameboardPosition.Row + number;

            return gameboardPosition;
        }

        /// <summary>
        /// Move position down one row
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private GameboardPosition MovePositionDown(GameboardPosition gameboardPosition, int number)
        {
            gameboardPosition.Row = gameboardPosition.Row + number;

            return gameboardPosition;
        }

        /// <summary>
        /// Move position down one row, one column to the left
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private GameboardPosition MovePositionDownLeft(GameboardPosition gameboardPosition, int number)
        {
            gameboardPosition.Column = gameboardPosition.Column - number;
            gameboardPosition.Row = gameboardPosition.Row + number;

            return gameboardPosition;
        }

        /// <summary>
        /// Move position one column to the left
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private GameboardPosition MovePositionLeft(GameboardPosition gameboardPosition, int number)
        {
            gameboardPosition.Column = gameboardPosition.Column - number;

            return gameboardPosition;
        }

        /// <summary>
        /// Move position up one row, one column to the left
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private GameboardPosition MovePositionUpLeft(GameboardPosition gameboardPosition, int number)
        {
            gameboardPosition.Column = gameboardPosition.Column - number;
            gameboardPosition.Row = gameboardPosition.Row - number;

            return gameboardPosition;
        }

        /// <summary>
        /// Add player's move to the game board.
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <param name="PlayerPiece"></param>
        public void SetPlayerPiece(GameboardPosition gameboardPosition, PlayerPiece PlayerPiece)
        {
            //
            // Row and column value adjusted to match array structure
            // Note: gameboardPosition converted to array index by subtracting 1
            //
            _positionState[NextAvailableRowInColumn(gameboardPosition.Column - 1), gameboardPosition.Column - 1] = PlayerPiece;

            //
            // Change game board state to next player
            //
            SetNextPlayer();
        }

        /// <summary>
        /// Switch the game board state to the next player.
        /// </summary>
        private void SetNextPlayer()
        {
            if (_currentRoundState == GameboardState.PlayerXTurn)
            {
                _currentRoundState = GameboardState.PlayerOTurn;
            }
            else
            {
                _currentRoundState = GameboardState.PlayerXTurn;
            }
        }

        #endregion
    }
}

