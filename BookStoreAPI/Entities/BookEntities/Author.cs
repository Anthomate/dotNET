using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Entities.BookEntities
{
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le nom de l'auteur est requis.")]
        [StringLength(100, ErrorMessage = "Le nom de l'auteur ne peut pas dépasser 100 caractères.")]
        public string Name { get; set; } = default!;
    }
}