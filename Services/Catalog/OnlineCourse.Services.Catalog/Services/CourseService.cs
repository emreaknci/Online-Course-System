﻿using AutoMapper;
using MassTransit;
using MongoDB.Driver;
using OnlineCourse.Services.Catalog.Constants;
using OnlineCourse.Services.Catalog.Dtos;
using OnlineCourse.Services.Catalog.Models;
using OnlineCourse.Services.Catalog.Settings;
using OnlineCourse.Shared.Dtos;
using OnlineCourse.Shared.Messages;

namespace OnlineCourse.Services.Catalog.Services;
public class CourseService : ICourseService
{
    private readonly IMongoCollection<Course> _courseCollection;
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);

        var database = client.GetDatabase(databaseSettings.DatabaseName);

        _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);

        _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Shared.Dtos.Response<List<CourseDto>>> GetAllAsync()
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
        return Shared.Dtos.Response<List<CourseDto>>.Success(200, mappedCourses, Messages.CoursesListed);
    }

    public async Task<Shared.Dtos.Response<CourseDto>> GetByIdAsync(string id)
    {
        var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

        if (course == null)
            return Shared.Dtos.Response<CourseDto>.Fail(Messages.CourseNotFound, 404);

        course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
        var mappedCourse = _mapper.Map<CourseDto>(course);
        return Shared.Dtos.Response<CourseDto>.Success(200, mappedCourse, Messages.CourseListed);
    }

    public async Task<Shared.Dtos.Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
    {
        var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();

        if (courses.Any())
            foreach (var course in courses)
                course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
        else
            courses = new List<Course>();

        var mappedCourses = _mapper.Map<List<CourseDto>>(courses);

        return Shared.Dtos.Response<List<CourseDto>>.Success(200, mappedCourses);
    }

    public async Task<Shared.Dtos.Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
    {
        var newCourse = _mapper.Map<Course>(courseCreateDto);

        newCourse.CreatedTime = DateTime.Now;
        await _courseCollection.InsertOneAsync(newCourse);

        var mappedNewCourse = _mapper.Map<CourseDto>(newCourse);
        return Shared.Dtos.Response<CourseDto>.Success(200, mappedNewCourse, Messages.CourseAdded);
    }

    public async Task<Shared.Dtos.Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
    {
        var existingCourse = await _courseCollection.Find(x => x.Id == courseUpdateDto.Id)
            .FirstOrDefaultAsync();

        if (existingCourse == null)
            return Shared.Dtos.Response<NoContent>.Fail(Messages.CourseNotFound, 404);

        var updatedCourse = _mapper.Map<Course>(courseUpdateDto);

        if (existingCourse.Name != updatedCourse.Name)
        {
            await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent
            {
                CourseId = updatedCourse.Id,
                UpdatedName = updatedCourse.Name
            });
        }

        await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updatedCourse);

        return Shared.Dtos.Response<NoContent>.Success(204);
    }

    public async Task<Shared.Dtos.Response<NoContent>> DeleteAsync(string id)
    {
        var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

        return result.DeletedCount > 0
            ? Shared.Dtos.Response<NoContent>.Success(204, message: Messages.CourseDeleted)
            : Shared.Dtos.Response<NoContent>.Fail(Messages.CourseNotFound, 404);
    }
}
