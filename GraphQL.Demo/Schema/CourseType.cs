public class CourseType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Subject Subject { get; set; }
    [GraphQLNonNullType]
    public InstructorType Instructor { get; set; }
    [GraphQLNonNullType]
    public IEnumerable<StudentType> Students { get; set; }
}

public enum Subject{
    Mathematics,
    Science,
    History
}
