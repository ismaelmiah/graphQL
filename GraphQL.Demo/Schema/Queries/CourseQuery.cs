[ExtendObjectType(typeof(Query))]
public class CourseQuery
{
    private readonly CoursesRepository _courseRepository;
    public CourseQuery(CoursesRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    [UseDbContext(typeof(SchoolDbContext))]
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection()]
    [UseFiltering(typeof(CourseFilterType))]
    [UseSorting(typeof(CourseSortType))]
    public IQueryable<CourseType> GetPaginatedCourses([ScopedService] SchoolDbContext context)
    {
        return context.Courses.Select(c => new CourseType{
            Id = c.Id,
            Name = c.Name,
            Subject = c.Subject,
            InstructorId = c.InstructorId,
            CreatorId = c.CreatorId
        });
    }

    public async Task<IEnumerable<CourseType>> GetCourses([ScopedService] SchoolDbContext context)
    {
        var courses =await _courseRepository.GetAll();

        return courses.Select(c => new CourseType{
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