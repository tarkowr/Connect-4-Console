## Tic-Tac-Toe Version 2

**Authors:** Jacob Barsheff, Wyatt Miller, and Richie Tarkowski

**Date Created:** 8/30/2018

**Course:** CIT 255

**Overview:** This application allows two users to play a simple Connect Four game using the console UI. The purpose of the application is to demonstrate encapsulation and the MVC design pattern.

##

### Application Workflow

1. Splash Screen:

        Display:
        a. Game Title 
        b. Introduction to Game

        Options:
        a. Main Menu
        b. Quit
            
2. Main Menu:

        Options:
        a. Play a Round
        b. View Game Rules
        c. View Current Game Stats
        d. View Historic Game Stats
        e. Save Game Results
        f. Quit
        
3. View Game Rules:

        Display:
        a. Game Rules
        
        Options:
        a. Return to Main Menu

4. Play a Round:

        Display:
        a. Prompt the Player on their Turn to Make a Move.
        b. Game Board that Reflects all Round Events.
        c. A Column Picker that Hovers over Columns and is Controllable with Arrow Keys.
        
        Functionality:
        a. Select a Player to Start the Game or let the Computer pick.
        b. Players can see a Visual Representation of the Game Board (Connect Four) and will be Prompted to make a Move.
        c. Able to Drop a Game Piece into the Desired Column via Arrow Keys.
            i. Column Picker will skip over columns that are full.
            ii. Column Picker will loop from end column to start and vice versa.
        d. Players Alternate between Moves, unless a Game-Winning move is made or no more Spots are left.
        e. If a Player Connects Four or more Pieces in a Row, they will Win the Game.
        f. If no more Spots are Available on the Game Board, the Game Ends as a Tie.
        g. Players can Exit the Round at any Point in the Round.
        h. If the Game Ends (Round is Won, Tied, or Exited), the Player is taken to the Game Status Screen.
        
5. Game Stats Screen:

        Display:
        a. Who won the Previous Round
        b. Player One Wins
        c. Player Two Wins
        d. Ties
        
        Options:
        a. Return to Main Menu
        
6. Historic Game Stats Screen:

        Display:
        a. Player One Total Wins
        b. Player Two Total Wins
        c. Total Ties
        
        Options
        a. Return to Main Menu
        
7. Save Game Results:

        Display:
        a. Confirmation that the Data was Saved
        b. The Data that was Saved
        
        Options:
        a. Return to Main Menu
        
8. Quit:

        Display:
        a. Closing Screen

        
     