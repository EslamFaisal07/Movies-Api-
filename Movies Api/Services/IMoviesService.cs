namespace Movies_Api.Services
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetAll(byte genereId = 0);

        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(Movie movie);





    }
}
