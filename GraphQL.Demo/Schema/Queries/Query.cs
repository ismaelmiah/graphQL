public record Book(string Title, Author Author);
public record Author(string Name);

public class Query
{
    private readonly CoursesRepository _courseRepository;

    public Query(CoursesRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 2)]
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