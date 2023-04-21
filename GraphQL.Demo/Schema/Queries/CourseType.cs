public class CourseType : ISearchResultType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Subject Subject { get; set; }
    [IsProjected(true)]
    public string CreatorId { get; set; }
    [IsProjected(true)]
    public Guid InstructorId { get; set; }

    [GraphQLNonNullType]
    public async Task<InstructorType> Instructor([Service] InstructorLoader instructorLoader)
    {
        var instructor = await instructorLoader.LoadAsync(this.InstructorId);

        return new InstructorType
        {
            Id = instructor.Id,
            FirstName = instructor.FirstName,
            LastName = instructor.LastName,
            Salary = instructor.Salary
        };
    }
    public IEnumerable<StudentType> Students { get; set; }
    public async Task<UserType> Creator([Service] UserDataLoader userDataLoader)
    {
        if(CreatorId == null) return null;

        return await userDataLoader.LoadAsync(CreatorId, CancellationToken.None);
    }
}
