using System.Linq;
using BattleshipWpf.Core;
using Xunit;

namespace BattleshipWpf.Tests
{
    public class GameEngineTests
    {
        [Fact]
        public void Reset_InitializesCorrectNumberOfShips()
        {
            var engine = new GameEngine();
            Assert.Equal(5, engine.Ships.Count);
        }

        [Fact]
        public void Attack_OnShipCell_ReturnsHit()
        {
            var engine = new GameEngine();
            var shipCell = engine.Cells.First(c => c.HasShip);
            var result = engine.Attack(shipCell.X, shipCell.Y);
            Assert.Equal(AttackResult.Hit, result);
            Assert.True(shipCell.IsHit);
            Assert.Equal("Hit!", engine.StatusMessage);
        }

        [Fact]
        public void Attack_OnEmptyCell_ReturnsMiss()
        {
            var engine = new GameEngine();
            var emptyCell = engine.Cells.First(c => !c.HasShip);
            var result = engine.Attack(emptyCell.X, emptyCell.Y);
            Assert.Equal(AttackResult.Miss, result);
            Assert.True(emptyCell.IsMiss);
            Assert.Equal("Miss.", engine.StatusMessage);
        }
    }
}
