namespace BookStoreAPI.Models.Dto.GenreDto
{
    public class GenreGetRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
    }
}
