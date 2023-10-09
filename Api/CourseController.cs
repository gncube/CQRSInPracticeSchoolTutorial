using Application.Interfaces;
using Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Api;

public class CourseController
{
    private readonly ILogger _logger;
    private readonly IRepository<Course> _courseRepository;

    public CourseController(ILoggerFactory loggerFactory, IRepository<Course> courseRepository)
    {
        _logger = loggerFactory.CreateLogger<CourseController>();
        _courseRepository = courseRepository;
    }

    [Function("CourseController")]
    public HttpResponseData CoursesGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = "courses")] HttpRequestData req)
    {
        _logger.LogInformation("{FunctionName} function processed a request.", nameof(CoursesGet));

        var courses = _courseRepository.GetAll();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteAsJsonAsync(courses);
        return response;
    }

    [Function(nameof(CoursesAddAsync))]
    public async Task<HttpResponseData> CoursesAddAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "courses")] HttpRequestData req)
    {
        _logger.LogInformation("{FunctionName} function processed a request.", nameof(CoursesAddAsync));

        var course = await req.ReadFromJsonAsync<Course>();

        if (course == null)
        {
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }

        await _courseRepository.AddAsync(course);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteAsJsonAsync(course);
        return response;
    }
}
