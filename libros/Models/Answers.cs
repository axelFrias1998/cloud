using System.Collections.Generic;

namespace libros.Models
{
    public class Responses
    {
        public List<int> PrimeroPares { get; set; }

        public List<int> PrimeroImpares { get; set; }

        public int SegundoHombres { get; set; }

        public int SegundoMujeres { get; set; }

        public int SegundoMenorTreinta { get; set; }

        public int SegundoMayorTreinta { get; set; }

        public List<Usuario> SegundoUsuarios { get; set; }
    }
}