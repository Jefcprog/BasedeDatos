using BasedeDatos.MysqlModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasedeDatos.Controllers
{
    public class GestionController : Controller
    {
        private readonly BdmysqlContext _context;

        public GestionController(BdmysqlContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new GestionViewModel
            {
                Regiones = await _context.Regiones.ToListAsync(),
                Provincias = await _context.Provincias.ToListAsync(),
                Cantones = await _context.Cantones.ToListAsync(),
                Parroquias = await _context.Parroquias.ToListAsync()
            };
            return View(model);
        }
    }

    public class GestionViewModel
    {
        public List<Regione> Regiones { get; set; } = new List<Regione>();
        public List<Provincia> Provincias { get; set; } = new List<Provincia>();
        public List<Cantone> Cantones { get; set; } = new List<Cantone>();
        public List<Parroquia> Parroquias { get; set; } = new List<Parroquia>();
    }
}