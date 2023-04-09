using Microsoft.EntityFrameworkCore;

public class InstructorRepository{
    
    private readonly IDbContextFactory<SchoolDbContext> _contextFactory;

    public InstructorRepository(IDbContextFactory<SchoolDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<InstructorDto>> GetAll()
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Instructors.ToListAsync();
        }
    }

    public async Task<InstructorDto> GetById(Guid instructorId)
    {
        Console.WriteLine(instructorId);
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Instructors.FirstOrDefaultAsync(c => c.Id == instructorId);
        }
    }

    public async Task<InstructorDto> Create(InstructorDto instructor)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            context.Instructors.Add(instructor);
            await context.SaveChangesAsync();

            return instructor;
        }
    }

    public async Task<InstructorDto> Update(InstructorDto instructor)
    {
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            context.Instructors.Update(instructor);
            await context.SaveChangesAsync();

            return instructor;
        }
    }

    public async Task<IEnumerable<InstructorDto>> GetByIds(IReadOnlyList<Guid> instructorIds)
    {
        Console.WriteLine(instructorIds);
        using (SchoolDbContext context = _contextFactory.CreateDbContext())
        {
            return await context.Instructors.Where(i => instructorIds.Contains(i.Id)).ToListAsync();
        }
    }
}