namespace BookStoreAPI.Entities
{
    //public enum Genre
    //{
    //    Fiction = 0,
    //    Mystery = 1,
    //    Romance = 2,
    //    ScienceFiction = 3,
    //    Fantasy = 4,
    //    NonFiction = 5,
    //    Biography = 6,
    //    History = 7,
    //    Poetry = 8
    //}

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