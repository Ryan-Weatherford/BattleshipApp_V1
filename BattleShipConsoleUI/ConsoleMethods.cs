using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShipLibrary.Models;
using System.Globalization;
using BattleShipLibrary.Data;

namespace BattleShipConsoleUI
{
    public static class ConsoleMethods
    {
        public static void StartGame()
        {
            Console.WriteLine("Battleship App \n" +
            "by Ryan Weatherford");
            Console.WriteLine();
            Console.WriteLine("This is a Battleship style game. Two competing players will have 5 ships, \n" +
                "each occupying one grid space. You each will select grid spaces to place your ships. \n" +
                "Then you will take turns guessing grid spaces and sinking ships.");
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
            Console.Clear();
        }

        public static void RecordPlayerShot(PlayerModel activePlayer, PlayerModel opponentPlayer)
        {
            (bool, string) isValidShot = (false, "");

            do
            {
                Console.WriteLine($"{activePlayer.PlayerName}'s turn:");
                Console.WriteLine();
                ConsoleMethods.DisplayShotGrid(activePlayer);
                isValidShot = GetValidatedShotInput(activePlayer, opponentPlayer);

                if (isValidShot.Item1 == false)
                {
                    Console.Beep();
                    Console.WriteLine();
                    Console.WriteLine(isValidShot.Item2);
                    Console.ReadLine();
                    Console.Clear();
                }
                
            } while (isValidShot.Item1 == false);

            
            string playerShot = isValidShot.Item2;

            string shotResult = "";

            foreach (GridSpaceModel opponentSpace in opponentPlayer.ShipLocations)
            {
                if (playerShot.ToUpper() == opponentSpace.GridLabel)
                {
                    if (opponentSpace.GridSpaceStatus == GridStatus.ship)
                    {
                        opponentSpace.GridSpaceStatus = GridStatus.sunk;
                        shotResult = "hit";
                    }
                    else if (opponentSpace.GridSpaceStatus == GridStatus.empty)
                    {
                        shotResult = "miss";
                    }
                    else if (opponentSpace.GridSpaceStatus == GridStatus.sunk)
                    {
                        shotResult = "repeat shot";
                    }
                }
            }

            foreach (GridSpaceModel playerSpace in activePlayer.ShotGrid)
            {
                if (playerShot.ToUpper() == playerSpace.GridLabel)
                {
                    if (shotResult == "hit")
                    {
                        playerSpace.GridSpaceStatus = GridStatus.hit;
                    }
                    else if (shotResult == "miss")
                    {
                        playerSpace.GridSpaceStatus = GridStatus.miss;
                    }
                    else if (playerSpace.GridSpaceStatus == GridStatus.miss)
                    {
                        shotResult = "repeat shot";
                    }
                }
            }

            Console.WriteLine();

            if (shotResult == "hit")
            {
                Console.WriteLine("Hit!");
            }
            else if (shotResult == "miss")
            {
                Console.WriteLine("Miss");
            }
      
            Console.ReadLine();
            Console.Clear();
        }

        private static (bool, string) GetValidatedShotInput(PlayerModel activePlayer, PlayerModel opponentPlayer)
        {
            bool outputBool = false;
            string outputString;
            string inputValidity = "invalid";

            Console.WriteLine();
            outputString = GetConsoleInfo("Target grid space: ");

            foreach (GridSpaceModel opponentSpace in opponentPlayer.ShipLocations)
            {
                if (outputString.ToUpper() == opponentSpace.GridLabel)
                {
                    inputValidity = "valid";

                    if (opponentSpace.GridSpaceStatus == GridStatus.sunk)
                    {
                        inputValidity = "repeat shot";
                    }
                }
            }

            foreach (GridSpaceModel playerSpace in activePlayer.ShotGrid)
            {
                if (outputString.ToUpper() == playerSpace.GridLabel)
                {
                    if (playerSpace.GridSpaceStatus == GridStatus.miss)
                    {
                        inputValidity = "repeat shot";
                    }
                }
            }

            if (inputValidity == "valid")
            {
                outputBool = true;
            }
            else if (inputValidity == "invalid")
            {
                outputString = "Invalid shot input. Try again.";
            }
            else if (inputValidity == "repeat shot")
            {
                outputString = "You have already shot there. Try a new space.";
            }

            return (outputBool, outputString);
        }

        internal static void IdentifyWinner(PlayerModel winner)
        {
            Console.WriteLine($"{winner.PlayerName} wins!");
        }

        public static void DisplayShotGrid(PlayerModel activePlayer)
        {
            GridSpaceModel currentRow = activePlayer.ShotGrid[0];

            foreach (GridSpaceModel gridSpace in activePlayer.ShotGrid)
            {
                if (gridSpace.GridLetter != currentRow.GridLetter)
                {
                    Console.WriteLine();
                    currentRow = gridSpace;
                }

                if (gridSpace.GridSpaceStatus == GridStatus.empty)
                {
                    Console.Write($"{gridSpace.GridLetter}{gridSpace.GridNumber} ");
                }
                else if (gridSpace.GridSpaceStatus == GridStatus.hit)
                {
                    Console.Write(" X ");
                }
                else if (gridSpace.GridSpaceStatus == GridStatus.miss)
                {
                    Console.Write(" O ");
                }
                else
                {
                    Console.WriteLine("?");
                }
            }
            Console.WriteLine();
        }

        public static void DisplayShipGrid(PlayerModel activePlayer)
        {
            GridSpaceModel currentRow = activePlayer.ShipLocations[0];

            foreach (GridSpaceModel gridSpace in activePlayer.ShipLocations)
            {
                if (gridSpace.GridLetter != currentRow.GridLetter)
                {
                    Console.WriteLine();
                    currentRow = gridSpace;
                }

                if (gridSpace.GridSpaceStatus == GridStatus.empty)
                {
                    Console.Write($"{gridSpace.GridLetter}{gridSpace.GridNumber} ");
                }
                else if (gridSpace.GridSpaceStatus == GridStatus.ship)
                {
                    Console.Write(" O ");
                }
            }
            Console.WriteLine();
        }

        public static string GetConsoleInfo(string message)
        {
            Console.Write(message);
            string output = Console.ReadLine();
            return output;
        }

        public static PlayerModel CreatePlayer(string playerTitle)
        {
            PlayerModel output = new PlayerModel();
            Console.WriteLine($"{playerTitle} creation");
            Console.WriteLine();
            output.PlayerName = GetConsoleInfo("Player Name: ");
            Console.Clear();
            GameLogic.InitializeShipGrid(output);
            GameLogic.InitializeShotGrid(output);
            PlaceShips(output);
            return output;
        }

        private static void PlaceShips(PlayerModel activePlayer)
        {
            int shipLoopNumber = 1;
            bool isValidLocation = false;

            do
            {
                Console.WriteLine("Choose grid space locations for your ships: ");
                Console.WriteLine();
                DisplayShipGrid(activePlayer);
                Console.WriteLine();
                string location = GetConsoleInfo($"Ship #{shipLoopNumber}: ");
                isValidLocation = GameLogic.PlaceShipBool(activePlayer, location);

                while (isValidLocation == false)
                {
                    Console.Beep();
                    Console.WriteLine("Invalid ship location. Press Enter to try again.");
                    Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Choose grid space locations for your ships: ");
                    Console.WriteLine();
                    DisplayShipGrid(activePlayer);
                    Console.WriteLine();
                    location = GetConsoleInfo($"Ship #{shipLoopNumber}: ");
                    isValidLocation = GameLogic.PlaceShipBool(activePlayer, location);
                }

                shipLoopNumber++;
                Console.Clear();

            } while (shipLoopNumber < 6);

            Console.WriteLine($"{activePlayer.PlayerName}'s ship locations:");
            Console.WriteLine();
            DisplayShipGrid(activePlayer);
            Console.ReadLine();
            Console.Clear();
        }
        
        public static void EndGame()
        {
            Console.WriteLine();
            Console.WriteLine("End of Game.");
            Console.ReadLine();
        }

    }

}

