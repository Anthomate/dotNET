using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreAPI.Entities.BookEntities
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le titre du livre est requis.")]
        [StringLength(200, ErrorMessage = "Le titre du livre ne peut pas dépasser 200 caractères.")]
        public string Title { get; set; } = default!;
        [Required(ErrorMessage = "L'auteur du livre est requis.")]
        public Author Author { get; set; } = default!;
        [Required(ErrorMessage = "L'éditeur du livre est requis.")]
        public Publisher Publisher { get; set; } = default!;
        [Required(ErrorMessage = "La date de publication du livre est requise.")]
        public DateTime PublicationDate { get; set; }
        [Required(ErrorMessage = "Le prix du livre est requis.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à zéro.")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Le numéro ISBN du livre est requis.")]
        [StringLength(13, ErrorMessage = "Le numéro ISBN doit avoir une longueur de 13 caractères.")]
        public string ISBN { get; set; } = default!;
        [Required(ErrorMessage = "Le genre du livre est requis.")]
        public Genre BookGenre { get; set; } = default!;
    }
}