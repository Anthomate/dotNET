namespace BookStoreAPI.Entities.BookEntities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public Author Author { get; set; } = default!;
        public Publisher Publisher { get; set; } = default!;
        public DateTime PublicationDate { get; set; }
        public double Price { get; set; }
        public string ISBN { get; set; } = default!;
        public Genre BookGenre { get; set; }
    }
}