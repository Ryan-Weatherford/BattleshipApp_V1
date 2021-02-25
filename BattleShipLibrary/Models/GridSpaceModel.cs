using System;
using System.Collections.Generic;
using System.Text;
using BattleShipLibrary.Data;

namespace BattleShipLibrary.Models
{
    public class GridSpaceModel
    {
        public string GridLetter { get; set; }
        public int GridNumber { get; set; }
        public GridStatus GridSpaceStatus { get; set; } = GridStatus.empty;

        public string GridLabel
        {
            get
            {
                return $"{GridLetter}{GridNumber}";
            }
        }
    }
}
