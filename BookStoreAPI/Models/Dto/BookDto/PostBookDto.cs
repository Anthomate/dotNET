namespace BookStoreAPI.Models.Dto.BookDto
{
    public class PostBookDto
    {
        public string Title { get; set; } = default!;
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public DateTime PublicationDate { get; set; }
        public double Price { get; set; }
        public string ISBN { get; set; } = default!;
        public int GenreId { get; set; }
    }
}