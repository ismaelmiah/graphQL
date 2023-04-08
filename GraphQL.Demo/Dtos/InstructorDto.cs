
public class InstructorDto: PersonType{
    public double Salary { get; set; }
    public IEnumerable<CourseDto> Courses { get; set; }
}