using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Moogle__Consola
{
	public class Query
	{
		public string QueryText { get; private set; }
		public string[] QueryWords { get; private set; }
		public string[] QueryDistinctWords { get; private set; }
		public int DatabaseTotalDocs { get; private set; }
		public Dictionary<string, int> DatabaseTerm_DF { get; private set; }
		public Dictionary<string, double> QueryTerm_TFIDF { get; private set; }

		public Query(string query, TFIDF tfidf)
		{
			QueryText = query;
			QueryWords = new string[0];
			QueryDistinctWords = new string[0];
			DatabaseTotalDocs = tfidf.FileNames.Length;
			DatabaseTerm_DF = tfidf.Term_DF;
			QueryTerm_TFIDF = new Dictionary<string, double>();

			SetQueryWords();
			SetQueryTFIDF();
		}
		private void SetQueryWords()
		{
			string[] words = Regex.Split(QueryText.ToLower(), @"\W+|_").Where((term => !string.IsNullOrWhiteSpace(term))).ToArray();
			QueryWords = words;
			QueryDistinctWords = words.Distinct().ToArray();

		}
		private void SetQueryTFIDF()
		{
			Dictionary<string, double> Query_TF = new Dictionary<string, double>();
			Dictionary<string, int> Query_DF = new Dictionary<string, int>();
			//TF and DF
			foreach (string term in QueryWords)
			{
				if (Query_TF.ContainsKey(term))
					Query_TF[term]++;
				else
					Query_TF[term] = 1;

				if (DatabaseTerm_DF.ContainsKey(term) && !Query_DF.ContainsKey(term))
					Query_DF[term] = DatabaseTerm_DF[term] + 1;
				else
					Query_DF[term] = 1;
			}
			foreach (string term in Query_TF.Keys)
			{
				Query_TF[term] = Query_TF[term] / QueryWords.Length;
			}
			
			//IDF and TFIDF
			foreach(string term in Query_DF.Keys)
			{
				double DF = Query_DF[term];
				//DatabaseTotalDocs including the query as another doc is totaldocs + 1
				double TotalDocs = DatabaseTotalDocs + 1;
				double IDF = Math.Log2( TotalDocs / DF);
				double TF = Query_TF[term];
				double TFIDF = TF * IDF;
				QueryTerm_TFIDF[term] = TFIDF;
			}
		}
	}
}
