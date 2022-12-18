﻿using System;
using System.Collections.Generic;

namespace DL;

public partial class Beca
{
    public int IdBeca { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Alumno> Alumnos { get; } = new List<Alumno>();
}
