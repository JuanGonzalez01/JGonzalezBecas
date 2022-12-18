using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class AlumnoController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public AlumnoController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Alumno alumno = new ML.Alumno();
            alumno.Beca = new ML.Beca();

            //ML.Result result = BL.Alumno.GetAll(alumno);
            ML.Result result = GetAllAPI(alumno);
            //ML.Result resultBeca = BL.Beca.GetAll();
            ML.Result resultBeca = BecaGetAllAPI();


            if (result.Correct)
            {
                alumno.Alumnos = result.Objects;
                alumno.Beca.Becas = resultBeca.Objects;

                return View(alumno);
            }
            else
            {
                ViewBag.Message = $"Error: {result.Message}";
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetAll(ML.Alumno alumno)
        {
            alumno.Beca = new ML.Beca();

            //ML.Result result = BL.Alumno.GetAll(alumno);
            ML.Result result = GetAllAPIPost(alumno);

            //ML.Result resultBeca = BL.Beca.GetAll();
            ML.Result resultBeca = BecaGetAllAPI();


            if (result.Correct)
            {
                alumno.Alumnos = result.Objects;
                alumno.Beca.Becas = resultBeca.Objects;

                return View(alumno);
            }
            else
            {
                ViewBag.Message = $"Error: {result.Message}";

                return PartialView("Modal");
            }
        }

        [HttpGet]
        public ActionResult Form(int? IdAlumno)
        {
            ML.Alumno alumno = new ML.Alumno();

            //ML.Result resultBeca = BL.Beca.GetAll();
            ML.Result resultBeca = BecaGetAllAPI();

            if (IdAlumno == null)
            {
                //Formulario vacio pal ADD
                alumno.Beca = new ML.Beca();
            }
            else
            {
                //Formulario lleno por GetByID
                //ML.Result result = BL.Alumno.GetById(IdAlumno.Value);
                ML.Result result = GetByIdAPI(IdAlumno.Value);

                if (result.Correct)
                {
                    alumno = (ML.Alumno)result.Object;
                }
                else
                {
                    ViewBag.Message = $"Error: {result.Message}";
                }
            }

            alumno.Beca.Becas = resultBeca.Objects;

            return View(alumno);
        }

        [HttpPost]
        public ActionResult Form(ML.Alumno alumno)
        {
            if (alumno.IdAlumno == 0)
            {
                //ML.Result result = BL.Alumno.Add(alumno);
                ML.Result result = AddAPI(alumno);

                if (result.Correct)
                {
                    ViewBag.Message = "Alumno agregado con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.Message}";
                }
            }
            else
            {
                ML.Result result = UpdateAPI(alumno);
                //ML.Result result = BL.Alumno.Update(alumno);

                if (result.Correct)
                {
                    ViewBag.Message = "Alumno modificado con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.Message}";
                }
            }
            return PartialView("Modal");
        }

        [HttpGet]
        public ActionResult Delete(int? IdAlumno)
        {
            //ML.Result result = BL.Alumno.Delete(IdAlumno.Value);
            ML.Result result = DeleteAPI(IdAlumno.Value);

            if (result.Correct)
            {
                ViewBag.Message = "Alumno eliminado con éxito.";
            }
            else
            {
                ViewBag.Message = $"Error: {result.Message}";
            }
            return PartialView("Modal");
        }

        public ML.Result BecaGetAllAPI()
        {
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    var responseTask = client.GetAsync("Beca/GetAll");

                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Beca resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Beca>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }

                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public ML.Result GetAllAPI(ML.Alumno alumno)
        {
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    var responseTask = client.GetAsync("Alumno/GetAll");

                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Alumno resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Alumno>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }

                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public ML.Result GetAllAPIPost(ML.Alumno alumno)
        {
            alumno = InicializarAlumno(alumno);
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                using (var client = new HttpClient())
                {
                    string urlAPI = _configuration["UrlAPI"];
                    client.BaseAddress = new Uri(urlAPI);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ML.Alumno>("Alumno/GetAll", alumno);
                    postTask.Wait();

                    var resultAPI = postTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Alumno resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Alumno>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No existen registros.";
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

        public ML.Result GetByIdAPI(int IdAlumno)
        {
            ML.Result result = new ML.Result();

            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);
                    var responseTask = client.GetAsync("Alumno/GetbyId/" + IdAlumno);
                    responseTask.Wait();
                    var resultAPI = responseTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Alumno resultItemList = new ML.Alumno();
                        resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Alumno>(readTask.Result.Object.ToString());
                        result.Object = resultItemList;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No existen registros.";
                    }

                }
            }

            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;

            }

            return result;
        }

        public ML.Result AddAPI(ML.Alumno alumno)
        {
            alumno = InicializarAlumno(alumno);
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                using (var client = new HttpClient())
                {
                    string urlAPI = _configuration["UrlAPI"];
                    client.BaseAddress = new Uri(urlAPI);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ML.Alumno>("Alumno/Add", alumno);
                    postTask.Wait();

                    var resultAPI = postTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se agregaron registros.";
                    }

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
                result.Ex=ex;
            }

            return result;
        }

        public ML.Result UpdateAPI(ML.Alumno alumno)
        {
            alumno = InicializarAlumno(alumno);
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                using (var client = new HttpClient())
                {
                    string urlAPI = _configuration["UrlAPI"];
                    client.BaseAddress = new Uri(urlAPI);

                    //HTTP POST
                    var postTask = client.PutAsJsonAsync<ML.Alumno>("Alumno/Update", alumno);
                    postTask.Wait();

                    var resultAPI = postTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No existen registros.";
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

        public ML.Result DeleteAPI(int IdAlumno)
        {
            ML.Result result = new ML.Result();

            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);
                    var responseTask = client.DeleteAsync("Alumno/Delete/" + IdAlumno);
                    responseTask.Wait();
                    var resultAPI = responseTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No existen registros.";
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

        public ML.Alumno InicializarAlumno (ML.Alumno alumno)
        {
            alumno.Nombre = (alumno.Nombre == null) ? "" : alumno.Nombre;
            alumno.ApellidoPaterno = (alumno.ApellidoPaterno == null) ? "" : alumno.ApellidoPaterno;
            alumno.ApellidoMaterno = (alumno.ApellidoMaterno == null) ? "" : alumno.ApellidoMaterno;
            alumno.Alumnos = new List<object>();

            alumno.Beca = (alumno.Beca == null) ? new ML.Beca() : alumno.Beca;
            alumno.Beca.Nombre = (alumno.Beca.Nombre == null) ? "" : alumno.Beca.Nombre;
            alumno.Beca.Becas = new List<object>();

            alumno.BecasBusqueda = (alumno.BecasBusqueda == null) ? new List<int>() : alumno.BecasBusqueda;

            return alumno;
        }
    }
}
