using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data;
using System.Globalization;
using System.Numerics;

namespace Moogle__Consola
{
	public class DataBase
	{
		public string DataBasePath { get; private set; }
		public string[] Paths { get; private set; }
		public string[] FileNames { get; private set; }
		public string[] Texts { get; private set; }
		public string[] TotalDistinctWords { get; private set; }
		public Dictionary<string, string> Doc_Text { get; private set; }
		public Dictionary<string, string[]> Doc_Words { get; private set; }



		public DataBase()
		{
			SetDataBasePath();

			Paths = Directory.GetFiles(DataBasePath, "*.txt");
			FileNames = new string[Paths.Length];
			Texts = new string[Paths.Length];
			TotalDistinctWords = new string[0];
			Doc_Text = new Dictionary<string, string>();
			Doc_Words = new Dictionary<string, string[]>();

			SetDataReady();
			SetDocvsWords();
			SetTotalDistinctWords();
		}
		private void SetDataBasePath()
		{
			//busco la carpeta contenedora de mi proyecto y luego la que contiene mis documentos
			string projectpath = Directory.GetCurrentDirectory();
			projectpath = projectpath.Replace("MoogleServer", "");
            DataBasePath = Path.Combine(projectpath, "Content");
        }
		private void SetDataReady()
		{
			for (int i = 0; i < this.Paths.Length; i++)
			{
				FileNames[i] = Path.GetFileName(Paths[i]);
				Texts[i] = File.ReadAllText(Paths[i]);
				Doc_Text.Add(FileNames[i], Texts[i]);
			}
		}
		private void SetDocvsWords()
		{
			for (int i = 0; i < this.Texts.Length; i++)
			{
				//con la clase Regex (expresiones regulares) elimino de mi texto los signos de puntuacion y ciertos caracteres especiales
                string text = Regex.Replace(Texts[i].ToLower(), @"[°/|\\{}[\]()¿.,;:?!`¨'¡-]|""", string.Empty);
                string[] words = Regex.Split(text, " ").Where(word => !string.IsNullOrWhiteSpace(word)).ToArray();
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
