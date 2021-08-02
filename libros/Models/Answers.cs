using System.Collections.Generic;

namespace libros.Models
{
    public class Answers
    {
        public int[] PrimeroPares { get; set; }

        public int[] PrimeroImpares { get; set; }

        public int SegundoHombres { get; set; }

        public int SegundoMujeres { get; set; }

        public int SegundoMenorTreinta { get; set; }

        public int SegundoMayorTreinta { get; set; }

        public IEnumerable<Usuario> SegundoUsuarios { get; set; }

        public IEnumerable<string> Blobs { get; set; }
    }
}