using Microsoft.EntityFrameworkCore;

public record Book(string Title, Author Author);
public record Author(string Name);

public class Query
{
    [UseDbContext(typeof(SchoolDbContext))]
    public async Task<IEnumerable<ISearchResultType>> Search(string term, [ScopedService] SchoolDbContext context)
    {
        IEnumerable<CourseType> courses = await context.Courses.Where(c => c.Name.ToLower().Contains(term.ToLower()))
                                            .Select(c => new CourseType{
                                                Id = c.Id,
                                                Name = c.Name,
                                                Subject = c.Subject,
                                                InstructorId = c.InstructorId,
                                                CreatorId = c.CreatorId
                                            }).ToListAsync();

        IEnumerable<InstructorType> instructors = await context.Instructors
                                        .Where(i => i.FirstName.ToLower().Contains(term.ToLower()) || i.LastName.ToLower().Contains(term.ToLower()))
                                            .Select(i => new InstructorType{
                                                Id = i.Id,
                                                FirstName = i.FirstName,
                                                LastName = i.LastName,
                                                Salary = i.Salary
                                            }).ToListAsync();

        return new List<ISearchResultType>().Concat(courses).Concat(instructors);
    }
}