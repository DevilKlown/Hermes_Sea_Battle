namespace BattleshipWpf.Models
{
    using System.Collections.Generic;

    public class Ship
    {
        public List<Cell> Cells { get; } = new List<Cell>();
        public bool IsSunk => Cells.TrueForAll(c => c.IsHit);
    }
}
