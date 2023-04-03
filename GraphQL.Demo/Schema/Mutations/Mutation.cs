

public class Mutation
{
    private readonly List<CourseResult> _courses;
    public Mutation()
    {
        _courses = new List<CourseResult>();
    }

    public CourseResult CreateCourse(string name, Subject subject, Guid instructorId)
    {
        CourseResult courseResult = new CourseResult
        {
            Id = Guid.NewGuid(),
            Name = name,
            Subject = subject,
            InstructorId = instructorId
        };

        _courses.Add(courseResult);

        return courseResult;
    }

    public CourseResult UpdateCourse(Guid id, string name, Subject subject, Guid instructorId)
    {
        CourseResult course = _courses.FirstOrDefault(c => c.Id == id);

        if (course == null)
        {
            throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOUND"));
            //throw new Exception("Course not found");
        }
        course.Name = name;
        course.Subject = subject;
        course.InstructorId = instructorId;

        return course;
    }

    public bool DeleteCourse(Guid id)
    {
        return _courses.RemoveAll(c => c.Id == id) >= 1;
    }
}