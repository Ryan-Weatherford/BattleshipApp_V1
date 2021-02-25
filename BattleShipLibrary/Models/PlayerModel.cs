using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipLibrary.Models
{
    public class PlayerModel
    {
        public string PlayerName { get; set; }
        public List<GridSpaceModel> ShotGrid { get; set; } = new List<GridSpaceModel>();
        public List<GridSpaceModel> ShipLocations { get; set; } = new List<GridSpaceModel>();
    }
}
