namespace Movies_Api.DTOS
{
    public class CreateGenreDto
    {

        [MaxLength(100)]
        public string Name { get; set; } 


    }
}
