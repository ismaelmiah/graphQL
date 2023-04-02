var builder = WebApplication.CreateBuilder(args);

//Configure GraphQL Server
builder.Services.AddGraphQLServer().AddQueryType<LibraryQuery>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting().UseEndpoints(endpoints => endpoints.MapGraphQL());

app.UseAuthorization();

app.MapRazorPages();

app.Run();


//Implement GraphQL
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

    public List<Book?> GetBooks() => _book;
    public Book? GetBook(string title) => _book.FirstOrDefault(x => x?.Title == title);

    public Author? GetAuthor(string name) => _book.Where(x => x?.Author.Name == name).FirstOrDefault()?.Author;
}