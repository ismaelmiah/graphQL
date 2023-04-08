public class StudentDto : PersonType{
    public double GPA { get; set; }
    public IEnumerable<CourseDto> Courses { get; set; }
}
