using System;
using System.Collections.Generic;
using System.Linq;
using BattleshipWpf.Models;

namespace BattleshipWpf.Core
{
    public enum AttackResult
    {
        Hit,
        Miss,
        AlreadyAttacked
    }

    public class GameEngine
    {
        private const int BoardSize = 10;
        private readonly Cell[,] _board = new Cell[BoardSize, BoardSize];
        private readonly List<Ship> _ships = new List<Ship>();
        private readonly Random _rnd = new Random();
        private readonly int[] _shipLengths = new[] {5,4,3,3,2};

        public IEnumerable<Cell> Cells => _board.Cast<Cell>();
        public IReadOnlyCollection<Ship> Ships => _ships.AsReadOnly();
        public string StatusMessage { get; private set; } = "New game – fire!";

        public GameEngine()
        {
            Reset();
        }

        public void Reset()
        {
            // initialise cells
            for (int y = 0; y < BoardSize; y++)
                for (int x = 0; x < BoardSize; x++)
                    _board[x, y] = new Cell(x, y);

            _ships.Clear();
            // place ships
            foreach (var length in _shipLengths)
            {
                bool placed = false;
                while (!placed)
                {
                    bool horizontal = _rnd.Next(2) == 0;
                    int startX = _rnd.Next(BoardSize - (horizontal ? length : 0));
                    int startY = _rnd.Next(BoardSize - (horizontal ? 0 : length));
                    // collision check
                    bool collision = false;
                    for (int i = 0; i < length; i++)
                    {
                        int x = startX + (horizontal ? i : 0);
                        int y = startY + (horizontal ? 0 : i);
                        if (_board[x, y].HasShip) { collision = true; break; }
                    }
                    if (collision) continue;
                    var ship = new Ship();
                    for (int i = 0; i < length; i++)
                    {
                        int x = startX + (horizontal ? i : 0);
                        int y = startY + (horizontal ? 0 : i);
                        var cell = _board[x, y];
                        cell.State = CellState.Ship;
                        ship.Cells.Add(cell);
                    }
                    _ships.Add(ship);
                    placed = true;
                }
            }
            StatusMessage = "New game – fire!";
        }

        public AttackResult Attack(int x, int y)
        {
            if (x < 0 || x >= BoardSize || y < 0 || y >= BoardSize)
                throw new ArgumentOutOfRangeException("Coordinates out of board");

            var cell = _board[x, y];
            if (cell.IsHit || cell.IsMiss)
                return AttackResult.AlreadyAttacked;

            if (cell.HasShip)
            {
                cell.State = CellState.Hit;
                StatusMessage = "Hit!";
                var ship = _ships.First(s => s.Cells.Contains(cell));
                if (ship.IsSunk)
                    StatusMessage = "You sunk a ship!";
                if (_ships.All(s => s.IsSunk))
                    StatusMessage = "All enemy ships sunk – you win!";
                return AttackResult.Hit;
            }
            else
            {
                cell.State = CellState.Miss;
                StatusMessage = "Miss.";
                return AttackResult.Miss;
            }
        }
    }
}
