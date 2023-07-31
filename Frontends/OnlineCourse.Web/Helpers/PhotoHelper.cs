using Microsoft.Extensions.Options;
using OnlineCourse.Web.Models;

namespace OnlineCourse.Web.Helpers;

public class PhotoHelper
{
    private readonly ServiceApiSettings _serviceApiSettings;

    public PhotoHelper(IOptions<ServiceApiSettings> serviceApiSettings)
    {
        _serviceApiSettings = serviceApiSettings.Value;
    }

    public string GetPhotoStockUrl(string photoUrl)
    {
        return $"{_serviceApiSettings.PhotoStockUri}/photos/{photoUrl}";
    }
}


