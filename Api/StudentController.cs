using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Api;

public class StudentController
{
    private readonly ILogger _logger;
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Course> _courseRepository;
    private readonly SchoolDbContext _context;

    public StudentController(ILoggerFactory loggerFactory, IRepository<Student> studentRepository, IRepository<Course> courseRepository, SchoolDbContext context)
    {
        _logger = loggerFactory.CreateLogger<StudentController>();
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _context = context;
    }

    [Function("StudentController")] // Query
    public IActionResult GetList([HttpTrigger(AuthorizationLevel.Function, "get", Route = "students")] HttpRequestData req, string enrolled, int? number)
    {
        _logger.LogInformation("{FunctionName} function processed a request.", nameof(GetList));

        IReadOnlyList<Student> students = _studentRepository.GetAll();
        List<StudentDto> studentDtos = students.Select(x => ConvertToDto(x)).ToList();

        return new OkObjectResult(students);
    }

    private StudentDto ConvertToDto(Student student)
    {
        return new StudentDto
        {
            //Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Enrollments = student.Enrollments.Select(x => new EnrollmentDto
            {
                // Id = x.Id,
                Course = x.Course.Name,
                Grade = x.Grade.ToString(),
                //Number = x.Number
            }).ToList()
        };
    }


    [Function(nameof(Register))] // Command
    public async Task<IActionResult> Register(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "students")] HttpRequestData req)
    {

        var dto = await req.ReadFromJsonAsync<StudentCreateDto>();
        if (dto == null)
            return new BadRequestObjectResult("Invalid dto data");
        var student = new Student(dto.Name, dto.Email);

        if (dto.Course1 != null && dto.Course1Grade != null)
        {
            Course course = await _courseRepository.GetByNameAsync(dto.Course1);
            if (course == null)
                return new NotFoundObjectResult($"Course with name {dto.Course1} not found");
            student.Enroll(course, Enum.Parse<Grade>(dto.Course1Grade));
        }

        _studentRepository.AddAsync(student);
        _context.SaveChanges();
        return new OkObjectResult(student);
    }

    [Function(nameof(Enroll))] // Command
    public async Task<IActionResult> Enroll(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "students/studentId/{studentId}/courseId/{courseId}")] HttpRequestData req,
    long studentId)
    {
        _logger.LogInformation("{FunctionName} function processed a request.", nameof(Enroll));
        Student student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            return new NotFoundObjectResult($"Student with id {studentId} not found");

        var studentEnrollmentDto = await req.ReadFromJsonAsync<StudentEnrollmentDto>();
        if (studentEnrollmentDto == null)
            return new BadRequestObjectResult("Invalid dto enrollment data");

        Course course = await _courseRepository.GetByNameAsync(studentEnrollmentDto.Course);
        if (course == null)
            return new NotFoundObjectResult($"Course with id {studentEnrollmentDto.Course} not found");

        bool success = Enum.TryParse(studentEnrollmentDto.Grade, out Grade grade);
        if (!success)
            return new BadRequestObjectResult($"Invalid grade {studentEnrollmentDto.Grade}");

        student.Enroll(course, grade);

        _context.SaveChanges();

        // Return a success response.
        return new OkObjectResult(student);
    }

    [Function(nameof(Transfer))] // Command
    public async Task<IActionResult> Transfer(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "students/studentId/{studentId}/enrollmentnumber/{enrollmentNumber}")] HttpRequestData req,
    long studentId, int enrollmentNumber)
    {
        _logger.LogInformation("{FunctionName} function processed a request.", nameof(Transfer));
        Student student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            return new NotFoundObjectResult($"Student with id {studentId} not found");

        var studentTransferDto = await req.ReadFromJsonAsync<StudentTransferDto>();
        if (studentTransferDto == null)
            return new BadRequestObjectResult("Invalid dto transfer data");

        Course course = await _courseRepository.GetByNameAsync(studentTransferDto.Course);
        if (course == null)
            return new NotFoundObjectResult($"Course with id {studentTransferDto.Course} not found");

        bool success = Enum.TryParse(studentTransferDto.Grade, out Grade grade);
        if (!success)
            return new BadRequestObjectResult($"Invalid grade {studentTransferDto.Grade}");

        Enrollment enrollment = student.GetEnrollment(enrollmentNumber);

        _context.SaveChanges();

        // Return a success response.
        return new OkObjectResult(enrollment);
    }
}
