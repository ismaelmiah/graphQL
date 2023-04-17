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

        CourseDto courseDto = new CourseDto
        {
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            CreatorId = userId,
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
    public async Task<CourseResult> UpdateCourse(Guid id, CourseInputType courseInput, [Service] ITopicEventSender topicEventSender, ClaimsPrincipal claimsPrinciple)
    {

        string userId = claimsPrinciple.FindFirstValue(FirebaseUserClaimType.ID);

        CourseDto courseDTO = await _coursesRepository.GetById(id);

        if (courseDTO == null)
        {
            throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOUND"));
        }

        if (courseDTO.CreatorId != userId)
        {
            throw new GraphQLException(new Error("You do not have permission to update this course.", "INVALID_PERMISSION"));
        }

        courseDTO.Name = courseInput.Name;
        courseDTO.Subject = courseInput.Subject;
        courseDTO.InstructorId = courseInput.InstructorId;

        courseDTO = await _coursesRepository.Update(courseDTO);

        CourseResult course = new CourseResult
        {
            Id = courseDTO.Id,
            Name = courseDTO.Name,
            Subject = courseDTO.Subject,
            InstructorId = courseDTO.InstructorId
        };

        string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdate)}";
        await topicEventSender.SendAsync(updateCourseTopic, course);

        return course;
    }

    [Authorize(Policy = "IsAdmin")]
    public async Task<bool> DeleteCourse(Guid id)
    {
        try
        {
            return await _coursesRepository.Delete(id);
        }
        catch
        {
            return false;
        }
    }
}