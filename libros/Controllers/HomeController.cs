using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using libros.Models;

namespace libros.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var answer = new Answers();

            //Primera pregunta 
            int[] numeros = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};
            answer.PrimeroImpares = Array.FindAll(numeros, x => x % 2 != 0);
            answer.PrimeroPares = Array.FindAll(numeros, x => x % 2 == 0);
//1F1F24
            //Segunda pregunta    
            List<Usuario> listUsuarios = new List<Usuario>();
            listUsuarios.Add(new Usuario{ IdUsuario = 1, Nombre = "Francisco Alvarez", Edad = 20, Sexo = 'H'});
            listUsuarios.Add(new Usuario{ IdUsuario = 3, Nombre = "Maria Gonzalez", Edad = 30, Sexo = 'M'});
            listUsuarios.Add(new Usuario{ IdUsuario = 4, Nombre = "Karla Fernandez", Edad = 24, Sexo = 'M'});
            listUsuarios.Add(new Usuario{ IdUsuario = 11, Nombre = "Andres Morales", Edad = 28, Sexo = 'H'});
            listUsuarios.Add(new Usuario{ IdUsuario = 13, Nombre = "Jose Garcia", Edad = 23, Sexo = 'H'});

            answer.SegundoHombres = (from usuario in listUsuarios
                                    where usuario.Sexo == 'H'
                                    select usuario).Count();
            answer.SegundoMujeres = (from usuario in listUsuarios
                                    where usuario.Sexo == 'M'
                                    select usuario).Count();
            answer.SegundoMayorTreinta = (from usuario in listUsuarios
                                    where usuario.Edad >= 30
                                    select usuario).Count();
            answer.SegundoMenorTreinta = (from usuario in listUsuarios
                                    where usuario.Edad >= 20 && usuario.Edad < 30
                                    select usuario).Count();
            int[] ids = {1, 4, 13};
            answer.SegundoUsuarios = from usuario in listUsuarios
                                    where ids.Contains(usuario.IdUsuario)
                                    select usuario;

            return View(answer);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
