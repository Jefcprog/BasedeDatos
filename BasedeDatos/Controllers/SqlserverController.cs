using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using BasedeDatos.SqlServerModels;

namespace BasedeDatos.Controllers
{
    public class SqlServerController : Controller
    {
        private readonly BdsqlserverContext _context;

        public SqlServerController(BdsqlserverContext context)
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

            if (tableName == "Provincias")
            {
                ViewBag.Data = await _context.Provincias.ToListAsync();
            }
            else if (tableName == "Cantones")
            {
                ViewBag.Data = await _context.Cantones.ToListAsync();
            }
            else if (tableName == "Parroquias")
            {
                ViewBag.Data = await _context.Parroquias.ToListAsync();
            }
            else if (tableName == "Regiones")
            {
                ViewBag.Data = await _context.Regiones.ToListAsync();
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SearchTable(string tableName, string searchQuery)
        {
            ViewBag.TableName = tableName;
            ViewBag.ShowForm = true;

            if (tableName == "Provincias")
            {
                ViewBag.Data = await _context.Provincias
                    .FromSqlRaw("EXEC buscar_provincia @searchQuery", new SqlParameter("@searchQuery", searchQuery))
                    .ToListAsync();
            }
            else if (tableName == "Cantones")
            {
                ViewBag.Data = await _context.Cantones
                    .FromSqlRaw("EXEC buscar_canton @searchQuery", new SqlParameter("@searchQuery", searchQuery))
                    .ToListAsync();
            }
            else if (tableName == "Parroquias")
            {
                ViewBag.Data = await _context.Parroquias
                    .FromSqlRaw("EXEC buscar_parroquia @searchQuery", new SqlParameter("@searchQuery", searchQuery))
                    .ToListAsync();
            }
            else if (tableName == "Regiones")
            {
                ViewBag.Data = await _context.Regiones
                    .FromSqlRaw("EXEC buscar_region @searchQuery", new SqlParameter("@searchQuery", searchQuery))
                    .ToListAsync();
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntry(string tableName, int? IdProv, string nombre, string region, int? IdCan, int? IdPar)
        {
            using var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            if (tableName == "Provincias")
            {
                using var command = new SqlCommand("crear_provincia", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@p_id_prov", IdProv ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@p_prov_descrip", string.IsNullOrWhiteSpace(nombre) ? (object)DBNull.Value : nombre);
                command.Parameters.AddWithValue("@p_id_reg", string.IsNullOrWhiteSpace(region) ? (object)DBNull.Value : region);
                await command.ExecuteNonQueryAsync();
            }
            else if (tableName == "Cantones")
            {
                using var command = new SqlCommand("crear_canton", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@p_id_can", IdCan ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@p_can_descrip", string.IsNullOrWhiteSpace(nombre) ? (object)DBNull.Value : nombre);
                command.Parameters.AddWithValue("@p_id_prov", IdProv ?? (object)DBNull.Value);
                await command.ExecuteNonQueryAsync();
            }
            else if (tableName == "Parroquias")
            {
                using var command = new SqlCommand("crear_parroquia", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@p_id_par", IdPar ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@p_par_descrip", string.IsNullOrWhiteSpace(nombre) ? (object)DBNull.Value : nombre);
                command.Parameters.AddWithValue("@p_id_can", IdCan ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@p_id_prov", IdProv ?? (object)DBNull.Value);
                await command.ExecuteNonQueryAsync();
            }
            else if (tableName == "Regiones")
            {
                using var command = new SqlCommand("crear_region", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@p_id_reg", IdProv ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@p_reg_descrip", string.IsNullOrWhiteSpace(nombre) ? (object)DBNull.Value : nombre);
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditProvincia(int idProv, string provDescrip, int? idReg)
        {
            using var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("actualizar_provincia", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@p_id_prov", idProv);
            command.Parameters.AddWithValue("@p_prov_descrip", provDescrip);
            command.Parameters.AddWithValue("@p_id_reg", idReg ?? (object)DBNull.Value);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditCanton(int idCan, string canDescrip, int idProv)
        {
            using var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("actualizar_canton", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@p_id_can", idCan);
            command.Parameters.AddWithValue("@p_can_descrip", canDescrip);
            command.Parameters.AddWithValue("@p_id_prov", idProv);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditParroquia(int idPar, string parDescrip, int idCan, int idProv)
        {
            using var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("actualizar_parroquia", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@p_id_par", idPar);
            command.Parameters.AddWithValue("@p_par_descrip", parDescrip);
            command.Parameters.AddWithValue("@p_id_can", idCan);
            command.Parameters.AddWithValue("@p_id_prov", idProv);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditRegione(int idReg, string regDescrip)
        {
            using var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("actualizar_region", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@p_id_reg", idReg);
            command.Parameters.AddWithValue("@p_reg_descrip", regDescrip);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProvincia(int idProv)
        {
            using var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("eliminar_provincia", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@p_id_prov", idProv);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCanton(int idCan)
        {
            using var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("eliminar_canton", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@p_id_can", idCan);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteParroquia(int idPar)
        {
            using var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("eliminar_parroquia", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@p_id_par", idPar);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRegione(int idReg)
        {
            using var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("eliminar_region", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@p_id_reg", idReg);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }
    }
}
