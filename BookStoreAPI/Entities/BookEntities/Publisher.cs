using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Entities.BookEntities
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le nom de l'éditeur est requis.")]
        [StringLength(150, ErrorMessage = "Le nom de l'éditeur ne peut pas dépasser 150 caractères.")]
        public string Name { get; set; } = default!;
    }
}