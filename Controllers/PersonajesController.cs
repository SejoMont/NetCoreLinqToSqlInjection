using Microsoft.AspNetCore.Mvc;
using NetCoreLinqToSqlInjection.Models;
using NetCoreLinqToSqlInjection.Repositories;

namespace NetCoreLinqToSqlInjection.Controllers
{
    public class PersonajesController : Controller
    {
        private IRepositoryPersonajes repo;
        public PersonajesController(IRepositoryPersonajes repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            List<Personaje> personajes = this.repo.GetPersonajes();
            return View(personajes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Personaje personaje)
        {
            this.repo.InsertPersonaje(personaje.IdPersonaje, personaje.Nombre, personaje.Imagen);
            return RedirectToAction("Index");
        }
    }
}
