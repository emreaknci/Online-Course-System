
namespace OnlineCourse.Web.Services.Interfaces;
public interface IPhotoStockService
{
    Task<string> UploadPhoto(IFormFile photo);

    Task<bool> DeletePhoto(string photoUrl);
}
