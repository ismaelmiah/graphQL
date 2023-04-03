using Bogus;

public record Book(string Title, Author Author);
public record Author(string Name);

public class LibraryQuery
{
    private readonly List<Book?> _book;
    private readonly Faker<InstructorType> _instructorFaker;
    private readonly Faker<StudentType> _studentFaker;
    private readonly Faker<CourseType> _courseFaker;


    public LibraryQuery()
    {
        _book = new(){
                    new Book("I Love GraphQL", new Author("Brandon Minnick")),
                    new Book("GraphQL is the Future", new Author("Brandon Minnick")),
                    new Book("I Love SOAP + XML", new Author("John 'I Hate New Technology'")),
                    null
                };

        _instructorFaker = new Faker<InstructorType>()
                            .RuleFor(c => c.Id, f => Guid.NewGuid())
                            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                            .RuleFor(c => c.LastName, f => f.Name.LastName())
                            .RuleFor(c => c.Salary, f => f.Random.Double(0, 100000));

        _studentFaker = new Faker<StudentType>()
                        .RuleFor(c => c.Id, f => Guid.NewGuid())
                        .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                        .RuleFor(c => c.LastName, f => f.Name.LastName())
                        .RuleFor(c => c.GPA, f => f.Random.Double(1, 4));

        _courseFaker = new Faker<CourseType>()
                        .RuleFor(c => c.Id, f => Guid.NewGuid())
                        .RuleFor(c => c.Name, f => f.Name.JobTitle())
                        .RuleFor(c => c.Subject, f => f.PickRandom<Subject>())
                        .RuleFor(c => c.Instructor, f => _instructorFaker.Generate())
                        .RuleFor(c => c.Students, f => _studentFaker.Generate(3));
    }

    [GraphQLDeprecated("This query is deprecated.")]
    public List<Book?> GetBooks() => _book;

    [GraphQLDeprecated("This query is deprecated.")]
    public Book? GetBook(string title) => _book.FirstOrDefault(x => x?.Title == title);

    [GraphQLDeprecated("This query is deprecated.")]
    public Author? GetAuthor(string name) => _book.Where(x => x?.Author.Name == name).FirstOrDefault()?.Author;

    public IEnumerable<CourseType> GetCourses()
    {

        return _courseFaker.Generate(5);
        // return new List<CourseType>()
        // {
        //     new CourseType(){
        //         Id = Guid.NewGuid(),
        //         Name = "Geomatry",
        //         Subject = Subject.Mathematics,
        //         InstructorType = new(){ Id = Guid.NewGuid() }
        //     }
        // };
    }

    public async Task<CourseType> GetCourseByIdAsync(Guid id)
    {
        await Task.Delay(1000);
        CourseType course = _courseFaker.Generate();

        course.Id = id;

        return course;
    }
}