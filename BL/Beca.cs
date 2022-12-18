using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Beca
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezBecasContext context = new DL.JgonzalezBecasContext())
                {
                    var query = (from Beca in context.Becas
                                 select new { IdBeca = Beca.IdBeca, Nombre = Beca.Nombre });

                    if (query != null && query.ToList().Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var column in query)
                        {
                            ML.Beca beca = new ML.Beca();

                            beca.IdBeca = column.IdBeca;
                            beca.Nombre = column.Nombre;

                            result.Objects.Add(beca);
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
                result.Message = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
    }
}
