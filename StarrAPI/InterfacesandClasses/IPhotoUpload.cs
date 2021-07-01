using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace StarrAPI.InterfacesandClasses
{
    public interface IPhotoUpload
    {
        Task<ImageUploadResult> AddPhotosAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}