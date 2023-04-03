public class Subscription
{
    [Subscribe]
    public CourseResult CourseCreated([EventMessage] CourseResult course) => course;
}