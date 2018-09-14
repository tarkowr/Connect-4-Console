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
        /// Return a list of column indexes that have one or more open spots 
        /// </summary>
        /// <returns></returns>
        public List<int> OpenColumns()
        {
            return _columns.Where(col => GameboardColumnAvailable(col)).ToList();
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
        /// Calculate the next available spot in a column
        /// </summary>
        /// <param name="gameboardPosition"></param>
        /// <returns>The next available spot (row) in a column</returns>
        public int LastMoveInColumn(int column)
        {
            return _rows.Where(x => _positionState[x, column] == PlayerPiece.X || _positionState[x, column] == PlayerPiece.O).Min();
        }

        public PlayerPiece GetPlayerPieceByGameBoardPosition (GameboardPosition gameboardPosition)
        {
            return _positionState[gameboardPosition.Row, gameboardPosition.Column];
        }

        public PlayerPiece GetOtherPlayerPiece(PlayerPiece piece)
        {
            if(piece == PlayerPiece.X)
            {
                return PlayerPiece.O;
            }
            else
            {
                return PlayerPiece.X;
            }
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
        /// Check for any three in a row.
        /// </summary>
        /// <param name="playerPieceToCheck">Player's game piece to check</param>
        /// <returns>true if a player has won</returns>
        private bool FourInARow(PlayerPiece playerPieceToCheck, GameboardPosition gameboardPosition)
        {
            GameboardPosition originalPosition = gameboardPosition;

            //Check up and down

            //Check left and right

            //Check up right and down left

            //Check up left and down right

            return false;
        }

        private bool IsWallOrEnemyPieceNext(PlayerPiece piece, GameboardPosition gameboardPosition, PositionMovement movement)
        {
            const int top = 8;
            const int bottom = -1;
            const int right = 8;
            const int left = -1;

            List<int> walls = new List<int>() { top, right, bottom, left };

            PlayerPiece enemyPiece = GetOtherPlayerPiece(piece);

            GameboardPosition newPosition = new GameboardPosition(-1, -1);

            switch (movement)
            {
                case PositionMovement.Up:
                    newPosition = MovePositionUp(gameboardPosition);
                    break;
                case PositionMovement.UpRight:

                    break;
                case PositionMovement.Right:

                    break;
                case PositionMovement.DownRight:

                    break;
                case PositionMovement.Down:

                    break;
                case PositionMovement.DownLeft:

                    break;
                case PositionMovement.Left:

                    break;
                case PositionMovement.UpLeft:

                    break;
                default:
                    break;
            }

            if(walls.Contains(newPosition.Row) || walls.Contains(newPosition.Column)){
                return true;
            }
            else
            {
                if(GetPlayerPieceByGameBoardPosition(newPosition) == enemyPiece)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private GameboardPosition MovePositionUp(GameboardPosition gameboardPosition)
        {
            gameboardPosition.Row = gameboardPosition.Row - 1;

            return gameboardPosition;
        }

        private GameboardPosition MovePositionUpRight(GameboardPosition gameboardPosition)
        {
            gameboardPosition.Column = gameboardPosition.Column + 1;
            gameboardPosition.Row = gameboardPosition.Row - 1;

            return gameboardPosition;
        }

        private GameboardPosition MovePositionRight(GameboardPosition gameboardPosition)
        {
            gameboardPosition.Column = gameboardPosition.Column + 1;

            return gameboardPosition;
        }

        private GameboardPosition MovePositionDownRight(GameboardPosition gameboardPosition)
        {
            gameboardPosition.Column = gameboardPosition.Column + 1;
            gameboardPosition.Row = gameboardPosition.Row + 1;

            return gameboardPosition;
        }

        private GameboardPosition MovePositionDown(GameboardPosition gameboardPosition)
        {
            gameboardPosition.Row = gameboardPosition.Row + 1;

            return gameboardPosition;
        }

        private GameboardPosition MovePositionDownLeft(GameboardPosition gameboardPosition)
        {
            gameboardPosition.Column = gameboardPosition.Column - 1;
            gameboardPosition.Row = gameboardPosition.Row + 1;

            return gameboardPosition;
        }

        private GameboardPosition MovePositionLeft(GameboardPosition gameboardPosition)
        {
            gameboardPosition.Column = gameboardPosition.Column - 1;

            return gameboardPosition;
        }

        private GameboardPosition MovePositionUpLeft(GameboardPosition gameboardPosition)
        {
            gameboardPosition.Column = gameboardPosition.Column - 1;
            gameboardPosition.Row = gameboardPosition.Row - 1;

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
            //_positionState[gameboardPosition.Row - 1, gameboardPosition.Column - 1] = PlayerPiece;
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

