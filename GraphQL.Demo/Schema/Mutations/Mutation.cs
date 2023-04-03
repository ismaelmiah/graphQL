

using HotChocolate.Subscriptions;

public class Mutation
{
    private readonly List<CourseResult> _courses;
    public Mutation()
    {
        _courses = new List<CourseResult>();
    }

    public async Task<CourseResult> CreateCourse(CourseInputType courseInput, [Service] ITopicEventSender topicEventSender)
    {
        CourseResult course = new CourseResult
        {
            Id = Guid.NewGuid(),
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            InstructorId = courseInput.InstructorId
        };

        _courses.Add(course);
        await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

        return course;
    }

    public CourseResult UpdateCourse(Guid id, CourseInputType courseInput)
    {
        CourseResult course = _courses.FirstOrDefault(c => c.Id == id);

        if (course == null)
        {
            throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOUND"));
            //throw new Exception("Course not found");
        }
        course.Name = courseInput.Name;
        course.Subject = courseInput.Subject;
        course.InstructorId = courseInput.InstructorId;

        return course;
    }

    public bool DeleteCourse(Guid id)
    {
        return _courses.RemoveAll(c => c.Id == id) >= 1;
    }
}