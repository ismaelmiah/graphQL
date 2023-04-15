

using System.Security.Claims;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Subscriptions;

public class Mutation
{
    private readonly CoursesRepository _coursesRepository;
    public Mutation(CoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }

    [Authorize]
    public async Task<CourseResult> CreateCourse(CourseInputType courseInput, [Service] ITopicEventSender topicEventSender, ClaimsPrincipal claimsPrinciple)
    {
        string userId = claimsPrinciple.FindFirstValue(FirebaseUserClaimType.ID);
        string email = claimsPrinciple.FindFirstValue(FirebaseUserClaimType.EMAIL);

        CourseDto courseDto = new CourseDto
        {
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            InstructorId = courseInput.InstructorId
        };

        try
        {
            var courseResult = await _coursesRepository.Create(courseDto);

            CourseResult course = new CourseResult
            {
                Id = courseResult.Id,
                Name = courseResult.Name,
                Subject = courseResult.Subject,
                InstructorId = courseResult.InstructorId
            };

            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

            return course;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [Authorize]
    public async Task<CourseResult> UpdateCourse(Guid id, CourseInputType courseInput, [Service] ITopicEventSender topicEventSender)
    {

        CourseDto courseDto = new CourseDto
        {
            Id = id,
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            InstructorId = courseInput.InstructorId
        };

        courseDto = await _coursesRepository.Update(courseDto);

        CourseResult course = new CourseResult
        {
            Id = courseDto.Id,
            Name = courseDto.Name,
            Subject = courseDto.Subject,
            InstructorId = courseDto.InstructorId
        };

        string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdate)}";
        await topicEventSender.SendAsync(updateCourseTopic, course);

        return course;
    }

    [Authorize]
    public async Task<bool> DeleteCourse(Guid id)
    {
        try{
            return await _coursesRepository.Delete(id);
        }
        catch{
            return false;
        }
    }
}