using Moogle__Consola;

namespace MoogleEngine;


public static class Moogle
{
	public static DataBase data = new DataBase();
	public static TFIDF tfidf = new TFIDF(data);
	public static SearchResult Query(string query) {
		// Modifique este método para responder a la búsqueda

		if (!string.IsNullOrWhiteSpace(query))
		{
			Query user_query = new Query(query, tfidf);
			Similarity similarity = new Similarity(user_query, tfidf);
			SearchItem[] items = similarity.Items.ToArray();

            /*if (items.Length <= 4)
			{
				Suggestion suggestion = new Suggestion(query, similarity.DatabaseDistinctWords);
				string sug = suggestion.QuerySuggestion;
				return new SearchResult(items, sug);
			}
			else return new SearchResult(items, "");*/
            Suggestion suggestion = new Suggestion(query, similarity.DatabaseDistinctWords, tfidf.Term_DF);
            string sug = suggestion.QuerySuggestion;
            return new SearchResult(items, sug);
        }
		else
		{
			SearchItem[] items = new SearchItem[1] {new SearchItem("Por favor, ingrese algún término para realizar una búsqueda", "Intente nuevamente", 1f)};
			return new SearchResult(items, "");
		}
    }
}
