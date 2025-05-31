using System.Windows;

using AxDa.XamlDocConverter.Abstractions;

namespace WpfRichTextConverterTest.WpfWindows.OptionsWindow;

public partial class OptionsWin : Window
{
	public XamlDocConverterOptions Options { get; set; }



	public OptionsWin(XamlDocConverterOptions options)
	{
		Options = options;

		InitializeComponent();

		ImgEnforceWs.ToolTip = @"A peculiarity of the WPF parser requires the ""xml:space=preserve"" XML attribute in the root element.

If this attribute is not present in the XML root element, the WPF renderer may not display words with the expected word break.

However, this requirement typically results in the resulting XML being output as a single, long line. For troubleshooting purposes,
you can disable this option. The resulting XML will then be output with normal indentation. However, the result will not be
WPF-compatible if this option is disabled.";
	}



	private void BtnOk_Click(object sender, RoutedEventArgs e)
	{
		Options.EnforceWSPreserve = ChkEnforceWs.IsChecked ?? false;

		DialogResult = true;
		Close();
	}

	private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
}
