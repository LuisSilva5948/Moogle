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
		public string QuerySuggestion { get; private set; }
		public string[] DataBaseDistinctWords { get; private set; }
        public Suggestion(string query, string[] databaseDistinctWords, Dictionary<string, int> Term_DF)
		{

            Query = query;
			DataBaseDistinctWords = databaseDistinctWords;
            QuerySuggestion = "";

			SetQuerySuggestion(Term_DF);
		}
		private void SetQuerySuggestion(Dictionary<string, int> Term_DF)
		{
			//tokenizo la query
			string[] queryTerms = Regex.Split(Query.ToLower(), " ").Where(term => !String.IsNullOrWhiteSpace(term)).ToArray();
			int smallestdistance = int.MaxValue;

			//por cada término de mi query itero
			for (int i = 0; i < queryTerms.Length; i++)
			{
                if (i != 0)
                {
                    QuerySuggestion += " ";
                }
				//si el termino aparece en más de 4 documentos de mi base de datos, es un término general y es devuelto como tal
				if (DataBaseDistinctWords.Contains(queryTerms[i]) && Term_DF[queryTerms[i]] > 4)
				{
                    QuerySuggestion += queryTerms[i];
                }
				//de lo contrario, buscar el término general (de alto DF) que más se parezca a mi término actual de la query
				else
                {
                    string newQueryTerm = "";
                    foreach (string databaseTerm in DataBaseDistinctWords)
                    {
						if (Term_DF[databaseTerm] > 3)
						{
							//para calcular la similitud de una palabra a otra uso el algoritmo de distancia de Levenshtein
							int distance = LevenshteinDistance(queryTerms[i], databaseTerm);
							if (smallestdistance > distance)
							{
								smallestdistance = distance;
								newQueryTerm = databaseTerm;
							}
						}
                    }
                    QuerySuggestion += newQueryTerm;
                }
				//cuando la sugerencia obtenida es igual a la propia query, devolverla vacia para que no sea mostrada,
				//puesto que los terminos de la query fueron correctos
				if (QuerySuggestion == Query)
					QuerySuggestion = "";
            }
		}
		private int LevenshteinDistance(string s, string t)
		{
			// d es una matriz con m+1 filas y n+1 columnas
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

			// recorre la matriz llenando cada unos de los pesos.
			// i columnas, j renglones
			for (int i = 1; i <= m; i++)
			{
				// recorre para j
				for (int j = 1; j <= n; j++)
				{
					// si son iguales en posiciones equidistantes el peso es 0
					// de lo contrario el peso suma a uno.
					costo = (s[i - 1] == t[j - 1]) ? 0 : 1;
					d[i, j] = System.Math.Min(System.Math.Min(d[i - 1, j] + 1,  //Eliminacion
								  d[i, j - 1] + 1),                             //Insercion 
								  d[i - 1, j - 1] + costo);                     //Sustitucion
				}
			}
			return d[m, n];
		}
	}
}
