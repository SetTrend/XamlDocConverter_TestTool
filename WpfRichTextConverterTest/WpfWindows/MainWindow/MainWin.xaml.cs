using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using AxDa.XamlDocConverter.Html;
using AxDa.XamlDocConverter.Markdown;

using WpfRichTextConverterTest.WpfWindows.AboutWin;
using WpfRichTextConverterTest.WpfWindows.OptionsWindow;

namespace WpfRichTextConverterTest.WpfWindows.MainWindow
{
	public partial class MainWin : Window
	{
		private readonly HtmlXamlDocConverterOptions _htmlOptions = new HtmlXamlDocConverterOptions();
		private readonly MarkdownXamlDocConverterOptions _mdOptions = new MarkdownXamlDocConverterOptions();



		public string HtmlTitle { get; set; } = "Test";

		public string YfmtText { get; set; } = "Test";




		private string? YfmtTextParameter => MdWithYfmt.IsChecked ?? false ? YfmtText : null;


		public MainWin() => InitializeComponent();



		private string TextRangeContent
		{
			get
			{
				using MemoryStream ms = new MemoryStream(TextConverter.InitialMemoryStreamCapacity);

				new TextRange(RtBox.Document.ContentStart, RtBox.Document.ContentEnd).Save(ms, DataFormats.Xaml);
				ms.Position = 0L;

				return new StreamReader(ms).ReadToEnd();
			}

			set
			{
				using MemoryStream ms = new MemoryStream(TextConverter.InitialMemoryStreamCapacity);
				using StreamWriter writer = new StreamWriter(ms);

				writer.Write(value);
				writer.Flush();
				ms.Position = 0L;

				new TextRange(RtBox.Document.ContentStart, RtBox.Document.ContentEnd).Load(ms, DataFormats.Xaml);
			}
		}



		private void AddErrorMessage(string title, Exception ex)
		{
			string text = title + ": " + (ex.InnerException?.Message ?? ex.Message);

			if (string.IsNullOrWhiteSpace(ErrorList.Text)) ErrorList.Text = text;
			else ErrorList.Text += Environment.NewLine + text;

			ErrorScoll.ScrollToBottom();
		}


		private void RichTextChanged()
		{
			string xaml = TextRangeContent;

			XamlBox.Text = xaml;

			try { HtmlBox.Text = TextConverter.XamlToHtml(xaml, _htmlOptions, HtmlTitle); } catch (Exception ex) { AddErrorMessage("XAML->HTML", ex); }
			try { MdBox.Text = TextConverter.XamlToMarkdown(xaml, _mdOptions, YfmtTextParameter); } catch (Exception ex) { AddErrorMessage("XAML->Markdown", ex); }
		}

		private void XamlTextChanged()
		{
			try
			{
				string xaml = XamlBox.Text;

				TextRangeContent = xaml;

				try { HtmlBox.Text = TextConverter.XamlToHtml(xaml, _htmlOptions, HtmlTitle); } catch (Exception ex) { AddErrorMessage("XAML->HTML", ex); }
				try { MdBox.Text = TextConverter.XamlToMarkdown(xaml, _mdOptions, YfmtTextParameter); } catch (Exception ex) { AddErrorMessage("XAML->Markdown", ex); }
			}
			catch (Exception ex) { AddErrorMessage("XAML->XAML", ex); }
		}

		private void HtmlTextChanged()
		{
			string html = HtmlBox.Text;

			try
			{
				string xaml = TextConverter.HtmlToXaml(html, _htmlOptions);

				TextRangeContent = xaml;
				XamlBox.Text = xaml;
				try { MdBox.Text = TextConverter.XamlToMarkdown(xaml, _mdOptions, YfmtTextParameter); } catch (Exception ex) { AddErrorMessage("XAML->Markdown", ex); }
			}
			catch (Exception ex) { AddErrorMessage("HTML->XAML", ex); }
		}

		private void MdTextChanged()
		{
			string markdown = MdBox.Text;

			try
			{
				string xaml = TextConverter.MarkdownToXaml(markdown, _mdOptions);

				TextRangeContent = xaml;
				XamlBox.Text = xaml;
				try { HtmlBox.Text = TextConverter.XamlToHtml(xaml, _htmlOptions, HtmlTitle); } catch (Exception ex) { AddErrorMessage("XAML->HTML", ex); }
			}
			catch (Exception ex) { AddErrorMessage("Markdown->XAML", ex); }
		}

		private void HtmlOptionsChanged(bool? isFragment)
		{
			if (isFragment.HasValue)
			{
				HtmlTitleText.Visibility = isFragment.Value ? Visibility.Hidden : Visibility.Visible;
				_htmlOptions.AsDocumentFragment = isFragment.Value;
			}

			RichTextChanged();
		}

		private void MdOptionsChanged(bool? showYfmt)
		{
			if (showYfmt.HasValue)
				MdYfmtText.Visibility = showYfmt.Value ? Visibility.Visible : Visibility.Hidden;

			RichTextChanged();
		}



		private void BoldBtn_Click(object sender, RoutedEventArgs e) => RtBox.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);

		private void ItalicBtn_Click(object sender, RoutedEventArgs e) => RtBox.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);

		private void ClearErrors_Click(object sender, RoutedEventArgs e) => ErrorList.Text = "";


		private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (RtBox.IsFocused) RichTextChanged();
		}

		private void XamlBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (XamlBox.IsFocused) XamlTextChanged();
		}

		private void HtmlBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (HtmlBox.IsFocused) HtmlTextChanged();
		}

		private void MdBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (MdBox.IsFocused) MdTextChanged();
		}


		private void ClearCmd_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			ClearErrors.Focus();

			RtBox.Document.Blocks.Clear();
			XamlBox.Clear();
			HtmlBox.Clear();
			MdBox.Clear();
		}

		private void QuitCmd_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) => Close();

		private void OptionsCmd_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			OptionsWin optionsWin = new OptionsWin(_htmlOptions) { Owner = this };

			if (optionsWin.ShowDialog() ?? false) _mdOptions.EnforceWSPreserve = _htmlOptions.EnforceWSPreserve;
		}

		private void AboutCmd_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) => new AboutBox() { Owner = this }.ShowDialog();


		private void HtmlDoc_Click(object sender, RoutedEventArgs e)
		{
			if (HtmlDocument.IsFocused) HtmlOptionsChanged(false);
		}

		private void HtmlFrag_Click(object sender, RoutedEventArgs e)
		{
			if (HtmlFragment.IsFocused) HtmlOptionsChanged(true);
		}

		private void HtmlTitle_Changed(object sender, TextChangedEventArgs e)
		{
			if (HtmlTitleText.IsFocused) HtmlOptionsChanged(null);
		}

		private void NoYfmt_Click(object sender, RoutedEventArgs e)
		{
			if (MdNoYfmt.IsFocused) MdOptionsChanged(false);
		}

		private void WithYfmt_Click(object sender, RoutedEventArgs e)
		{
			if (MdWithYfmt.IsFocused) MdOptionsChanged(true);
		}

		private void MdYfmt_Changed(object sender, TextChangedEventArgs e)
		{
			if (MdYfmtText.IsFocused) MdOptionsChanged(null);
		}
	}
}