using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Alumno
    {
        public int IdAlumno { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public bool Genero { get; set; }
        public int Edad { get; set; }
        public List<Object> Alumnos { get; set; }
        public Beca Beca { get; set; }

        public List<int> BecasBusqueda { get; set; }
    }
}
