using HotChocolate.Execution;
using HotChocolate.Subscriptions;

public class Subscription
{
    [Subscribe]
    public CourseResult CourseCreated([EventMessage] CourseResult course) => course;

    [SubscribeAndResolve]
    public ValueTask<ISourceStream<CourseResult>> CourseUpdate(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver)
    {
        string topicName = $"{courseId}_{nameof(Subscription.CourseUpdate)}";
        return topicEventReceiver.SubscribeAsync<string, CourseResult>(topicName);
    }
}