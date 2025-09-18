using API.Model;

namespace API.Repository.IRepository
{
    public interface IPhotoRepository
    {
        Task<IReadOnlyList<Photo>> GetUnapprovedPhotos();
        Task<Photo?> GetPhotoById(int id);
        void RemovePhoto(Photo photo);
    }
}
