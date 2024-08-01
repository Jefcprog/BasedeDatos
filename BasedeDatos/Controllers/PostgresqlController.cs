using Microsoft.AspNetCore.Mvc;
using Npgsql;
using BasedeDatos.PostgresqlModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BasedeDatos.Controllers
{
    public class PostgresqlController : Controller
    {
        private readonly BdpostgresqlContext _context;

        public PostgresqlController(BdpostgresqlContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.ShowForm = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable(string tableName)
        {
            ViewBag.TableName = tableName;
            ViewBag.ShowForm = true;

            IEnumerable<object> data = null;

            switch (tableName)
            {
                case "Provincias":
                    data = await _context.Provincias.ToListAsync();
                    break;
                case "Cantones":
                    data = await _context.Cantones.ToListAsync();
                    break;
                case "Parroquias":
                    data = await _context.Parroquias.ToListAsync();
                    break;
                default:
                    ModelState.AddModelError(string.Empty, "Nombre de tabla no válido");
                    return View("Error"); // O alguna vista de error específica
            }

            ViewBag.Data = data;
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SearchTable(string tableName, string searchQuery)
        {
            ViewBag.TableName = tableName;
            ViewBag.ShowForm = true;

            IEnumerable<object> data = null;

            switch (tableName)
            {
                case "Provincias":
                    data = await _context.Provincias
                        .Where(p => p.Nombre != null && p.Nombre.Contains(searchQuery))
                        .ToListAsync();
                    break;
                case "Cantones":
                    data = await _context.Cantones
                        .Where(c => c.Nombre != null && c.Nombre.Contains(searchQuery))
                        .ToListAsync();
                    break;
                case "Parroquias":
                    data = await _context.Parroquias
                        .Where(p => p.Nombre != null && p.Nombre.Contains(searchQuery))
                        .ToListAsync();
                    break;
                default:
                    ModelState.AddModelError(string.Empty, "Nombre de tabla no válido");
                    return View("Error"); // O alguna vista de error específica
            }

            ViewBag.Data = data;
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CreateEntry(string tableName, int? codProv, string nombre, int? region, int? codCan, int? codPar)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                ModelState.AddModelError(string.Empty, "El nombre no puede estar vacío.");
                return View("Error"); // O alguna vista de error específica
            }

            try
            {
                switch (tableName)
                {
                    case "Provincias":
                        if (region.HasValue)
                        {
                            var provincia = new Provincia
                            {
                                IdProv = codProv ?? 0, // Asumiendo que este campo puede ser autogenerado y no necesita valor
                                Nombre = nombre,
                                Region = region.Value // Asegúrate de que la región sea válida
                            };
                            _context.Provincias.Add(provincia);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "La región no puede estar vacía.");
                            return View("Error"); // O alguna vista de error específica
                        }
                        break;

                    case "Cantones":
                        if (codProv.HasValue && _context.Provincias.Any(p => p.IdProv == codProv))
                        {
                            var canton = new Cantone
                            {
                                IdCan = codCan ?? 0, // Asumiendo que este campo puede ser autogenerado y no necesita valor
                                Nombre = nombre,
                                IdProv = codProv.Value
                            };
                            _context.Cantones.Add(canton);
                        }
                        else
                        {
                            ModelState.AddModelError("IdProv", "La provincia especificada no existe.");
                            return View("Error"); // O alguna vista de error específica
                        }
                        break;

                    case "Parroquias":
                        if (codCan.HasValue && _context.Cantones.Any(c => c.IdCan == codCan))
                        {
                            var parroquia = new Parroquia
                            {
                                IdPar = codPar ?? 0, // Asumiendo que este campo puede ser autogenerado y no necesita valor
                                Nombre = nombre,
                                IdCan = codCan.Value,
                                IdProv = codProv ?? 0 // Puedes tener una lógica específica para el código de provincia si es necesario
                            };
                            _context.Parroquias.Add(parroquia);
                        }
                        else
                        {
                            ModelState.AddModelError("IdCan", "El cantón especificado no existe.");
                            return View("Error"); // O alguna vista de error específica
                        }
                        break;

                    default:
                        ModelState.AddModelError(string.Empty, "Nombre de tabla no válido");
                        return View("Error"); // O alguna vista de error específica
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception )
            {
                // Registra el error para diagnóstico
                // _logger.LogError(ex, "Error al crear la entrada.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error al procesar la solicitud.");
                return View("Error"); // O alguna vista de error específica
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditEntry(string tableName, int id, string nombre, int region, int? codCan, int? codProv)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                ModelState.AddModelError(string.Empty, "El nombre no puede estar vacío.");
                return View("Error"); // O alguna vista de error específica
            }

            switch (tableName)
            {
                case "Provincias":
                    var provincia = await _context.Provincias.FindAsync(id);
                    if (provincia != null)
                    {
                        provincia.Nombre = nombre;
                        provincia.Region = region;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Provincia no encontrada.");
                        return View("Error"); // O alguna vista de error específica
                    }
                    break;

                case "Cantones":
                    var canton = await _context.Cantones.FindAsync(id);
                    if (canton != null)
                    {
                        if (codProv.HasValue && _context.Provincias.Any(p => p.IdProv == codProv))
                        {
                            canton.Nombre = nombre;
                            canton.IdProv = codProv.Value;
                        }
                        else
                        {
                            ModelState.AddModelError("IdProv", "La provincia especificada no existe.");
                            return View("Error"); // O alguna vista de error específica
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Canton no encontrado.");
                        return View("Error"); // O alguna vista de error específica
                    }
                    break;

                case "Parroquias":
                    var parroquia = await _context.Parroquias.FindAsync(id);
                    if (parroquia != null)
                    {
                        if (codCan.HasValue && _context.Cantones.Any(c => c.IdCan == codCan))
                        {
                            parroquia.Nombre = nombre;
                            parroquia.IdCan = codCan.Value;
                            parroquia.IdProv = codProv ?? parroquia.IdProv;
                        }
                        else
                        {
                            ModelState.AddModelError("IdCan", "El cantón especificado no existe.");
                            return View("Error"); // O alguna vista de error específica
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Parroquia no encontrada.");
                        return View("Error"); // O alguna vista de error específica
                    }
                    break;

                default:
                    ModelState.AddModelError(string.Empty, "Nombre de tabla no válido");
                    return View("Error"); // O alguna vista de error específica
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEntry(string tableName, int id)
        {
            switch (tableName)
            {
                case "Provincias":
                    var provincia = await _context.Provincias.FindAsync(id);
                    if (provincia != null)
                    {
                        _context.Provincias.Remove(provincia);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Provincia no encontrada.");
                        return View("Error"); // O alguna vista de error específica
                    }
                    break;

                case "Cantones":
                    var canton = await _context.Cantones.FindAsync(id);
                    if (canton != null)
                    {
                        _context.Cantones.Remove(canton);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Canton no encontrado.");
                        return View("Error"); // O alguna vista de error específica
                    }
                    break;

                case "Parroquias":
                    var parroquia = await _context.Parroquias.FindAsync(id);
                    if (parroquia != null)
                    {
                        _context.Parroquias.Remove(parroquia);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Parroquia no encontrada.");
                        return View("Error"); // O alguna vista de error específica
                    }
                    break;

                default:
                    ModelState.AddModelError(string.Empty, "Nombre de tabla no válido");
                    return View("Error"); // O alguna vista de error específica
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
