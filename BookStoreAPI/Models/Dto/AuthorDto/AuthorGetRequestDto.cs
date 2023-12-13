namespace BookStoreAPI.Models.Dto.AuthorDto
{
    public class AuthorGetRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
