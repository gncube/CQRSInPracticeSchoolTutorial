namespace Application.CQRS.Commands;
public class EditPersonalInfoCommand
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
