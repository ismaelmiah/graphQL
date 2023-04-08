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
            return await context.Courses.Include(c => c.Instructor).Include(c => c.Students).ToListAsync();
        }
    }

    public async Task<CourseDto> GetById(Guid courseId)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Courses.Include(c => c.Instructor).Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == courseId);
        }
    }

    public async Task<CourseDto> Create(CourseDto course)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            return course;
        }
    }

    public async Task<CourseDto> Update(CourseDto course)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            context.Courses.Update(course);
            await context.SaveChangesAsync();

            return course;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            CourseDto course = new CourseDto()
            {
                Id = id
            };
            context.Courses.Remove(course);

            return await context.SaveChangesAsync() > 0;
        }
    }
}