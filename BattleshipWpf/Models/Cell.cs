namespace BattleshipWpf.Models
{
    public enum CellState
    {
        Empty,
        Ship,
        Hit,
        Miss
    }

    public class Cell
    {
        public int X { get; }
        public int Y { get; }
        public CellState State { get; set; } = CellState.Empty;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool HasShip => State == CellState.Ship;
        public bool IsHit => State == CellState.Hit;
        public bool IsMiss => State == CellState.Miss;
    }
}
