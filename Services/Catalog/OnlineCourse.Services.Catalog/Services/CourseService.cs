using AutoMapper;
using MongoDB.Driver;
using OnlineCourse.Services.Catalog.Dtos;
using OnlineCourse.Services.Catalog.Models;
using OnlineCourse.Services.Catalog.Settings;
using OnlineCourse.Shared.Dtos;

namespace OnlineCourse.Services.Catalog.Services;
public class CourseService : ICourseService
{
    private readonly IMongoCollection<Course> _courseCollection;
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;

    public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);

        var database = client.GetDatabase(databaseSettings.DatabaseName);

        _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);

        _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<Response<List<CourseDto>>> GetAllAsync()
    {
        var courses = await _courseCollection.Find(course => true).ToListAsync();

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            }
        }
        else
        {
            courses = new List<Course>();
        }

        var mappedCourses = _mapper.Map<List<CourseDto>>(courses);
        return Response<List<CourseDto>>.Success(200, mappedCourses);
    }

    public async Task<Response<CourseDto>> GetByIdAsync(string id)
    {
        var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

        if (course == null)
            return Response<CourseDto>.Fail("Course not found", 404);
        
        course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
        var mappedCourse = _mapper.Map<CourseDto>(course);
        return Response<CourseDto>.Success(200, mappedCourse);
    }

    public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
    {
        var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();

        if (courses.Any())
            foreach (var course in courses)
                course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
        else
            courses = new List<Course>();
        
        var mappedCourses = _mapper.Map<List<CourseDto>>(courses);

        return Response<List<CourseDto>>.Success( 200,mappedCourses);
    }

    public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
    {
        var newCourse = _mapper.Map<Course>(courseCreateDto);

        newCourse.CreatedTime = DateTime.Now;
        await _courseCollection.InsertOneAsync(newCourse);

        var mappedNewCourse = _mapper.Map<CourseDto>(newCourse);
        return Response<CourseDto>.Success(200, mappedNewCourse);
    }

    public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
    {
        var updateCourse = _mapper.Map<Course>(courseUpdateDto);
        var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);

        return result == null 
            ? Response<NoContent>.Fail("Course not found", 404) 
            : Response<NoContent>.Success(204);
    }

    public async Task<Response<NoContent>> DeleteAsync(string id)
    {
        var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

        return result.DeletedCount > 0 
            ? Response<NoContent>.Success(204) 
            : Response<NoContent>.Fail("Course not found", 404);
    }
}
