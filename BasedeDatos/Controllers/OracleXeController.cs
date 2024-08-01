using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using BasedeDatos.OracleModels;
using Microsoft.EntityFrameworkCore;

namespace BasedeDatos.Controllers
{
    public class OracleXeController : Controller
    {
        private readonly ModelContext _context;

        public OracleXeController(ModelContext context)
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

            switch (tableName)
            {
                case "Provincias":
                    var oracleProvincias = await _context.Provincias.ToListAsync();
                    var provincias = oracleProvincias.Select(p => new BasedeDatos.Models.Provincia
                    {
                        IdProv = Convert.ToInt32(p.IdProv),
                        Nombre = p.Nombre,
                        Region = Convert.ToInt32(p.Region)
                    }).ToList();
                    ViewBag.Data = provincias;
                    break;

                case "Cantones":
                    var oracleCantones = await _context.Cantones.ToListAsync();
                    var cantones = oracleCantones.Select(c => new BasedeDatos.Models.Canton
                    {
                        IdCan = c.IdCan,
                        Nombre = c.Nombre,
                        IdProv = c.IdProv
                    }).ToList();
                    ViewBag.Data = cantones;
                    break;

                case "Parroquias":
                    var oracleParroquias = await _context.Parroquias.ToListAsync();
                    var parroquias = oracleParroquias.Select(p => new BasedeDatos.Models.Parroquia
                    {
                        IdPar = Convert.ToInt32(p.IdPar),
                        Nombre = p.Nombre,
                        IdCan = Convert.ToInt32(p.IdCan),
                        IdProv = Convert.ToInt32(p.IdProv)
                    }).ToList();
                    ViewBag.Data = parroquias;
                    break;

                case "Regiones":
                    var oracleRegiones = await _context.Regiones.ToListAsync();
                    var regiones = oracleRegiones.Select(r => new BasedeDatos.Models.Region
                    {
                        IdReg = Convert.ToInt32(r.Region),
                        Nombre = r.Nombre
                    }).ToList();
                    ViewBag.Data = regiones;
                    break;

                default:
                    ModelState.AddModelError(string.Empty, "Nombre de tabla no válido");
                    return View("Error");
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SearchTable(string tableName, string searchQuery)
        {
            ViewBag.TableName = tableName;
            ViewBag.ShowForm = true;

            try
            {
                switch (tableName)
                {
                    case "Provincias":
                        var oracleProvincias = await _context.Provincias
                            .FromSqlRaw("BEGIN :result := buscar_provincia(:searchQuery); END;",
                            new OracleParameter("searchQuery", searchQuery))
                            .ToListAsync();
                        ViewBag.Data = oracleProvincias.Select(p => new BasedeDatos.Models.Provincia
                        {
                            IdProv = Convert.ToInt32(p.IdProv),
                            Nombre = p.Nombre,
                            Region = Convert.ToInt32(p.Region)
                        }).ToList();
                        break;

                    case "Cantones":
                        var oracleCantones = await _context.Cantones
                            .FromSqlRaw("BEGIN :result := buscar_canton(:searchQuery); END;",
                            new OracleParameter("searchQuery", searchQuery))
                            .ToListAsync();
                        ViewBag.Data = oracleCantones.Select(c => new BasedeDatos.Models.Canton
                        {
                            IdCan = Convert.ToInt32(c.IdCan),
                            Nombre = c.Nombre,
                            IdProv = Convert.ToInt32(c.IdProv)
                        }).ToList();
                        break;

                    case "Parroquias":
                        var oracleParroquias = await _context.Parroquias
                            .FromSqlRaw("BEGIN :result := buscar_parroquia(:searchQuery); END;",
                            new OracleParameter("searchQuery", searchQuery))
                            .ToListAsync();
                        ViewBag.Data = oracleParroquias.Select(p => new BasedeDatos.Models.Parroquia
                        {
                            IdPar = Convert.ToInt32(p.IdPar),
                            Nombre = p.Nombre,
                            IdCan = Convert.ToInt32(p.IdCan),
                            IdProv = Convert.ToInt32(p.IdProv)
                        }).ToList();
                        break;

                    case "Regiones":
                        var oracleRegiones = await _context.Regiones
                            .FromSqlRaw("BEGIN :result := buscar_region(:searchQuery); END;",
                            new OracleParameter("searchQuery", searchQuery))
                            .ToListAsync();
                        ViewBag.Data = oracleRegiones.Select(r => new BasedeDatos.Models.Region
                        {
                            IdReg = Convert.ToInt32(r.Region),
                            Nombre = r.Nombre
                        }).ToList();
                        break;

                    default:
                        ModelState.AddModelError(string.Empty, "Nombre de tabla no válido");
                        return View("Error");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ocurrió un error al buscar los datos: {ex.Message}");
                return View("Error");
            }

            return View("Index");
        }



        [HttpPost]
        public async Task<IActionResult> CreateEntry(string tableName, int? IdProv, string nombre, int? Region, int? IdCan, int? IdPar)
        {
            using var connection = new OracleConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            OracleCommand command = null;

            if (tableName == "Provincias")
            {
                command = new OracleCommand("CREAR_PROVINCIA", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.Add("P_ID_PROV", OracleDbType.Int32).Value = IdProv ?? (object)DBNull.Value;
                command.Parameters.Add("P_PROV_DESCRIP", OracleDbType.Varchar2).Value = nombre ?? (object)DBNull.Value;
                command.Parameters.Add("P_ID_REG", OracleDbType.Int32).Value = Region ?? (object)DBNull.Value;
            }
            else if (tableName == "Cantones")
            {
                command = new OracleCommand("CREAR_CANTON", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.Add("P_ID_CAN", OracleDbType.Int32).Value = IdCan ?? (object)DBNull.Value;
                command.Parameters.Add("P_CAN_DESCRIP", OracleDbType.Varchar2).Value = nombre ?? (object)DBNull.Value;
                command.Parameters.Add("P_ID_PROV", OracleDbType.Int32).Value = IdProv ?? (object)DBNull.Value;
            }
            else if (tableName == "Parroquias")
            {
                command = new OracleCommand("CREAR_PARROQUIA", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.Add("P_ID_PAR", OracleDbType.Int32).Value = IdPar ?? (object)DBNull.Value;
                command.Parameters.Add("P_PAR_DESCRIP", OracleDbType.Varchar2).Value = nombre ?? (object)DBNull.Value;
                command.Parameters.Add("P_ID_CAN", OracleDbType.Int32).Value = IdCan ?? (object)DBNull.Value;
                command.Parameters.Add("P_ID_PROV", OracleDbType.Int32).Value = IdProv ?? (object)DBNull.Value;
            }
            else if (tableName == "Regiones")
            {
                command = new OracleCommand("CREAR_REGION", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.Add("P_ID_REG", OracleDbType.Int32).Value = Region ?? (object)DBNull.Value;
                command.Parameters.Add("P_REG_DESCRIP", OracleDbType.Varchar2).Value = nombre ?? (object)DBNull.Value;
            }

            if (command != null)
            {
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditProvincia(int idProv, string provDescrip, int? idReg)
        {
            using var connection = new OracleConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("ACTUALIZAR_PROVINCIA", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("P_ID_PROV", OracleDbType.Int32).Value = idProv;
            command.Parameters.Add("P_PROV_DESCRIP", OracleDbType.Varchar2).Value = provDescrip;
            command.Parameters.Add("P_ID_REG", OracleDbType.Int32).Value = idReg ?? (object)DBNull.Value;
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditCanton(int idCan, string canDescrip, int idProv)
        {
            using var connection = new OracleConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("ACTUALIZAR_CANTON", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("P_ID_CAN", OracleDbType.Int32).Value = idCan;
            command.Parameters.Add("P_CAN_DESCRIP", OracleDbType.Varchar2).Value = canDescrip;
            command.Parameters.Add("P_ID_PROV", OracleDbType.Int32).Value = idProv;
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditParroquia(int idPar, string parDescrip, int idCan, int idProv)
        {
            using var connection = new OracleConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("ACTUALIZAR_PARROQUIA", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("P_ID_PAR", OracleDbType.Int32).Value = idPar;
            command.Parameters.Add("P_PAR_DESCRIP", OracleDbType.Varchar2).Value = parDescrip;
            command.Parameters.Add("P_ID_CAN", OracleDbType.Int32).Value = idCan;
            command.Parameters.Add("P_ID_PROV", OracleDbType.Int32).Value = idProv;
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditRegione(int idReg, string regDescrip)
        {
            using var connection = new OracleConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("ACTUALIZAR_REGION", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("P_ID_REG", OracleDbType.Int32).Value = idReg;
            command.Parameters.Add("P_REG_DESCRIP", OracleDbType.Varchar2).Value = regDescrip;
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProvincia(int idProv)
        {
            using var connection = new OracleConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("ELIMINAR_PROVINCIA", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("P_ID_PROV", OracleDbType.Int32).Value = idProv;
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCanton(int idCan)
        {
            using var connection = new OracleConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("ELIMINAR_CANTON", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("P_ID_CAN", OracleDbType.Int32).Value = idCan;
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteParroquia(int idPar)
        {
            using var connection = new OracleConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("ELIMINAR_PARROQUIA", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("P_ID_PAR", OracleDbType.Int32).Value = idPar;
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRegion(int idReg)
        {
            using var connection = new OracleConnection(_context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("ELIMINAR_REGION", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add("P_ID_REG", OracleDbType.Int32).Value = idReg;
            await command.ExecuteNonQueryAsync();

            return RedirectToAction("Index");
        }
    }
}
