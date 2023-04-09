using Bogus;

public record Book(string Title, Author Author);
public record Author(string Name);

public class LibraryQuery
{
    private readonly List<Book?> _book;
    private readonly CoursesRepository _courseRepository;

    public LibraryQuery(CoursesRepository courseRepository)
    {
        _book = new(){
                    new Book("I Love GraphQL", new Author("Brandon Minnick")),
                    new Book("GraphQL is the Future", new Author("Brandon Minnick")),
                    new Book("I Love SOAP + XML", new Author("John 'I Hate New Technology'")),
                    null
                };

        _courseRepository = courseRepository;
    }

    [GraphQLDeprecated("This query is deprecated.")]
    public List<Book?> GetBooks() => _book;

    [GraphQLDeprecated("This query is deprecated.")]
    public Book? GetBook(string title) => _book.FirstOrDefault(x => x?.Title == title);

    [GraphQLDeprecated("This query is deprecated.")]
    public Author? GetAuthor(string name) => _book.Where(x => x?.Author.Name == name).FirstOrDefault()?.Author;

    public async Task<IEnumerable<CourseType>> GetCourses()
    {

        return (await _courseRepository.GetAll()).Select(c => new CourseType{
            Id = c.Id,
            Name = c.Name,
            Subject = c.Subject,
            InstructorId = c.InstructorId
        });
    }

    public async Task<CourseType> GetCourseByIdAsync(Guid id)
    {
        var course = await _courseRepository.GetById(id);        

        return new CourseType{
            Id = course.Id,
            Name = course.Name,
            Subject = course.Subject,
            InstructorId = course.InstructorId
        };
    }
}