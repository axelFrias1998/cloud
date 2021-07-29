using System.ComponentModel.DataAnnotations;

namespace libros.Models
{
    public class Libro
    {
        [Key]
        public int IdLibro { get; set; }

        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(100)]
        public string Escritor { get; set; }

        [Required]
        public int Existencia { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:$#.##}")]
        public float Precio { get; set; }

        [Required]
        public string Genero { get; set; }
    }
}