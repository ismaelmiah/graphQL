
using Bogus;

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

    public IEnumerable<CourseType> GetCourses()
    {
        Faker<InstructorType> instructorFaker = new Faker<InstructorType>()
                                                .RuleFor(c => c.Id, f => Guid.NewGuid())
                                                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                                                .RuleFor(c => c.FirstName, f => f.Name.LastName())
                                                .RuleFor(c => c.Salary, f => f.Random.Double(0, 100000));
                                                
        Faker<StudentType> studentFaker = new Faker<StudentType>()
                                                .RuleFor(c => c.Id, f => Guid.NewGuid())
                                                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                                                .RuleFor(c => c.FirstName, f => f.Name.LastName())
                                                .RuleFor(c => c.GPA, f => f.Random.Double(1, 4));

        Faker<CourseType> courseFaker = new Faker<CourseType>()
                                        .RuleFor(c => c.Id, f => Guid.NewGuid())
                                        .RuleFor(c => c.Name, f => f.Name.JobTitle())
                                        .RuleFor(c => c.Subject, f => f.PickRandom<Subject>())
                                        .RuleFor(c => c.InstructorType, f => instructorFaker.Generate())
                                        .RuleFor(c => c.Students, f => studentFaker.Generate(3));
                                        
        List<CourseType> courses = courseFaker.Generate(5);

        return courses;
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
}