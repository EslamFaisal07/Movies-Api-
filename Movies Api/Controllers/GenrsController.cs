using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_Api.Services;



namespace Movies_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenrsController(IGenresService  _genresService) : ControllerBase
    {


        [HttpGet]

        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genresService.GetAll();
            return Ok(genres);

        }


        [HttpPost]

        public async Task<IActionResult> CreateAsync([FromBody]CreateGenreDto createGenreDto)
        {
            var genre = new Genre()
            {
                Name = createGenreDto.Name
            };
            await _genresService.Add(genre);
            return Ok(genre);
          
        }



        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateAsync(byte id , [FromBody] CreateGenreDto dto )
        {
            var genre = await _genresService.GetById(id);
            if (genre == null)
            {
                return NotFound($" No Genre was found with Id {id}");
            }
            genre.Name = dto.Name;
            _genresService.Update(genre);
            return Ok(genre);

        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genresService.GetById( id);

            if (genre is null)
            {
                return NotFound($" No Genre was found with Id {id}");
            }
            _genresService.Delete(genre);
            return Ok(genre);
        }






    }
}
