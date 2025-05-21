using System.IO;

using AxDa.XamlDocConverter.Html;
using AxDa.XamlDocConverter.Markdown;

namespace WpfRichTextConverterTest;

internal class TextConverter
{
	internal const int InitialMemoryStreamCapacity = 4_096;



	internal static string XamlToHtml(string xaml, HtmlXamlDocConverterOptions htmlOptions, string title)
	{
		using MemoryStream xamlStream = new MemoryStream(InitialMemoryStreamCapacity);
		using StreamWriter writer = new StreamWriter(xamlStream);

		writer.Write(xaml);
		writer.Flush();
		xamlStream.Position = 0L;

		using MemoryStream htmlStream = new MemoryStream(InitialMemoryStreamCapacity);

		new HtmlXamlDocConverter(htmlOptions).XamlToHtml(xamlStream, htmlStream, title);
		htmlStream.Position = 0L;

		return new StreamReader(htmlStream).ReadToEnd();
	}

	internal static string HtmlToXaml(string html, HtmlXamlDocConverterOptions htmlOptions)
	{
		using MemoryStream htmlStream = new MemoryStream(InitialMemoryStreamCapacity);
		using StreamWriter writer = new StreamWriter(htmlStream);

		writer.Write(html);
		writer.Flush();
		htmlStream.Position = 0L;

		using MemoryStream xamlStream = new MemoryStream(InitialMemoryStreamCapacity);

		new HtmlXamlDocConverter(htmlOptions).HtmlToXaml(htmlStream, xamlStream);
		xamlStream.Position = 0L;

		return new StreamReader(xamlStream).ReadToEnd();
	}

	internal static string XamlToMarkdown(string xaml, MarkdownXamlDocConverterOptions mdOption, string? yfmt)
	{
		using MemoryStream xamlStream = new MemoryStream(InitialMemoryStreamCapacity);
		using StreamWriter writer = new StreamWriter(xamlStream);

		writer.Write(xaml);
		writer.Flush();
		xamlStream.Position = 0L;

		using MemoryStream mdStream = new MemoryStream(InitialMemoryStreamCapacity);

		new MarkdownXamlDocConverter(mdOption).XamlToMarkdown(xamlStream, mdStream, yfmt);
		mdStream.Position = 0L;

		return new StreamReader(mdStream).ReadToEnd();
	}

	internal static string MarkdownToXaml(string markdown, MarkdownXamlDocConverterOptions mdOption)
	{
		using MemoryStream mdStream = new MemoryStream(InitialMemoryStreamCapacity);
		using StreamWriter writer = new StreamWriter(mdStream);

		writer.Write(markdown);
		writer.Flush();
		mdStream.Position = 0L;

		using MemoryStream xamlStream = new MemoryStream(InitialMemoryStreamCapacity);

		new MarkdownXamlDocConverter(mdOption).MarkdownToXaml(mdStream, xamlStream);
		xamlStream.Position = 0L;

		return new StreamReader(xamlStream).ReadToEnd();
	}
}
