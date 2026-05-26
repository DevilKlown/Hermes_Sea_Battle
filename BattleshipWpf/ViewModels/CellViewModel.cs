using System.ComponentModel;
using BattleshipWpf.Models;

namespace BattleshipWpf.ViewModels
{
    public class CellViewModel : INotifyPropertyChanged
    {
        public Cell Model { get; }
        public CellViewModel(Cell cell) => Model = cell;

        public bool IsHit
        {
            get => Model.IsHit;
            set { Model.State = value ? CellState.Hit : Model.State; OnPropertyChanged(nameof(IsHit)); }
        }

        public bool IsMiss
        {
            get => Model.IsMiss;
            set { Model.State = value ? CellState.Miss : Model.State; OnPropertyChanged(nameof(IsMiss)); }
        }

        public string DisplaySymbol => Model.State switch
        {
            CellState.Hit => "✕",
            CellState.Miss => "·",
            _ => string.Empty
        };

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
