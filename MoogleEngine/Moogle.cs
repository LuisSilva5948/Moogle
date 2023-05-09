using Moogle__Consola;

namespace MoogleEngine;


public static class Moogle
{
	//creando los objetos data y tfidf de sus respectivas clases alisto mis documentos
	public static DataBase data = new DataBase();
	public static TFIDF tfidf = new TFIDF(data);
	public static SearchResult Query(string query) {

        //si la query no es vacia, ejecutar la búsqueda
        if (!string.IsNullOrWhiteSpace(query))
		{
			Query user_query = new Query(query, tfidf);
            Similarity similarity = new Similarity(user_query, tfidf);
			SearchItem[] items = similarity.Items.ToArray();
            Suggestion suggestion = new Suggestion(query, similarity.DatabaseDistinctWords, tfidf.Term_DF);
            string sug = suggestion.QuerySuggestion;
			//los documentos encontrados son devueltos junto con una sugerencia
            return new SearchResult(items, sug);
        }
		else
		{
			SearchItem[] items = new SearchItem[1] {new SearchItem("Por favor, ingrese algún término para realizar una búsqueda", "Intente nuevamente", 0)};
			return new SearchResult(items, "");
		}
    }
}
