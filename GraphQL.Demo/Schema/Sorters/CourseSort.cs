using HotChocolate.Data.Sorting;

public class CourseSortType : SortInputType<CourseType>
{
    protected override void Configure(ISortInputTypeDescriptor<CourseType> descriptor)
    {
        descriptor.Ignore(c => c.Id);
        descriptor.Ignore(c => c.InstructorId);
        //descriptor.Field(c => c.Name).Name("CourseName");

        base.Configure(descriptor);
    }
}