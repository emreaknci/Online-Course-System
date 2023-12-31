﻿using AutoMapper;
using MongoDB.Driver;
using OnlineCourse.Services.Catalog.Constants;
using OnlineCourse.Services.Catalog.Dtos;
using OnlineCourse.Services.Catalog.Models;
using OnlineCourse.Services.Catalog.Settings;
using OnlineCourse.Shared.Dtos;

namespace OnlineCourse.Services.Catalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(category => true).ToListAsync();
            var mappedCategories = _mapper.Map<List<CategoryDto>>(categories);
            return Response<List<CategoryDto>>.Success(200, mappedCategories, Messages.CategoriesListed);
        }

        public async Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _categoryCollection.InsertOneAsync(category);

            var mappedCategory = _mapper.Map<CategoryDto>(category);
            return Response<CategoryDto>.Success(200, mappedCategory, Messages.CategoryAdded);
        }

        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection
                .Find<Category>(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return Response<CategoryDto>.Fail(Messages.CategoryNotFound, 404);
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Response<CategoryDto>.Success(200, categoryDto,Messages.CategoryListed);
        }

    }
}
