using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Moogle__Consola
{
	public class TFIDF
	{
		public string[] FileNames { get; private set; }
		public string[] TotalDistinctWords { get; private set; }
		public Dictionary<string, string[]> Doc_Words { get; private set; }
		public Dictionary<string, int> Term_DF { get; private set; }
		public Dictionary<string, double> Term_IDF { get; private set; }
		public Dictionary<string, Dictionary<string, double>> Doc__Term_TF { get; private set; }
		public Dictionary<string, Dictionary<string, double>> Doc__Term_TFIDF { get; private set; }

		public TFIDF(DataBase data)
		{
			FileNames = data.FileNames;
			TotalDistinctWords = data.TotalDistinctWords;
			Doc_Words = data.Doc_Words;
			Term_DF = new Dictionary<string, int>();
			Term_IDF = new Dictionary<string, double>();
			Doc__Term_TF = new Dictionary<string, Dictionary<string, double>>();
			Doc__Term_TFIDF = new Dictionary<string, Dictionary<string, double>>();

			
			TermFrequency();
			InverseDocumentFrequency();
			TermFrequency_InverseDocumentFrequency();

		}
		private void TermFrequency()
		{
			foreach (KeyValuePair<string, string[]> kvp in Doc_Words)
			{
				string document = kvp.Key;
				string[] words = kvp.Value;

				Dictionary<string, double> Word_TF = new Dictionary<string, double>();

				foreach (string term in words)
				{
					if (Word_TF.ContainsKey(term))
						Word_TF[term]++;
					else
						Word_TF[term] = 1;
				}

				foreach (string term in Word_TF.Keys)
				{
					if (Term_DF.ContainsKey(term))
						Term_DF[term]++;
					else
						Term_DF[term] = 1;
				}

				double Terms_in_Doc = words.Length;
				foreach (string term in Word_TF.Keys)
				{
					Word_TF[term] = Word_TF[term] / Terms_in_Doc;
				}
				Doc__Term_TF[document] = Word_TF;
			}
		}
		private void InverseDocumentFrequency()
		{
			foreach (string term in TotalDistinctWords)
			{
				double DF = Term_DF[term];
				double Total_Docs = Doc_Words.Count;
				double IDF = Math.Log2(Total_Docs / DF);
				Term_IDF.Add(term, IDF);
			}
		}
		private void TermFrequency_InverseDocumentFrequency()
		{
			foreach(string filename in FileNames)
			{
				Dictionary<string, double> Term_TFIDF = new Dictionary<string, double>();
				foreach (KeyValuePair<string, double> kvp in Doc__Term_TF[filename])
				{
					string word = kvp.Key;
					double TF = kvp.Value;
					double IDF = Term_IDF[word];
					double TFIDF = TF * IDF;
					Term_TFIDF.Add(word, TFIDF);
					
				}
				Doc__Term_TFIDF[filename] = Term_TFIDF;
			}
		}
	}
}
