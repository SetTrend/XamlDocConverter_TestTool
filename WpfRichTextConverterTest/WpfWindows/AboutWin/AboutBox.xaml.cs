using System;
using System.IO;
using System.Windows;

namespace WpfRichTextConverterTest.WpfWindows.AboutWin;

public partial class AboutBox : Window
{
	public AboutBox()
	{
		InitializeComponent();

		BuiltTime.Text = $"Build time: {File.GetLastWriteTime(Environment.ProcessPath ?? "")}.";
	}

	private void OK_Click(object sender, RoutedEventArgs e) => Close();
}
