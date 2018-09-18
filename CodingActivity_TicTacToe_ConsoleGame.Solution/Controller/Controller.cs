using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodingActivity_TicTacToe_ConsoleGame
{
    public class GameController
    {
        #region FIELDS
        //
        // track game and round status
        //
        private bool _playingGame;
        private bool _playingRound;

        private int _roundNumber;

        //
        // track the results of multiple rounds
        //
        private int _playerXNumberOfWins;
        private int _playerONumberOfWins;
        private int _numberOfCatsGames;

        //
        // instantiate  a Gameboard object
        // instantiate a GameView object and give it access to the Gameboard object
        //
        private static Gameboard _gameboard = new Gameboard();
        private static ConsoleView _gameView = new ConsoleView(_gameboard);

        #endregion

        #region PROPERTIES

        #endregion

        #region CONSTRUCTORS

        public GameController()
        {
            InitializeGame();
            PlayGame();
        }
        
        #endregion

        #region METHODS

        /// <summary>
        /// Initialize the multi-round game.
        /// </summary>
        public void InitializeGame()
        {
            //
            // Initialize game variables
            //
            _playingGame = true;
            _playingRound = false;
            _roundNumber = 0;
            _playerONumberOfWins = 0;
            _playerXNumberOfWins = 0;
            _numberOfCatsGames = 0;

            //
            // Initialize game board status
            //
            _gameboard.InitializeGameboard();
        }


        /// <summary>
        /// Game Loop
        /// </summary>
        public void PlayGame()
        {
            _gameView.DisplayWelcomeScreen();
            
            //
            // opening menu happens
            //
            ManageOpeningMenuOption();
            
            //
            // game loop happens
            //
            while (_playingGame)
            {
                //
                // main menu happens here
                //
                ManageMainMenuOption();
                
                //
                // Round loop happens
                //
                while (_playingRound)
                {
                    //
                    // Perform the task associated with the current game and round state
                    //
                    ManageGameStateTasks();
                }

                //
                // Go back to main menu
                //
                _gameboard.InitializeGameboard();
                _gameView.InitializeView();
                _playingRound = false;
            }

            _gameView.DisplayExitPrompt();
        }

        /// <summary>
        /// manage each new task based on the current game state
        /// </summary>
        private void ManageGameStateTasks()
        {
            switch (_gameView.CurrentViewState)
            {
                case ConsoleView.ViewState.Active:
                    if(_gameboard.CurrentRoundState != Gameboard.GameboardState.NewRound)
                    {
                        _gameView.DisplayGameArea();
                    }

                    switch (_gameboard.CurrentRoundState)
                    {
                        case Gameboard.GameboardState.NewRound:
                            _roundNumber++;
                            //Choose first player
                            SelectFirstPlayer();
                            break;

                        case Gameboard.GameboardState.PlayerXTurn:
                            ManagePlayerTurn(Gameboard.PlayerPiece.X);
                            break;

                        case Gameboard.GameboardState.PlayerOTurn:
                            ManagePlayerTurn(Gameboard.PlayerPiece.O);
                            break;

                        case Gameboard.GameboardState.PlayerXWin:
                            _playerXNumberOfWins++;
                            _gameView.DisplayCurrentGameStatus(_roundNumber, _playerXNumberOfWins, _playerONumberOfWins, _numberOfCatsGames);
                            _playingRound = false;
                            break;

                        case Gameboard.GameboardState.PlayerOWin:
                            _playerONumberOfWins++;
                            _gameView.DisplayCurrentGameStatus(_roundNumber, _playerXNumberOfWins, _playerONumberOfWins, _numberOfCatsGames);
                            _playingRound = false;
                            break;

                        case Gameboard.GameboardState.CatsGame:
                            _numberOfCatsGames++;
                            _gameView.DisplayCurrentGameStatus(_roundNumber, _playerXNumberOfWins, _playerONumberOfWins, _numberOfCatsGames);
                            _playingRound = false;
                            break;

                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Attempt to get a valid player move. 
        /// If the player chooses a location that is taken, the CurrentRoundState remains unchanged,
        /// the player is given a message indicating so, and the game loop is cycled to allow the player
        /// to make a new choice.
        /// </summary>
        /// <param name="currentPlayerPiece">identify as either the X or O player</param>
        private void ManagePlayerTurn(Gameboard.PlayerPiece currentPlayerPiece)
        {
            GameboardPosition gameboardPosition = _gameView.GetPlayerPositionChoice();

            //
            // player chose an open position on the game board, add it to the game board
            //
            if (_gameboard.GameboardColumnAvailable(gameboardPosition.Column - 1))
            {
                _gameboard.SetPlayerPiece(gameboardPosition, currentPlayerPiece);

                //
                // Evaluate and update the current game board state
                //
                _gameboard.UpdateGameboardState(gameboardPosition.Column - 1);
            }
        }

        /// <summary>
        /// Manage the opening menu
        /// </summary>
        private void ManageOpeningMenuOption()
        {
            switch (_gameView.DisplayOpeningMenu())
            {
                    case OpeningMenuOption.MainMenu:
                        _playingGame = true;
                        break;
                    case OpeningMenuOption.Quit:
                        _playingGame = false;
                        _gameView.DisplayExitPrompt();
                        break;
                    default:
                        break;
            }
        }

        /// <summary>
        /// Manage the main menu 
        /// </summary>
        private void ManageMainMenuOption()
        {
            switch (_gameView.DisplayMainMenu())
            {
                    case MainMenuOption.PlayNewRound:
                        _playingRound = true;
                        break;
                    case MainMenuOption.ViewRules:
                        // placeholder code
                        break;
                    case MainMenuOption.ViewCurrentGameResults:
                        _gameView.DisplayCurrentGameStatus(_roundNumber, _playerXNumberOfWins, _playerONumberOfWins, _numberOfCatsGames);
                        break;
                    case MainMenuOption.Quit:
                        _playingGame = false;
                        break;
                    default:
                        break;
            }
        }

        /// <summary>
        /// Players choose which player goes first in the Connect Four game
        /// </summary>
        /// <returns></returns>
        private void SelectFirstPlayer()
        {
            var selection = new ConsoleKeyInfo();
            int index = 0;
            string[] playerOptions = new string[]
            {
                "Player " + Gameboard.PlayerPiece.X.ToString(),
                "Player " + Gameboard.PlayerPiece.O.ToString()
            };

            _gameView.SelectFirstPlayer(index, playerOptions);

            //Move selector until user presses ENTER on one option
            while (selection.Key != ConsoleKey.Enter)
            {
                //Get a key from the user
                selection = _gameView.GetKey();

                //Handle the key pressed and update the current index
                index = HandleKeyMovement(selection, index, playerOptions.Length);

                //Display the list of options after the movement
                _gameView.DisplayFirstPlayerOptions(index, playerOptions);
            }

            if(index == 0)
            {
                _gameboard.CurrentRoundState = Gameboard.GameboardState.PlayerXTurn;
            }
            else
            {
                _gameboard.CurrentRoundState = Gameboard.GameboardState.PlayerOTurn;
            }
        }

        /// <summary>
        /// Handler for when Player Uses Up and Down Keys
        /// </summary>
        /// <param name="i"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int HandleKeyMovement(ConsoleKeyInfo i, int index, int lengthOfOptions)
        {
            if (i.Key == ConsoleKey.DownArrow)
            {
                index++;
                if (index > lengthOfOptions - 1) index = 0;
            }
            else if (i.Key == ConsoleKey.UpArrow)
            {
                index--;
                if (index < 0) index = lengthOfOptions - 1;
            }

            return index;
        }

        #endregion
    }
}
