using Domain.Enums;

namespace Domain;

public class Enrollment
{
    public long Id { get; set; }
    public Grade Grade { get; set; }
    public long CourseId { get; set; }
    public Student Student { get; set; }
}