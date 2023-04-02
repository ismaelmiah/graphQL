
public record Book(string Title, Author Author);
public record Author(string Name);
public class LibraryQuery
{
    readonly List<Book?> _book = new(){
        new Book("I Love GraphQL", new Author("Brandon Minnick")),
        new Book("GraphQL is the Future", new Author("Brandon Minnick")),
        new Book("I Love SOAP + XML", new Author("John 'I Hate New Technology'")),
        null
    };

    [GraphQLDeprecated("This query is deprecated.")]
    public List<Book?> GetBooks() => _book;
    
    [GraphQLDeprecated("This query is deprecated.")]
    public Book? GetBook(string title) => _book.FirstOrDefault(x => x?.Title == title);

    [GraphQLDeprecated("This query is deprecated.")]
    public Author? GetAuthor(string name) => _book.Where(x => x?.Author.Name == name).FirstOrDefault()?.Author;
}