using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasedeDatos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using BasedeDatos.MysqlModels;
using Npgsql;

namespace BasedeDatos.Controllers
{
    public class MysqlController : Controller
    {
        private readonly BdmysqlContext _context;

        public MysqlController(BdmysqlContext context)
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
                    .FromSqlRaw("CALL buscar_provincia({0})", searchQuery)
                    .ToListAsync();
            }
            else if (tableName == "Cantones")
            {
                ViewBag.Data = await _context.Cantones
                    .FromSqlRaw("CALL buscar_canton({0})", searchQuery)
                    .ToListAsync();
            }
            else if (tableName == "Parroquias")
            {
                ViewBag.Data = await _context.Parroquias
                    .FromSqlRaw("CALL buscar_parroquia({0})", searchQuery)
                    .ToListAsync();
            }
            else if (tableName == "Regiones")
            {
                ViewBag.Data = await _context.Regiones
                    .FromSqlRaw("CALL buscar_region({0})", searchQuery)
                    .ToListAsync();
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntry(string tableName, int? IdProv, string nombre, string region, int? IdCan, int? IdPar)
        {
            // Obtener la cadena de conexión y abrir la conexión
            using var connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            // Verificar el nombre de la tabla y ejecutar el procedimiento almacenado correspondiente
            if (tableName == "Provincias")
            {
                using var command = new NpgsqlCommand("crear_provincia", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("p_id_prov", IdProv ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_prov_descrip", string.IsNullOrWhiteSpace(nombre) ? (object)DBNull.Value : nombre);
                command.Parameters.AddWithValue("p_id_reg", string.IsNullOrWhiteSpace(region) ? (object)DBNull.Value : region);
                await command.ExecuteNonQueryAsync();
            }
            else if (tableName == "Cantones")
            {
                using var command = new NpgsqlCommand("crear_canton", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("p_id_can", IdCan ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_can_descrip", string.IsNullOrWhiteSpace(nombre) ? (object)DBNull.Value : nombre);
                command.Parameters.AddWithValue("p_id_prov", IdProv ?? (object)DBNull.Value);
                await command.ExecuteNonQueryAsync();
            }
            else if (tableName == "Parroquias")
            {
                using var command = new NpgsqlCommand("crear_parroquia", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("p_id_par", IdPar ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_par_descrip", string.IsNullOrWhiteSpace(nombre) ? (object)DBNull.Value : nombre);
                command.Parameters.AddWithValue("p_id_can", IdCan ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("p_id_prov", IdProv ?? (object)DBNull.Value);
                await command.ExecuteNonQueryAsync();
            }
            else if (tableName == "Regiones")
            {
                using var command = new NpgsqlCommand("crear_region", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("p_id_reg", IdProv ?? (object)DBNull.Value); // Ajusta según la estructura de la tabla
                command.Parameters.AddWithValue("p_reg_descrip", string.IsNullOrWhiteSpace(nombre) ? (object)DBNull.Value : nombre);
                await command.ExecuteNonQueryAsync();
            }
            else
            {
                // Manejar el caso cuando el nombre de la tabla no es válido
                ModelState.AddModelError(string.Empty, "Nombre de tabla no válido");
                return View("Error"); // O alguna vista de error específica
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> EditProvincia(int idProv, string provDescrip, int? idReg)
        {
            using var connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("actualizar_provincia", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("@p_id_prov", MySqlDbType.Int32).Value = idProv;
            command.Parameters.Add("@p_prov_descrip", MySqlDbType.VarChar).Value = provDescrip;
            command.Parameters.Add("@p_id_reg", MySqlDbType.Int32).Value = idReg ?? (object)DBNull.Value;

            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> EditCanton(int idCan, string canDescrip, int idProv)
        {
            using var connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("actualizar_canton", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("@p_id_can", MySqlDbType.Int32).Value = idCan;
            command.Parameters.Add("@p_can_descrip", MySqlDbType.VarChar).Value = canDescrip;
            command.Parameters.Add("@p_id_prov", MySqlDbType.Int32).Value = idProv;

            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditParroquia(int idPar, string parDescrip, int idCan, int idProv)
        {
            using var connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("actualizar_parroquia", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("@p_id_par", MySqlDbType.Int32).Value = idPar;
            command.Parameters.Add("@p_par_descrip", MySqlDbType.VarChar).Value = parDescrip;
            command.Parameters.Add("@p_id_can", MySqlDbType.Int32).Value = idCan;
            command.Parameters.Add("@p_id_prov", MySqlDbType.Int32).Value = idProv;

            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditRegione(int idReg, string regDescrip)
        {
            using var connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("actualizar_region", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("@p_id_reg", MySqlDbType.Int32).Value = idReg;
            command.Parameters.Add("@p_reg_descrip", MySqlDbType.VarChar).Value = regDescrip;

            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProvincia(int idProv)
        {
            using var connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("eliminar_provincia", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("p_id_prov", idProv);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCanton(int idCan)
        {
            using var connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("eliminar_canton", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("p_id_can", idCan);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteParroquia(int idPar)
        {
            using var connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("eliminar_parroquia", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("p_id_par", idPar);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRegione(int idReg)
        {
            using var connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("eliminar_region", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("p_id_reg", idReg);
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Filter(string combination)
        {
            switch (combination)
            {
                case "Regiones":
                    ViewBag.Fields = new[] { "Regionion", "Region" };
                    break;
                case "Provincias":
                    ViewBag.Fields = new[] { "Regionion", "Region", "IdProvincia", "Provincia" };
                    break;
                case "Cantones":
                    ViewBag.Fields = new[] { "Regionion", "Region", "IdProvincia", "Provincia", "IdCanton", "Canton" };
                    break;
                case "Parroquias":
                    ViewBag.Fields = new[] { "Regionion", "Region", "IdProvincia", "Provincia", "IdCanton", "Canton", "IdParroquia", "Parroquia" };
                    break;
                default:
                    ViewBag.Fields = new string[0];
                    break;
            }
            return View("Filter");
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteFilter(string combination, string filterValue)
        {
            ViewBag.Combination = combination;
            ViewBag.FilterValue = filterValue;

            if (combination == "Regiones")
            {
                ViewBag.Result = await _context.Provincias
                    .FromSqlRaw("CALL filtro_regiones({0})", filterValue)
                    .ToListAsync();
            }
            else if (combination == "Provincias")
            {
                ViewBag.Result = await _context.Provincias
                    .FromSqlRaw("CALL filtro_provincias({0})", filterValue)
                    .ToListAsync();
            }
            else if (combination == "Cantones")
            {
                ViewBag.Result = await _context.Cantones
                    .FromSqlRaw("CALL filtro_cantones({0})", filterValue)
                    .ToListAsync();
            }
            else if (combination == "Parroquias")
            {
                ViewBag.Result = await _context.Provincias
                    .FromSqlRaw("CALL filtro_parroquias({0})", filterValue)
                    .ToListAsync();
            }

            return View("Filter");
        }
    }
}
