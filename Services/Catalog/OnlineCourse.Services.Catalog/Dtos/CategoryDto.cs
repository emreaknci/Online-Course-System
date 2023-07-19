using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnlineCourse.Services.Catalog.Models;

namespace OnlineCourse.Services.Catalog.Dtos
{
    public class CategoryDto
    {
        public string Id{ get; set; }
        public string Name{ get; set; }
    }
    public class CategoryCreateDto
    {
        public string Name { get; set; }
    }
}
