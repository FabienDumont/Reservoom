using System.Windows.Input;

namespace Reservoom.Commands;

public abstract class CommandBase : ICommand {
	public virtual bool CanExecute(object? parameter) {
		return true;
	}

	public abstract void Execute(object? parameter);

	public void OnCanExecuteChanged() {
		CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}

	public event EventHandler? CanExecuteChanged;
}