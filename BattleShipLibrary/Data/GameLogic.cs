using BattleShipLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipLibrary.Data
{
    public static class GameLogic
    {
        public static void InitializeShotGrid (PlayerModel player)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach (string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddShotGridSpace(player, letter, number);
                }
            }
        }

        public static void InitializeShipGrid(PlayerModel player)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach (string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddShipGridSpace(player, letter, number);
                }
            }
        }

        private static void AddShotGridSpace (PlayerModel player, string letter, int number)
        {
            GridSpaceModel gridSpace = new GridSpaceModel
            {
                GridLetter = letter,
                GridNumber = number,
                GridSpaceStatus = GridStatus.empty
            };

            player.ShotGrid.Add(gridSpace);
        }

        private static void AddShipGridSpace(PlayerModel player, string letter, int number)
        {
            GridSpaceModel gridSpace = new GridSpaceModel
            {
                GridLetter = letter,
                GridNumber = number,
                GridSpaceStatus = GridStatus.empty
            };

            player.ShipLocations.Add(gridSpace);
        }

        public static bool PlayerStillActive(PlayerModel player)
        {
            bool isActive = false;

            foreach (var ships in player.ShipLocations)
            {
                if (ships.GridSpaceStatus == GridStatus.ship)
                {
                    isActive = true;
                }
            }

            return isActive;
        }

        public static bool PlaceShipBool(PlayerModel player, string location)
        {
            bool output = false;

            foreach (var gridSpace in player.ShipLocations)
            {
                if (location.ToUpper() == gridSpace.GridLabel && gridSpace.GridSpaceStatus == GridStatus.empty)
                {
                    output = true;
                    gridSpace.GridSpaceStatus = GridStatus.ship;
                }
            }

            return output;
        }
    }
}
