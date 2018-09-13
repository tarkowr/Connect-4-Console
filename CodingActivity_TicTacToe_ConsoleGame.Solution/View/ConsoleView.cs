using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CodingActivity_TicTacToe_ConsoleGame
{
    public class ConsoleView
    {
        #region ENUMS

        public enum ViewState
        {
            Active,
            PlayerTimedOut, // TODO Track player time on task
            PlayerUsedMaxAttempts
        }

        #endregion

        #region FIELDS

        private const int GAMEBOARD_VERTICAL_LOCATION = 7;

        private const int POSITIONPROMPT_VERTICAL_LOCATION = 20;
        private const int POSITIONPROMPT_HORIZONTAL_LOCATION = 3;

        private const int MESSAGEBOX_VERTICAL_LOCATION = 23;

        private const int TOP_LEFT_ROW = 3;
        private const int TOP_LEFT_COLUMN = 6;

        private Gameboard _gameboard;
        private ViewState _currentViewStat;

        #endregion

        #region PROPERTIES
        public ViewState CurrentViewState
        {
            get { return _currentViewStat; }
            set { _currentViewStat = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public ConsoleView(Gameboard gameboard)
        {
            _gameboard = gameboard;

            InitializeView();

        }

        #endregion

        #region METHODS

        /// <summary>
        /// Initialize the console view
        /// </summary>
        public void InitializeView()
        {
            _currentViewStat = ViewState.Active;

            InitializeConsole();
        }

        /// <summary>
        /// configure the console window
        /// </summary>
        public void InitializeConsole()
        {
            ConsoleUtil.WindowWidth = ConsoleConfig.windowWidth;
            ConsoleUtil.WindowHeight = ConsoleConfig.windowHeight;

            Console.BackgroundColor = ConsoleConfig.bodyBackgroundColor;
            Console.ForegroundColor = ConsoleConfig.bodyBackgroundColor;

            ConsoleUtil.WindowTitle = "Connect Four";
        }

        /// <summary>
        /// display the Continue prompt
        /// </summary>
        public void DisplayContinuePrompt()
        {
            Console.CursorVisible = false;

            Console.WriteLine();

            ConsoleUtil.DisplayMessage("Press any key to continue.");
            ConsoleKeyInfo response = Console.ReadKey();

            Console.WriteLine();

            Console.CursorVisible = true;
        }

        /// <summary>
        /// display the Exit prompt on a clean screen
        /// </summary>
        public void DisplayExitPrompt()
        {
            ConsoleUtil.DisplayReset();

            Console.CursorVisible = false;

            Console.WriteLine();
            ConsoleUtil.DisplayMessage("Thank you for playing the Connect Four game! Press any key to EXIT.");

            Console.ReadKey();

            System.Environment.Exit(0);
        }

        /// <summary>
        /// display the session timed out screen
        /// </summary>
        public void DisplayTimedOutScreen()
        {
            ConsoleUtil.HeaderText = "Session Timed Out!";
            ConsoleUtil.DisplayReset();

            DisplayMessageBox("It appears your session has timed out.");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display the maximum attempts reached screen
        /// </summary>
        public void DisplayMaxAttemptsReachedScreen()
        {
            StringBuilder sb = new StringBuilder();

            ConsoleUtil.HeaderText = "Maximum Attempts Reached!";
            ConsoleUtil.DisplayReset();

            sb.Append(" It appears that you are having difficulty entering your");
            sb.Append(" choice. Please refer to the instructions and play again.");

            DisplayMessageBox(sb.ToString());

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Inform the player that their position choice is not available
        /// </summary>
        public void DisplayGamePositionChoiceNotAvailableScreen()
        {
            StringBuilder sb = new StringBuilder();

            ConsoleUtil.HeaderText = "Column Unavailable";
            ConsoleUtil.DisplayReset();

            sb.Append(" The column is already full. Please try another column.");

            DisplayMessageBox(sb.ToString());

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display the welcome screen
        /// </summary>
        public void DisplayWelcomeScreen()
        {
            StringBuilder sb = new StringBuilder();

            ConsoleUtil.HeaderText = "Connect Four";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Written by Jacob Barsheff, Wyatt Miller, and Richie Tarkowski");
            ConsoleUtil.DisplayMessage("Northwestern Michigan College CIT 255");
            Console.WriteLine();

            sb.Clear();
            sb.AppendFormat("This application is designed to allow two players to play ");
            sb.AppendFormat("a game of tic-tac-toe. The rules are the standard rules for the ");
            sb.AppendFormat("game with each player taking a turn.");
            ConsoleUtil.DisplayMessage(sb.ToString());
            Console.WriteLine();

            sb.Clear();
            sb.AppendFormat("Your first task will be to set up your account details.");
            ConsoleUtil.DisplayMessage(sb.ToString());

            DisplayContinuePrompt();
        }

        public void DisplayGameArea()
        {
            ConsoleUtil.HeaderText = "Current Game Board";
            ConsoleUtil.DisplayReset();

            DisplayGameboard();
            DisplayGameStatus();
        }

        public void DisplayCurrentGameStatus(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            ConsoleUtil.HeaderText = "Current Game Status";
            ConsoleUtil.DisplayReset();

            double playerXPercentageWins = (double)playerXWins / roundsPlayed;
            double playerOPercentageWins = (double)playerOWins / roundsPlayed;
            double percentageOfCatsGames = (double)catsGames / roundsPlayed;

            ConsoleUtil.DisplayMessage("Rounds Played: " + roundsPlayed);
            ConsoleUtil.DisplayMessage("Rounds for Player X: " + playerXWins + " - " + String.Format("{0:P2}", playerXPercentageWins));
            ConsoleUtil.DisplayMessage("Rounds for Player O: " + playerOWins + " - " + String.Format("{0:P2}", playerOPercentageWins));
            ConsoleUtil.DisplayMessage("Cat's Games: " + catsGames + " - " + String.Format("{0:P2}", percentageOfCatsGames));

            DisplayContinuePrompt();
        }

        public bool DisplayNewRoundPrompt()
        {
            ConsoleUtil.HeaderText = "Continue or Quit";
            ConsoleUtil.DisplayReset();

            return DisplayGetYesNoPrompt("Would you like to play another round?");
        }

        public void DisplayGameStatus()
        {
            StringBuilder sb = new StringBuilder();

            switch (_gameboard.CurrentRoundState)
            {
                case Gameboard.GameboardState.NewRound:
                    //
                    // The new game status should not be an necessary option here
                    //
                    break;
                case Gameboard.GameboardState.PlayerXTurn:
                    DisplayMessageBox("It is currently Player X's turn.");
                    break;
                case Gameboard.GameboardState.PlayerOTurn:
                    DisplayMessageBox("It is currently Player O's turn.");
                    break;
                case Gameboard.GameboardState.PlayerXWin:
                    DisplayMessageBox("Player X Wins! Press any key to continue.");

                    Console.CursorVisible = false;
                    Console.ReadKey();
                    Console.CursorVisible = true;
                    break;
                case Gameboard.GameboardState.PlayerOWin:
                    DisplayMessageBox("Player O Wins! Press any key to continue.");

                    Console.CursorVisible = false;
                    Console.ReadKey();
                    Console.CursorVisible = true;
                    break;
                case Gameboard.GameboardState.CatsGame:
                    DisplayMessageBox("Cat's Game! Press any key to continue.");

                    Console.CursorVisible = false;
                    Console.ReadKey();
                    Console.CursorVisible = true;
                    break;
                default:
                    break;
            }
        }

        public void DisplayMessageBox(string message)
        {
            string leftMargin = new String(' ', ConsoleConfig.displayHorizontalMargin);
            string topBottom = new String('*', ConsoleConfig.windowWidth - 2 * ConsoleConfig.displayHorizontalMargin);

            StringBuilder sb = new StringBuilder();

            Console.SetCursorPosition(0, MESSAGEBOX_VERTICAL_LOCATION);
            Console.WriteLine(leftMargin + topBottom);

            Console.WriteLine(ConsoleUtil.Center("Game Status"));

            ConsoleUtil.DisplayMessage(message);

            Console.WriteLine(Environment.NewLine + leftMargin + topBottom);
        }

        /// <summary>
        /// display the current game board
        /// </summary>
        private void DisplayGameboard()
        {
            //
            // move cursor below header
            //
            Console.SetCursorPosition(0, GAMEBOARD_VERTICAL_LOCATION);

            Console.Write("\t\t\t        |---+---+---+---+---+---+---|\n");

            for (int i = 0; i < _gameboard.MaxNumOfRows; i++)
            {
                Console.Write("\t\t\t        | ");

                for (int j = 0; j < _gameboard.MaxNumOfColumns; j++)
                {
                    if (_gameboard.PositionState[i, j] == Gameboard.PlayerPiece.None)
                    {
                        Console.Write(" " + " | ");
                    }
                    else
                    {
                        Console.Write(_gameboard.PositionState[i, j] + " | ");
                    }

                }

                Console.Write("\n\t\t\t        |---+---+---+---+---+---+---|\n");
            }

        }

        /// <summary>
        /// display prompt for a player's next move
        /// </summary>
        /// <param name="coordinateType"></param>
        private void DisplayPositionPrompt()
        {
            //
            // Clear line by overwriting with spaces
            //
            Console.SetCursorPosition(POSITIONPROMPT_HORIZONTAL_LOCATION, POSITIONPROMPT_VERTICAL_LOCATION);
            Console.Write(new String(' ', ConsoleConfig.windowWidth));
            //
            // Write new prompt
            //
            Console.SetCursorPosition(POSITIONPROMPT_HORIZONTAL_LOCATION, POSITIONPROMPT_VERTICAL_LOCATION);
            Console.Write("Enter column number: ");
        }

        /// <summary>
        /// Display a Yes or No prompt with a message
        /// </summary>
        /// <param name="promptMessage">prompt message</param>
        /// <returns>bool where true = yes</returns>
        private bool DisplayGetYesNoPrompt(string promptMessage)
        {
            bool yesNoChoice = false;
            bool validResponse = false;
            string userResponse;

            while (!validResponse)
            {
                ConsoleUtil.DisplayReset();

                ConsoleUtil.DisplayPromptMessage(promptMessage + "(yes/no)");
                userResponse = Console.ReadLine();

                if (userResponse.ToUpper() == "YES")
                {
                    yesNoChoice = true;
                    validResponse = true;
                }
                else if (userResponse.ToUpper() == "NO")
                {
                    yesNoChoice = false;
                    validResponse = true;
                }
                else
                {
                    ConsoleUtil.DisplayMessage(
                        "It appears that you have entered an incorrect response." +
                        " Please enter either \"yes\" or \"no\"."
                        );
                    DisplayContinuePrompt();
                }
            }

            return yesNoChoice;
        }

        /// <summary>
        /// Get a player's position choice within the correct range of the array
        /// Note: The ConsoleView is allowed access to the GameboardPosition struct.
        /// </summary>
        /// <returns>GameboardPosition</returns>
        public GameboardPosition GetPlayerPositionChoice()
        {
            //
            // Initialize gameboardPosition with -1 values
            //
            GameboardPosition gameboardPosition = new GameboardPosition(-1, -1);

            //
            // Get column number.
            //
            if (CurrentViewState != ViewState.PlayerUsedMaxAttempts)
            {
                gameboardPosition.Column = PlayerCoordinateChoice();
            }

            return gameboardPosition;

        }

        /// <summary>
        /// Validate the player's coordinate response for integer and range
        /// </summary>
        /// <param name="coordinateType">an integer value within proper range or -1</param>
        /// <returns></returns>
        private int PlayerCoordinateChoice()
        {
            int tempCoordinate = -1;
            int numOfPlayerAttempts = 1;
            int maxNumOfPlayerAttempts = 4;

            while ((numOfPlayerAttempts <= maxNumOfPlayerAttempts))
            {
                DisplayPositionPrompt();

                if (int.TryParse(Console.ReadLine(), out tempCoordinate))
                {
                    //
                    // Player response within range
                    // Temporary: Make sure that the column is available
                    if (tempCoordinate >= 1 && tempCoordinate <= _gameboard.MaxNumOfColumns && _gameboard.GameboardColumnAvailable(tempCoordinate - 1))
                    {
                        return tempCoordinate;
                    }
                    //
                    // Player response out of range
                    //
                    else
                    {
                        DisplayMessageBox("Column numbers are limited to (1,2,3)");
                    }
                }
                //
                // Player response cannot be parsed as integer
                //
                else
                {
                    DisplayMessageBox("Column numbers are limited to (1,2,3)");
                }

                //
                // Increment the number of player attempts
                //
                numOfPlayerAttempts++;
            }

            //
            // Player used maximum number of attempts, set view state and return
            //
            CurrentViewState = ViewState.PlayerUsedMaxAttempts;
            return tempCoordinate;
        }

        public OpeningMenuOption DisplayOpeningMenu()
        {
            int userChoice = 0;
            OpeningMenuOption openingMenuOption = OpeningMenuOption.None;

            ConsoleUtil.HeaderText = "Welcome To Connect Four!";
            ConsoleUtil.DisplayReset();
            ConsoleUtil.DisplayMessage("Please Select a Following Option:");

            //
            // loop through enum values and build out main menu
            //
            foreach (OpeningMenuOption option in Enum.GetValues(typeof(OpeningMenuOption)))
            {
                if (option != OpeningMenuOption.None)
                {
                    Console.WriteLine($"\t{(int)option}. {option}");
                }
            }

            Console.WriteLine();

            //
            // checks for a valid integer and gives userfeedback
            //

            while ((!int.TryParse(Console.ReadLine(), out userChoice)) | userChoice > ((Enum.GetNames(typeof(OpeningMenuOption)).Length) - 1) | userChoice <= 0)
            {
                if (userChoice == 0)
                {
                    Console.WriteLine("Please enter a valid integer!");
                }
                else
                {
                    Console.WriteLine($"{userChoice} is not within the range of 1 and {(Enum.GetNames(typeof(OpeningMenuOption)).Length) - 1}!\nPlease enter a valid integer!");
                }
            }

            openingMenuOption = (OpeningMenuOption)userChoice;
            return openingMenuOption;
        }

        public MainMenuOption DisplayMainMenu()
        {
            MainMenuOption mainMenuOption = MainMenuOption.None;
            int userChoice = 0;

            ConsoleUtil.HeaderText = "Main Menu";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Please Select an Option Below!");
            Console.WriteLine();

            //
            // loop through enum values and build out main menu
            //
            foreach(MainMenuOption option in Enum.GetValues(typeof(MainMenuOption)))
            {
                if(option != MainMenuOption.None)
                {
                 Console.WriteLine($"\t{(int)option}. {option}");
                }
            }

            //
            //get user choice
            //

            while ((!int.TryParse(Console.ReadLine(), out userChoice)) | userChoice > ((Enum.GetNames(typeof(MainMenuOption)).Length) - 1) | userChoice <= 0)
            {   
                if(userChoice == 0)
                {
                    Console.WriteLine("Please enter a valid integer!");
                }
                else
                {
                    Console.WriteLine($"{userChoice} is not within the range of 1 and {(Enum.GetNames(typeof(MainMenuOption)).Length) - 1}!\nPlease enter a valid integer!");
                }
            }

            mainMenuOption = (MainMenuOption)userChoice;

            Console.WriteLine($"You have selected to {mainMenuOption.ToString()}");
            DisplayContinuePrompt();
            return mainMenuOption;
        }
        #endregion
    }
}
