using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SorterKicadLibrary
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		List<string> LinesList = new List<string>();

		public MainWindow()
		{
			InitializeComponent();
			IList<string> lines;
			//lines = new List<string>() { "", "Source Content" };
			//RichTextBoxClearAndPopulate(SourceRichTextBox, lines, true);
			//lines = new List<string>() { "", "Sorted Content" };
			//RichTextBoxClearAndPopulate(TargetRichTextBox, lines, true);
			RichTextBoxClearAndPopulate(SourceRichTextBox, null, true);
						RichTextBoxClearAndPopulate(TargetRichTextBox, null, true);
		}

		private void SortItems(object sender, RoutedEventArgs e)
		{
			string SourceText = new TextRange(SourceRichTextBox.Document.ContentStart, SourceRichTextBox.Document.ContentEnd).Text;
			LinesList = SourceText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

			IList<string> header = GetHeader();
			IList<Item> itemsList = GetElements();
			IList<Item> sortedItemsList = itemsList.OrderBy(x => x.Name).ToList();
			IList<string> footer = GetFooter();


			var uniqueItemsCount = sortedItemsList.GroupBy(x => x.Name).ToList().Count;
			var duplicatedItemsList = sortedItemsList.ToList().GroupBy(x => x.Name).Where(y => y.Count() > 1).ToList();
			StatusLabel.Content = "Unique items count: " + uniqueItemsCount + ";";
			if (true)
			{
				StatusLabel.Content += " Duplicated items count: " + duplicatedItemsList.Count;
				StatusLabel.Foreground = new SolidColorBrush(Colors.Red);
			}

			RichTextBoxClearAndPopulate(TargetRichTextBox, header, true);
			foreach (var item in sortedItemsList)
			{
				RichTextBoxClearAndPopulate(TargetRichTextBox, item.Content);
			}
			RichTextBoxClearAndPopulate(TargetRichTextBox, footer);


			var info = "No duplicates";
			if (duplicatedItemsList.Count < 1)
			{
				info = "No duplicates";
			}
			else
			{
				info = "Library has " + duplicatedItemsList.Count + " duplicates:\n";
				foreach (var item in duplicatedItemsList)
				{
					info += item.ElementAt(0).Name + ", ";
				}
			}
			RichTextBoxClearAndPopulate(InfoRichTextBox, new List<string>() { info }, true);
			if (duplicatedItemsList.Count > 1) MessageBox.Show(info, "Warning!!!", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		/// <summary>
		/// Populate RichTextBox
		/// </summary>
		/// <param name="rtb">target rtb</param>
		/// <param name="lines">content: list strings</param>
		/// <param name="clear">is rtb needs to be cleared</param>
		private void RichTextBoxClearAndPopulate(RichTextBox rtb, IList<string> lines, bool clear = false)
		{
			if (lines == null)
			{
				lines = new List<string>();
			}
			if (clear) rtb.Document.Blocks.Clear();
			foreach (var line in lines)
			{
				rtb.Document.Blocks.Add(new Paragraph(new Run(line)));
			}
		}

		private IList<string> GetHeader()
		{
			List<string> result = new List<string>();
			foreach (var line in LinesList)
			{
				if (!line.StartsWith("DEF")) result.Add(line);
				else break;
			}
			result.RemoveRange(result.Count - 3, 3);
			RemoveReadedLines(result.Count);
			return result;
		}

		private IList<Item> GetElements()
		{
			List<Item> listItems = new List<Item>();
			Item item = new Item();
			int numberLinesToRemove = 0;
			foreach (var line in LinesList)
			{
				item.Content.Add(line);
				if (line.StartsWith("ENDDEF"))
				{
					item.SetName();
					listItems.Add(item);
					numberLinesToRemove += item.Content.Count;
					item = new Item();
				}
			}
			RemoveReadedLines(numberLinesToRemove);
			return listItems;
		}

		private IList<string> GetFooter()
		{
			List<string> result = new List<string>();

			foreach (var line in LinesList)
			{
				result.Add(line);
			}
			RemoveReadedLines(result.Count);
			return result;
		}

		private void RemoveReadedLines(int count)
		{
			LinesList.RemoveRange(0, count);
		}

		//private void PrintStringList(RichTextBox rtb, IList<string> list)
		//{
		//	foreach (var line in list)
		//	{
		//		rtb.Document.Blocks.Add(new Paragraph(new Run(line)));
		//	}
		//}

	}
}
