using Microsoft.AspNetCore.Mvc;

namespace BasedeDatos.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetDbEngine(string dbEngine)
        {
            if (dbEngine == "MySQL")
            {
                return RedirectToAction("Index", "Mysql");
            }
            else if (dbEngine == "SQLServer")
            {
                return RedirectToAction("Index", "SqlServer");
            }
            else if (dbEngine == "PostgreSQL")
            {
                return RedirectToAction("Index", "Postgresql");
            }
            else if (dbEngine == "OracleXE")
            {
                return RedirectToAction("Index", "Oraclexe");
            }
            else
            {
                ModelState.AddModelError("", "Debe seleccionar un motor de base de datos válido.");
                return View("Index");
            }
        }
    }
}