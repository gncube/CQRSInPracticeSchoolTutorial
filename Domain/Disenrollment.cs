namespace Domain;

public class Disenrollment : Entity
{
    public Student Student { get; protected set; }
    public Course Course { get; protected set; }
    public DateTimeOffset DateTime { get; protected set; }
    public string Comment { get; protected set; }

    public Disenrollment(Student student, Course course, string comment)
    {
        Student = student;
        Course = course;
        Comment = comment;
        DateTime = DateTimeOffset.UtcNow;
    }
}