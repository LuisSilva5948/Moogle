using Moogle__Consola;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoogleEngine
{
	public class Suggestion
	{
		public string Query { get; private set; }
		public string newQuery { get; private set; }
		public string[] DataBaseDistinctWords { get; private set; }
		public Suggestion(string query, string[] databaseDistinctWords)
		{
			Query = query;
			DataBaseDistinctWords = databaseDistinctWords;
			newQuery = "";

			SetNewQuery();
		}
		private void SetNewQuery()
		{
			string[] queryTerms = Regex.Split(Query.ToLower(), " ").Where(term => !String.IsNullOrWhiteSpace(term)).ToArray();
			int smallestdistance = int.MaxValue;
			for (int i = 0; i < queryTerms.Length; i++)
			{
				if (i != 0)
				{
					newQuery += " ";
				}
				if (!DataBaseDistinctWords.Contains(queryTerms[i]))
				{
					string newQueryTerm = "";
					foreach (string databaseTerm in DataBaseDistinctWords)
					{
						int distance = LevenshteinDistance(queryTerms[i], databaseTerm);
						if (smallestdistance > distance)
						{
							smallestdistance = distance;
							newQueryTerm = databaseTerm;
						}
					}
					newQuery += newQueryTerm;
				}
				else newQuery += queryTerms[i];
			}
		}
		private int LevenshteinDistance(string s, string t)
		{
			double porcentaje;

			// d es una tabla con m+1 renglones y n+1 columnas
			int costo = 0;
			int m = s.Length;
			int n = t.Length;
			int[,] d = new int[m + 1, n + 1];

			// Verifica que exista algo que comparar
			if (n == 0) return m;
			if (m == 0) return n;

			// Llena la primera columna y la primera fila.
			for (int i = 0; i <= m; d[i, 0] = i++) ;
			for (int j = 0; j <= n; d[0, j] = j++) ;

			/// recorre la matriz llenando cada unos de los pesos.
			/// i columnas, j renglones
			for (int i = 1; i <= m; i++)
			{
				// recorre para j
				for (int j = 1; j <= n; j++)
				{
					/// si son iguales en posiciones equidistantes el peso es 0
					/// de lo contrario el peso suma a uno.
					costo = (s[i - 1] == t[j - 1]) ? 0 : 1;
					d[i, j] = System.Math.Min(System.Math.Min(d[i - 1, j] + 1,  //Eliminacion
								  d[i, j - 1] + 1),                             //Insercion 
								  d[i - 1, j - 1] + costo);                     //Sustitucion
				}
			}

			/// Calculamos el porcentaje de cambios en la palabra.
			if (s.Length > t.Length)
				porcentaje = ((double)d[m, n] / (double)s.Length);
			else
				porcentaje = ((double)d[m, n] / (double)t.Length);
			return d[m, n];
		}
	}
}
