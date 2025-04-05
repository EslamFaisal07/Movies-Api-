
using Microsoft.EntityFrameworkCore;

namespace Movies_Api.Services
{
    public class GenresService(ApplicationDbContext _dbContext) : IGenresService
    {
        public async  Task<Genre> Add(Genre genre)
        {
            await _dbContext.Genres.AddAsync(genre);
             _dbContext.SaveChanges();
            return genre;
        }

        public Genre Delete(Genre genre)
        {
            _dbContext.Remove(genre);
            _dbContext.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
      
            return await _dbContext.Genres.OrderBy(f => f.Name).ToListAsync(); ;
        }

        public async Task<Genre> GetById(byte id)
        {
            return await _dbContext.Genres.SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> IsValidGenre(byte id)
        {
            return await _dbContext.Genres.AnyAsync(g => g.Id == id);
        }

        public Genre Update(Genre genre)
        {
            _dbContext.Update(genre);
            _dbContext.SaveChanges();
            return genre;
        }
    }
}
