using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data;
using System.Globalization;

namespace Moogle__Consola
{
	public class DataBase
	{
		public string DataPath = Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName, "Content");
		public string[] Paths { get; private set; }
		public string[] FileNames { get; private set; }
		public string[] Texts { get; private set; }
		public string[] TotalDistinctWords { get; private set; }
		public Dictionary<string, string> Doc_Text { get; private set; }
		public Dictionary<string, string[]> Doc_Words { get; private set; }



		public DataBase()
		{
			Paths = Directory.GetFiles(DataPath, "*.txt");
			FileNames = new string[Paths.Length];
			Texts = new string[Paths.Length];
			TotalDistinctWords = new string[0];
			Doc_Text = new Dictionary<string, string>();
			Doc_Words = new Dictionary<string, string[]>();

			SetDataReady();
			SetDocvsWords();
			SetTotalDistinctWords();
		}
		private void SetDataReady()
		{
			for (int i = 0; i < this.Paths.Length; i++)
			{
				FileNames[i] = Path.GetFileName(Paths[i]);
				/*string pattern = @"[\p{M}]";
				//this pattern eliminates accent marks
				Texts[i] = Regex.Replace(File.ReadAllText(Paths[i]).ToLower().Normalize(NormalizationForm.FormD), pattern, string.Empty)
					.Normalize(NormalizationForm.FormC);
				Texts[i] = Regex.Replace(Texts[i], @"_", " ");*/
				Texts[i] = File.ReadAllText(Paths[i]);
				Doc_Text.Add(FileNames[i], Texts[i]);
			}
		}
		/*private string ClearAccentMarks(string text)
		{
			text = text.Replace('á', 'a');
			text = text.Replace('é', 'e');
			text = text.Replace('í', 'i');
			text = text.Replace('ó', 'o');
			text = text.Replace('ú', 'u');
			return text;
		}*/
		private void SetDocvsWords()
		{
			/*char[] separators = new char[] { '\n', '\t', ' ', '.', ',', ';', ':', '!', '_', '–', '-', '¿', '?', '’', '~', '“',
				'”', '"', '\'', '\\', '|', '/', '+', '@', '$', '#', '%', '^', '=', '`', '(', ')', '*', '-'};*/
			for (int i = 0; i < this.Texts.Length; i++)
			{
				/*string text = Texts[i];
				string term = "";
				List<string> termsList= new List<string>();
				for (int j = 0; j < text.Length; j++)
				{
					if (!char.IsLetterOrDigit(text[j]))
					{
						if(string.IsNullOrWhiteSpace(term))
						termsList.Add(term);
						term = "";
					}
					else
						term += text[j];
				}
				string[] terms = termsList.ToArray();*/

				string[] words = Regex.Split(Texts[i].ToLower(), @"\W+|_").Where(word => !string.IsNullOrWhiteSpace(word)).ToArray();
				/*foreach (string word in words)
				{
					Console.WriteLine(word);
				}
				List<string> Words_in_Doc = new List<string>();
				for (int j = 0; j < words.Length; j++)
				{
					if (!String.IsNullOrWhiteSpace(words[j]))
					{
						Words_in_Doc.Add(words[j]);
					}
				}
				words = Words_in_Doc.ToArray();*/
				Doc_Words.Add(this.FileNames[i], words);
			}
		}
		private void SetTotalDistinctWords()
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string[]> doc in Doc_Words)
			{
				string[] words = doc.Value;
				foreach (string word in words)
				{
					if (!String.IsNullOrWhiteSpace(word))
						list.Add(word);
				}
			}
			TotalDistinctWords = list.Distinct().ToArray();
		}
	}
}
