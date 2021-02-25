using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShipLibrary.Data;
using BattleShipLibrary.Models;

namespace BattleShipConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleMethods.StartGame();
            PlayerModel activePlayer = ConsoleMethods.CreatePlayer("Player 1");
            PlayerModel opponentPlayer = ConsoleMethods.CreatePlayer("Player 2");
            PlayerModel winner = null;

            do
            {
                ConsoleMethods.RecordPlayerShot(activePlayer, opponentPlayer);
                bool gameContinue = GameLogic.PlayerStillActive(opponentPlayer);

                if (gameContinue == true)
                {
                    PlayerModel tempHolder = opponentPlayer;
                    opponentPlayer = activePlayer;
                    activePlayer = tempHolder;

                }
                else
                {
                    winner = activePlayer;
                }

            } while (winner == null);

            ConsoleMethods.IdentifyWinner(winner);
            ConsoleMethods.EndGame();
        }
    }
}
