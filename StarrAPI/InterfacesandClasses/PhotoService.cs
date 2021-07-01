using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using StarrAPI.AutoMapperHelp;
using StarrAPI.InterfacesandClasses;

namespace StarrAPI.InterfacesandClasses
{
    public class PhotoService : IPhotoUpload
    {
        private readonly Cloudinary _cloud;
        public PhotoService(IOptions<CloudinarySettings> Config)
        {
            var account = new Account
            (
                Config.Value.CloudName,
                Config.Value.ApiKey,
                Config.Value.ApiSecret
            );
            _cloud = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> AddPhotosAsync(IFormFile file)
        {
            var UploadResults = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var UploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName,stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                UploadResults = await _cloud.UploadAsync(UploadParams);
            }

            return UploadResults;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var DelelteParams = new DeletionParams(publicId);

            var result = await _cloud.DestroyAsync(DelelteParams); 
            return result;
        }
    }
}