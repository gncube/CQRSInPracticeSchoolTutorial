using Domain.Enums;

namespace Domain;

public class SportsEnrollment
{
    public long Id { get; set; }
    public Grade Grade { get; set; }
    public long SportsId { get; set; }
    public Student Student { get; set; }
}
