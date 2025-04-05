using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_Api.Services;

namespace Movies_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController(IMoviesService _moviesService , IMapper _mapper , IGenresService genresService) : ControllerBase
    {

        private new List<string> _allowedExtentions = new List<string>{".jpg" , ".png"};

        private long _maxAllowedPosterSize = 1048576; // 1 MB
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto dto)
        {
            if (dto.Poster == null)
            {
                return BadRequest("poster is required");
            }

            if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            {
                return BadRequest("only png and jpg images are allowed");
            }
            if (dto.Poster.Length > _maxAllowedPosterSize)
            {
                return BadRequest("Image size is too large must be less than or equal 1MB");
            }
            var isValidGenre = genresService.IsValidGenre(dto.GenreId).Result;



            if (!isValidGenre)
            {
                return BadRequest("Invalid Genre Id");
            }
            using var datastream = new MemoryStream();
            await dto.Poster.CopyToAsync(datastream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = datastream.ToArray();

            _moviesService.Add(movie);
            return Ok(movie);


        }



        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _moviesService.GetAll();

            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);



            return Ok(data);
        }



        [HttpGet("{id}")]

        public async Task<IActionResult> GetMovieByIdAsync(int id )
        {
           
            var movie =  await _moviesService.GetById(id);

            if (movie == null)
            {
                return NotFound(" Movie Not Found ");
            }
            var data = _mapper.Map<MovieDetailsDto>(movie);
            return Ok(data);




        }





        [HttpGet("ByGenreId")]

        public async Task<IActionResult> GetMoviesByGenreIdAsync(byte id)
        {
            var movies = await _moviesService.GetAll(id);


            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);



            return Ok(data);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
            {
                
                return NotFound($" Movie Not Found with id {id}");
            }
           _moviesService.Delete(movie);


            return Ok(movie);
        }



        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateMovieAsync([FromForm] UpdatedMovieDto dto,int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
            {
                return NotFound($" Movie Not Found with id {id}");
            }
           
             
            var isValidGenre = await genresService.IsValidGenre(dto.GenreId);
            if (!isValidGenre)
            {
                return BadRequest("Invalid Genre Id");
            }

            if (dto.Poster != null)
            {
                if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                {
                    return BadRequest("only png and jpg images are allowed");
                }
                if (dto.Poster.Length > _maxAllowedPosterSize)
                {
                    return BadRequest("Image size is too large must be less than or equal 1MB");
                }
                using var datastream = new MemoryStream();
                await dto.Poster.CopyToAsync(datastream);

                movie.Poster = datastream.ToArray();


            }


            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rate = dto.Rate;
            movie.StoryLine = dto.StoryLine;
            movie.GenreId = dto.GenreId;

         _moviesService.Update(movie);
            return Ok(movie);




        }

















    }
}
