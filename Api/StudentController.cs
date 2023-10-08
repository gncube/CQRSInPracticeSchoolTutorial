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

    [Function("StudentController")]
    public IActionResult StudentsGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = "students")] HttpRequestData req)
    {
        _logger.LogInformation("{FunctionName} function processed a request.", nameof(StudentsGet));

        var students = _studentRepository.GetAll();

        return new OkObjectResult(students);
    }

    [Function(nameof(StudentsAddAsync))]
    public async Task<IActionResult> StudentsAddAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "students")] HttpRequestData req)
    {
        _logger.LogInformation("{FunctionName} function processed a request.", nameof(StudentsAddAsync));
        // Get the student data from the request body.
        var student = await req.ReadFromJsonAsync<Student>();

        // Add the student to the repository.
        _studentRepository.AddAsync(student);

        // Return a success response.
        return new OkObjectResult(student);
    }

    [Function(nameof(Enroll))]
    public async Task<IActionResult> Enroll(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "students/studentId/{studentId}/courseId/{courseId}")] HttpRequestData req,
        long studentId, long courseId, Grade grade)
    {
        _logger.LogInformation("{FunctionName} function processed a request.", nameof(Enroll));
        Student student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            return new NotFoundObjectResult($"Student with id {studentId} not found");

        Course course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            return new NotFoundObjectResult($"Course with id {courseId} not found");

        string message = student.EnrollIn(course, grade);

        _context.SaveChanges();

        // Return a success response.
        return new OkObjectResult(message);
    }
}
