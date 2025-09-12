using API.Model;
using CloudinaryDotNet.Actions;

namespace API.Interface
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        Photo CreateNewPhoto(ImageUploadResult result, string memberId);
    }
}
