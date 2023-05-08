using MoogleEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Moogle__Consola
{
	public class Similarity
	{
		public string QueryText { get; private set; }
		public string[] FileNames { get; private set; }
		public string[] DatabaseDistinctWords { get; private set; }
		public string[] QueryDistinctWords { get; private set; }
		public Dictionary<string, string> Doc_Text { get; private set; }
        public Dictionary<string, string[]> Doc_Words { get; private set; }
        public Dictionary<string, double> QueryTerm_TFIDF { get; private set; }
		public Dictionary<string, Dictionary<string, double>> Doc__Term_TFIDF { get; private set; }
		public Dictionary<string, Dictionary<string, double>> Doc__Term_newTFIDF { get; private set; }
		public Dictionary<string, double> BruteSimilarity { get; private set; }
		public Dictionary<string, double> CosineSimilarity { get; private set; }
		public List<SearchItem> Items { get; private set; }

		public Similarity(Query query, TFIDF tfidf)
		{
			QueryText = query.QueryText;
			FileNames = tfidf.FileNames;
			DatabaseDistinctWords = tfidf.TotalDistinctWords;
			QueryTerm_TFIDF = query.QueryTerm_TFIDF;
			Doc__Term_TFIDF = tfidf.Doc__Term_TFIDF;
			Doc__Term_newTFIDF = new Dictionary<string, Dictionary<string, double>>();
			BruteSimilarity = new Dictionary<string, double>();
			CosineSimilarity = new Dictionary<string, double>();
			Items = new List<SearchItem>();
			Doc_Text = tfidf.Doc_Text;
			Doc_Words = tfidf.Doc_Words;
			QueryDistinctWords = query.QueryDistinctWords;

			CalculateSimilarity();
			CalculateCosineSimilarity();
			SetMostSimilarDocuments();
		}
		private void CalculateSimilarity()
		{
			foreach (string filename in FileNames)
			{
				Dictionary<string, double> Term_newTFIDF = new Dictionary<string, double>();
				foreach (string word in DatabaseDistinctWords)
				{
					if (Doc__Term_TFIDF[filename].ContainsKey(word) && QueryTerm_TFIDF.ContainsKey(word))
					{
						double new_tfidf = Doc__Term_TFIDF[filename][word] * QueryTerm_TFIDF[word];
						if (new_tfidf > 0)
							Term_newTFIDF.Add(word, new_tfidf);
					}
				}
				Doc__Term_newTFIDF.Add(filename, Term_newTFIDF);

				double sim = 0;
				foreach (double value in Term_newTFIDF.Values)
				{
					sim += value;
				}
				BruteSimilarity.Add(filename, sim);
			}
		}
		private void CalculateCosineSimilarity()
		{
			double Sum_of_Query_TFIDF_Squared = 0;
			foreach (double tfidf in QueryTerm_TFIDF.Values)
			{
				double value = Math.Pow(tfidf, 2);
				Sum_of_Query_TFIDF_Squared += value;
			}

			foreach (string filename in FileNames)
			{
				double Sim = BruteSimilarity[filename];
				double Sum_of_Doc_TFIDF_Squared = 0;
				foreach (double tfidf in Doc__Term_TFIDF[filename].Values)
				{
					double value = Math.Pow(tfidf, 2);
					Sum_of_Doc_TFIDF_Squared += value;
				}
				if (Sum_of_Doc_TFIDF_Squared > 0 && Sum_of_Query_TFIDF_Squared > 0)
				{
					double CosSim = Sim / Math.Sqrt(Sum_of_Doc_TFIDF_Squared * Sum_of_Query_TFIDF_Squared);
					CosineSimilarity.Add(filename, CosSim);
				}
			}
		}
		private void SetMostSimilarDocuments()
		{
			List<KeyValuePair<string, double>> SimilarDocuments = new List<KeyValuePair<string, double>>();
			foreach (KeyValuePair<string, double> kvp in CosineSimilarity)
			{
				double similarity = kvp.Value;
				if (similarity > 0)
				{
					SimilarDocuments.Add(kvp);
				}
			}
			SimilarDocuments = SimilarDocuments.OrderByDescending(kvp => kvp.Value).ToList();
			
			if (SimilarDocuments.Count > 0)
			{
				foreach (var kvp in SimilarDocuments)
				{
					string title = kvp.Key;
					string snippet = GetSnippet(title);
					double score = kvp.Value;
					Items.Add(new SearchItem(title, snippet, score));
				}
			}
			else Items.Add(new SearchItem("No se han encontrado resultados para tu búsqueda ('" + QueryText + "')", "Intente nuevamente", 0));
		}
		private string GetSnippet(string title)
		{
			string text = Doc_Text[title];

			if (text.Length < 50)
			{
				return text;
			}

            string queryTerm = "";
			double highestValue = 0;
            foreach (var kvp in Doc__Term_newTFIDF[title])
            {
				string key = kvp.Key;
				double newValue = kvp.Value;
                if (newValue > highestValue)
                {
                    highestValue = newValue;
                    queryTerm = key;
                }
            }

            //int queryTermIndex = 0;
            //int queryTermStartIndex = 0;
            //int queryTermEndIndex = 0;
            int queryIndex = Array.IndexOf(Doc_Words[title], queryTerm);

            string[] separatedtext = Regex.Split(text, " ").Where(word => !string.IsNullOrWhiteSpace(word)).ToArray();
            
            int start = Math.Max(0, queryIndex - 10);
			int end = Math.Min(separatedtext.Length - 1, queryIndex + 30);
			string snippet = string.Join(' ', separatedtext, start, end - start + 1);
            return snippet;
		}
	}
}