using System.Windows.Input;

namespace WpfRichTextConverterTest.Commands.MainWindow
{
	public class QuitCommand : RoutedCommand
	{
		public QuitCommand() => InputGestures.Add(new KeyGesture(Key.F4, ModifierKeys.Alt));
	}
}
