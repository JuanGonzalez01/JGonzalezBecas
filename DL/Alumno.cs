using System;
using System.Collections.Generic;

namespace DL;

public partial class Alumno
{
    public int IdAlumno { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public bool? Genero { get; set; }

    public int? Edad { get; set; }

    public int? IdBeca { get; set; }

    public virtual Beca? IdBecaNavigation { get; set; }
}
