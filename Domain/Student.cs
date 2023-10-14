using Domain.Enums;

namespace Domain;
public class Student : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }

    public readonly IList<Enrollment> _enrollments = new List<Enrollment>();
    public IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();


    public readonly IList<Disenrollment> _disenrollments = new List<Disenrollment>();
    public IReadOnlyList<Disenrollment> Disenrollments => _disenrollments.ToList();

    public Student(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public Enrollment GetEnrollment(int index)
    {
        if (_enrollments.Count > index)
            return _enrollments[index];
        return null;
    }

    public void RemoveEnrollment(Enrollment enrollment, string comment)
    {
        _enrollments?.Remove(enrollment);

        var disenrollment = new Disenrollment(enrollment.Student, enrollment.Course, comment);
        _disenrollments.Add(disenrollment);
    }

    public void Enroll(Course course, Grade grade)
    {
        if (_enrollments.Count >= 2)
            throw new Exception("Cannot have move than 2 enrollments");

        var enrollment = new Enrollment(this, course, grade);
        _enrollments.Add(enrollment);
    }
}

