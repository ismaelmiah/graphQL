using Microsoft.EntityFrameworkCore;

public class CoursesRepository
{

    private readonly IDbContextFactory<SchoolDbContext> _contextFactory;

    public CoursesRepository(IDbContextFactory<SchoolDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<CourseDto>> GetAll()
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Courses.ToListAsync();
        }
    }
}