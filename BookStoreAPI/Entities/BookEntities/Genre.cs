using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Entities.BookEntities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le titre du genre est requis.")]
        [StringLength(50, ErrorMessage = "Le titre du genre ne peut pas dépasser 50 caractères.")]
        public string Title { get; set; } = default!;
    }
}