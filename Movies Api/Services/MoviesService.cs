
using Microsoft.EntityFrameworkCore;
using Movies_Api.Models;

namespace Movies_Api.Services
{
    public class MoviesService(ApplicationDbContext _Context) : IMoviesService
    {
        public async Task<Movie> Add(Movie movie)
        {
            await _Context.Movies.AddAsync(movie);
            _Context.SaveChanges();
            return movie;
        }

        public Movie Delete(Movie movie)
        {
            _Context.Remove(movie);
            _Context.SaveChanges();
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genereId = 0)
        {
            return await _Context.Movies
                .Where(m=>m.GenreId == genereId || genereId == 0).OrderByDescending(m => m.Rate).Include(m => m.Genre).ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _Context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
        }

        public Movie Update(Movie movie)
        {
            _Context.Update(movie);
            _Context.SaveChanges();
            return movie;
        }
    }
}
