using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using BattleshipWpf.Models;
using BattleshipWpf.Core;

namespace BattleshipWpf.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
        private const int BoardSize = 10;
        public ObservableCollection<CellViewModel> OpponentCells { get; } = new ObservableCollection<CellViewModel>();
        private readonly GameEngine _engine = new GameEngine();

        private string _statusMessage = "Click a cell to fire.";
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }

        public ICommand AttackCommand { get; }
        public ICommand ResetCommand { get; }

        public GameViewModel()
        {
            AttackCommand = new RelayCommand(ExecuteAttack, CanAttack);
            ResetCommand = new RelayCommand(_ => ResetGame());
            ResetGame();
        }

        private bool CanAttack(object? parameter) => parameter is CellViewModel cvm && !cvm.IsHit && !cvm.IsMiss;

        private void ExecuteAttack(object? parameter)
        {
            if (parameter is not CellViewModel cvm) return;
            var cell = cvm.Model;
            var result = _engine.Attack(cell.X, cell.Y);
            switch (result)
            {
                case AttackResult.Hit:
                    cvm.IsHit = true;
                    break;
                case AttackResult.Miss:
                    cvm.IsMiss = true;
                    break;
                case AttackResult.AlreadyAttacked:
                    // ignore – UI already reflects state
                    break;
            }
            // Sync status message from engine
            StatusMessage = _engine.StatusMessage;
        }

        private void ResetGame()
        {
            _engine.Reset();
            OpponentCells.Clear();
            foreach (var cell in _engine.Cells)
            {
                OpponentCells.Add(new CellViewModel(cell));
            }
            // Ensure UI reflects initial status
            StatusMessage = _engine.StatusMessage;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
