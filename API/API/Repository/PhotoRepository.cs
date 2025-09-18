using API.Data;
using API.Model;
using API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class PhotoRepository(AppDbContext _context) : IPhotoRepository
    {
        public async Task<IReadOnlyList<Photo>> GetUnapprovedPhotos()
            => await _context.Photos.IgnoreQueryFilters().Where(p => !p.IsApproved).ToListAsync();

        public async Task<Photo?> GetPhotoById(int id)
            => await _context.Photos.IgnoreQueryFilters().SingleOrDefaultAsync(p => p.Id == id);

        public void RemovePhoto(Photo photo) => _context.Photos.Remove(photo);
    }
}
