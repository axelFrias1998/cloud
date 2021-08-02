using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using libros.Models;
using libros.Contexts;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace libros.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
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
            answer.Blobs = await GetFiles();
            return View(answer);
        }

        // /Home/AjaxForm/2
        [HttpPost]
        public ActionResult AjaxForm(int id)
        {
            return Json($"Ingresaste el ID: {id}");
        }
        
        public async Task<IEnumerable<string>> GetFiles()
        {
            var containerClient = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=cloudblobtest;AccountKey=hhpoUFX2OX3dgBun2GSwalF5Yd/HPpCZrpGrLNM/PhOK78ovV1S1YIcj76CH2NFlTkl2sVPP7m8RY1/MdPtAwQ==;EndpointSuffix=core.windows.net", "files");
            var items = new List<string>();
            await foreach(var blobItem in containerClient.GetBlobsAsync())
            {
                items.Add(blobItem.Name);
            }
            return items;
        }

        [HttpPost]
        public IActionResult Upload()
        {
            var containerClient = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=cloudblobtest;AccountKey=hhpoUFX2OX3dgBun2GSwalF5Yd/HPpCZrpGrLNM/PhOK78ovV1S1YIcj76CH2NFlTkl2sVPP7m8RY1/MdPtAwQ==;EndpointSuffix=core.windows.net", "files");
            
            foreach (var file in Request.Form.Files)
            {
                var blocClient = containerClient.GetBlobClient(file.FileName);
                using(var fileStream = file.OpenReadStream())
                {
                    blocClient.Upload(fileStream);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CrearLibro(Libro libro)
        {
            _context.Add(libro);
            _context.SaveChanges();
            return Json(new { Ok = true});
        }

        public PartialViewResult GetPartialLibro(int id)
        {
            var libro = _context.Libros.Where(x => x.IdLibro == id);
            return PartialView("_VerLibro", libro);
        }


        public async Task<PartialViewResult> ActualizarLibro(Libro libro)
        {
            var result = await _context.Libros.FirstOrDefaultAsync(x => x.IdLibro == libro.IdLibro);
            if(result is not null)
            {
                result.Escritor = (string.IsNullOrEmpty(libro.Escritor)) ? result.Escritor : libro.Escritor;
                result.Titulo = (string.IsNullOrEmpty(libro.Titulo)) ? result.Titulo : libro.Titulo;
                result.Genero = (string.IsNullOrEmpty(libro.Genero)) ? result.Genero : libro.Genero;
                result.Precio = (libro.Precio > 0) ? libro.Precio : result.Precio;
                result.Existencia = (libro.Existencia >= 0) ? libro.Existencia : result.Existencia;
                await _context.SaveChangesAsync(); 
            }
            return PartialView("_VerLibro", _context.Libros.Where(x => x.IdLibro == libro.IdLibro));
        }

        public PartialViewResult GetPartialLibros()
        {
            var libros = _context.Libros.ToList();
            return PartialView("_VerLibro", libros);
        }
    }
}
