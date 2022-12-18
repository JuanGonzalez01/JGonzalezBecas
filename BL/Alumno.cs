using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Alumno
    {
        public static ML.Result Add(ML.Alumno alumno)
        {
            ML.Result result= new ML.Result();

            try
            {
                using (DL.JgonzalezBecasContext context = new DL.JgonzalezBecasContext())
                {
                    DL.Alumno alumnoDL = new DL.Alumno();

                    alumnoDL.Nombre = alumno.Nombre;
                    alumnoDL.ApellidoPaterno = alumno.ApellidoPaterno;
                    alumnoDL.ApellidoMaterno = alumno.ApellidoMaterno;
                    alumnoDL.Genero = alumno.Genero;
                    alumnoDL.Edad = alumno.Edad;
                    alumnoDL.IdBeca = alumno.Beca.IdBeca;
                    
                    context.Alumnos.Add( alumnoDL );
                    context.SaveChanges();

                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
                result.Ex= ex;
            }

            return result;
        }

        public static ML.Result Update(ML.Alumno alumno)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezBecasContext context = new DL.JgonzalezBecasContext())
                {
                    var query = (from Alumno in context.Alumnos
                                 where Alumno.IdAlumno == alumno.IdAlumno
                                 select Alumno).SingleOrDefault();

                    if (query != null)
                    {
                        query.Nombre = alumno.Nombre;
                        query.ApellidoPaterno = alumno.ApellidoPaterno;
                        query.ApellidoMaterno = alumno.ApellidoMaterno;
                        query.Genero = alumno.Genero;
                        query.Edad = alumno.Edad;
                        query.IdBeca = alumno.Beca.IdBeca;

                        context.SaveChanges();
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se modificaron registros";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result GetAll(ML.Alumno alumnoBusqueda)
        {
            ML.Result result = new ML.Result();

            if (alumnoBusqueda.BecasBusqueda==null || alumnoBusqueda.BecasBusqueda.Count==0)
            {
                alumnoBusqueda.BecasBusqueda = new List<int> { 1,2,3 };
            }
            else
            {
                alumnoBusqueda.BecasBusqueda.AddRange(new List<int> { 0, 0 });
            }

            try
            {

                using (DL.JgonzalezBecasContext context = new DL.JgonzalezBecasContext())
                {
                    var query = (from Alumno in context.Alumnos
                                 join Beca in context.Becas on Alumno.IdBeca equals Beca.IdBeca
                                 where Alumno.IdBeca == alumnoBusqueda.BecasBusqueda[0] || Alumno.IdBeca == alumnoBusqueda.BecasBusqueda[1] || Alumno.IdBeca == alumnoBusqueda.BecasBusqueda[2]
                                 select new { IdAlumno = Alumno.IdAlumno, Nombre = Alumno.Nombre, ApellidoPaterno=Alumno.ApellidoPaterno,
                                                ApellidoMaterno = Alumno.ApellidoMaterno, Genero = Alumno.Genero, Edad = Alumno.Edad,
                                                IdBeca=Alumno.IdBeca, BecaNombre=Beca.Nombre });

                    if (query != null && query.ToList().Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var column in query)
                        {
                            ML.Alumno alumno = new ML.Alumno();

                            alumno.IdAlumno = column.IdAlumno;
                            alumno.Nombre= column.Nombre;
                            alumno.ApellidoPaterno= column.ApellidoPaterno;
                            alumno.ApellidoMaterno= column.ApellidoMaterno;
                            alumno.Genero = column.Genero.Value;
                            alumno.Edad = column.Edad.Value;

                            alumno.Beca = new ML.Beca();
                            alumno.Beca.IdBeca = column.IdBeca.Value;
                            alumno.Beca.Nombre = column.BecaNombre;

                            result.Objects.Add(alumno);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se encontraron registros";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message= ex.Message;
                result.Ex= ex;
            }

            return result;
        }

        public static ML.Result GetById(int idAlumno)
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL.JgonzalezBecasContext context = new DL.JgonzalezBecasContext())
                {
                    var query = (from Alumno in context.Alumnos
                                 join Beca in context.Becas on Alumno.IdBeca equals Beca.IdBeca
                                 where Alumno.IdAlumno == idAlumno
                                 select new
                                 {
                                     IdAlumno = Alumno.IdAlumno,
                                     Nombre = Alumno.Nombre,
                                     ApellidoPaterno = Alumno.ApellidoPaterno,
                                     ApellidoMaterno = Alumno.ApellidoMaterno,
                                     Genero = Alumno.Genero,
                                     Edad = Alumno.Edad,
                                     IdBeca = Alumno.IdBeca,
                                     BecaNombre = Beca.Nombre
                                 }).SingleOrDefault();

                    if (query != null)
                    {
                        ML.Alumno alumno = new ML.Alumno();

                        alumno.IdAlumno = query.IdAlumno;
                        alumno.Nombre = query.Nombre;
                        alumno.ApellidoPaterno = query.ApellidoPaterno;
                        alumno.ApellidoMaterno = query.ApellidoMaterno;
                        alumno.Genero = query.Genero.Value;
                        alumno.Edad = query.Edad.Value;

                        alumno.Beca = new ML.Beca();
                        alumno.Beca.IdBeca = query.IdBeca.Value;
                        alumno.Beca.Nombre = query.BecaNombre;

                        result.Object = alumno;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se encontraron registros";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result Delete(int idAlumno)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezBecasContext context = new DL.JgonzalezBecasContext())
                {
                    var query = (from Alumno in context.Alumnos
                                 where Alumno.IdAlumno == idAlumno
                                 select Alumno).First();

                    context.Alumnos.Remove(query);
                    context.SaveChanges();

                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
    }
}