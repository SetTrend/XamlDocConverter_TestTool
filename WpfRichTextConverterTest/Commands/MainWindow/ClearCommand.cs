using System.Windows.Input;

namespace WpfRichTextConverterTest.Commands.MainWindow
{
	public class ClearCommand : RoutedCommand
	{
		public ClearCommand() => InputGestures.Add(new KeyGesture(Key.Delete, ModifierKeys.Shift | ModifierKeys.Control));
	}
}
