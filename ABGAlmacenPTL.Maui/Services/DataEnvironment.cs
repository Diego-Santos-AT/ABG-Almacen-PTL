// ***********************************************************************
// Nombre: DataEnvironment.cs
// Entorno de Datos - Conversión fiel de VB6
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     EntornoDeDatos.Dsr
// ***********************************************************************

using System.Data;
using System.Data.SqlClient;
using ABGAlmacenPTL.Maui.Models;

namespace ABGAlmacenPTL.Maui.Services
{
    /// <summary>
    /// Entorno de Datos - Equivalente a EntornoDeDatos de VB6
    /// Proporciona acceso a la base de datos mediante procedimientos almacenados
    /// </summary>
    public class DataEnvironment : IDisposable
    {
        private SqlConnection? _gestionAlmacen;
        private bool _disposed;

        public SqlConnection GestionAlmacen
        {
            get
            {
                if (_gestionAlmacen == null)
                {
                    _gestionAlmacen = new SqlConnection(AppSettings.Instance.ConexionGestionAlmacen);
                }
                return _gestionAlmacen;
            }
        }

        public ConnectionState State => GestionAlmacen.State;

        /// <summary>
        /// Abre la conexión a la base de datos
        /// Equivalente a ed.GestionAlmacen.Open de VB6
        /// </summary>
        public async Task OpenAsync()
        {
            if (GestionAlmacen.State != ConnectionState.Open)
            {
                await GestionAlmacen.OpenAsync();
            }
        }

        /// <summary>
        /// Cierra la conexión a la base de datos
        /// </summary>
        public void Close()
        {
            if (_gestionAlmacen?.State != ConnectionState.Closed)
            {
                _gestionAlmacen?.Close();
            }
        }

        // ==================== Procedimientos almacenados ====================

        /// <summary>
        /// DameDatosBACdePTL - Obtiene datos de un BAC
        /// </summary>
        public async Task<DatosBACPTL?> DameDatosBACdePTL(string bac)
        {
            using var cmd = new SqlCommand("dbo.DameDatosBACdePTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BAC", bac);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new DatosBACPTL
                {
                    Unicod = reader["unicod"]?.ToString(),
                    Uniest = reader["uniest"] != DBNull.Value ? Convert.ToInt32(reader["uniest"]) : 0,
                    Unigru = reader["unigru"] != DBNull.Value ? Convert.ToInt32(reader["unigru"]) : 0,
                    Unitab = reader["unitab"] != DBNull.Value ? Convert.ToInt32(reader["unitab"]) : 0,
                    Unipes = reader["unipes"] != DBNull.Value ? Convert.ToDouble(reader["unipes"]) : 0,
                    Unipma = reader["unipma"] != DBNull.Value ? Convert.ToDouble(reader["unipma"]) : 0,
                    Univol = reader["univol"] != DBNull.Value ? Convert.ToDouble(reader["univol"]) : 0,
                    Univma = reader["univma"] != DBNull.Value ? Convert.ToDouble(reader["univma"]) : 0,
                    Unicaj = reader["unicaj"]?.ToString(),
                    Tipdes = reader["tipdes"]?.ToString(),
                    Uninca = reader["uninca"]?.ToString(),
                    Ubicod = reader["ubicod"] != DBNull.Value ? Convert.ToInt32(reader["ubicod"]) : null,
                    Ubialm = reader["ubialm"] != DBNull.Value ? Convert.ToInt32(reader["ubialm"]) : 0,
                    Ubiblo = reader["ubiblo"] != DBNull.Value ? Convert.ToInt32(reader["ubiblo"]) : 0,
                    Ubifil = reader["ubifil"] != DBNull.Value ? Convert.ToInt32(reader["ubifil"]) : 0,
                    Ubialt = reader["ubialt"] != DBNull.Value ? Convert.ToInt32(reader["ubialt"]) : 0,
                    Uninum = reader["uninum"] != DBNull.Value ? Convert.ToInt32(reader["uninum"]) : 0
                };
            }
            return null;
        }

        /// <summary>
        /// DameDatosUbicacionPTL - Obtiene datos de una ubicación PTL
        /// </summary>
        public async Task<DatosUbicacionPTL?> DameDatosUbicacionPTL(int alf, int alm, int blo, int fil, int alt)
        {
            using var cmd = new SqlCommand("dbo.DameDatosUbicacionPTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ALF", alf);
            cmd.Parameters.AddWithValue("@ALM", alm);
            cmd.Parameters.AddWithValue("@BLO", blo);
            cmd.Parameters.AddWithValue("@FIL", fil);
            cmd.Parameters.AddWithValue("@ALT", alt);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new DatosUbicacionPTL
                {
                    Ubicod = reader["ubicod"] != DBNull.Value ? Convert.ToInt32(reader["ubicod"]) : 0,
                    Ubialm = reader["ubialm"] != DBNull.Value ? Convert.ToInt32(reader["ubialm"]) : 0,
                    Ubiblo = reader["ubiblo"] != DBNull.Value ? Convert.ToInt32(reader["ubiblo"]) : 0,
                    Ubifil = reader["ubifil"] != DBNull.Value ? Convert.ToInt32(reader["ubifil"]) : 0,
                    Ubialt = reader["ubialt"] != DBNull.Value ? Convert.ToInt32(reader["ubialt"]) : 0,
                    Unicod = reader["unicod"]?.ToString()
                };
            }
            return null;
        }

        /// <summary>
        /// DameDatosCAJAdePTL - Obtiene datos de una caja PTL
        /// </summary>
        public async Task<DatosCajaPTL?> DameDatosCAJAdePTL(string sscc)
        {
            using var cmd = new SqlCommand("dbo.DameDatosCAJAdePTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SSCC", sscc);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new DatosCajaPTL
                {
                    Ltcgru = reader["ltcgru"] != DBNull.Value ? Convert.ToInt32(reader["ltcgru"]) : 0,
                    Ltctab = reader["ltctab"] != DBNull.Value ? Convert.ToInt32(reader["ltctab"]) : 0,
                    Ltccaj = reader["ltccaj"]?.ToString(),
                    Ltctip = reader["ltctip"] != DBNull.Value ? Convert.ToInt32(reader["ltctip"]) : 0,
                    Ltcide = reader["ltcide"] != DBNull.Value ? Convert.ToInt64(reader["ltcide"]) : 0,
                    Ltcpes = reader["ltcpes"] != DBNull.Value ? Convert.ToDouble(reader["ltcpes"]) : 0,
                    Ltcssc = reader["ltcssc"]?.ToString(),
                    Ltcvol = reader["ltcvol"] != DBNull.Value ? Convert.ToDouble(reader["ltcvol"]) : 0,
                    Tipdes = reader["tipdes"]?.ToString()
                };
            }
            return null;
        }

        /// <summary>
        /// DameContenidoBacGrupo - Obtiene el contenido de un BAC en un grupo
        /// </summary>
        public async Task<List<ContenidoBACGrupo>> DameContenidoBacGrupo(int grupo, string bac)
        {
            var result = new List<ContenidoBACGrupo>();
            using var cmd = new SqlCommand("dbo.DameContenidoBacGrupo", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Grupo", grupo);
            cmd.Parameters.AddWithValue("@BAC", bac);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new ContenidoBACGrupo
                {
                    Unicod = reader["unicod"]?.ToString(),
                    Unigru = reader["unigru"] != DBNull.Value ? Convert.ToInt32(reader["unigru"]) : 0,
                    Unitab = reader["unitab"] != DBNull.Value ? Convert.ToInt32(reader["unitab"]) : 0,
                    Uniart = reader["uniart"] != DBNull.Value ? Convert.ToInt32(reader["uniart"]) : 0,
                    Unican = reader["unican"] != DBNull.Value ? Convert.ToInt32(reader["unican"]) : 0,
                    Univol = reader["univol"] != DBNull.Value ? Convert.ToDouble(reader["univol"]) : 0,
                    Uniusu = reader["uniusu"] != DBNull.Value ? Convert.ToInt32(reader["uniusu"]) : 0,
                    Unifmd = reader["unifmd"] != DBNull.Value ? Convert.ToDateTime(reader["unifmd"]) : null,
                    Unires = reader["unires"] != DBNull.Value ? Convert.ToInt32(reader["unires"]) : 0,
                    Artnom = reader["artnom"]?.ToString(),
                    Ctapue = reader["ctapue"] != DBNull.Value ? Convert.ToInt32(reader["ctapue"]) : 0
                });
            }
            return result;
        }

        /// <summary>
        /// DameContenidoCajaGrupo - Obtiene el contenido de una caja en un grupo
        /// </summary>
        public async Task<List<ContenidoCajaGrupo>> DameContenidoCajaGrupo(int grupo, int tablilla, string caja)
        {
            var result = new List<ContenidoCajaGrupo>();
            using var cmd = new SqlCommand("dbo.DameContenidoCajaGrupo", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Grupo", grupo);
            cmd.Parameters.AddWithValue("@Tablilla", tablilla);
            cmd.Parameters.AddWithValue("@CAJA", caja);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new ContenidoCajaGrupo
                {
                    Ltcgru = reader["ltcgru"] != DBNull.Value ? Convert.ToInt32(reader["ltcgru"]) : 0,
                    Ltctab = reader["ltctab"] != DBNull.Value ? Convert.ToInt32(reader["ltctab"]) : 0,
                    Ltcart = reader["ltcart"] != DBNull.Value ? Convert.ToInt32(reader["ltcart"]) : 0,
                    Ltccaj = reader["ltccaj"]?.ToString(),
                    Ltccan = reader["ltccan"] != DBNull.Value ? Convert.ToDouble(reader["ltccan"]) : 0,
                    Ltcfin = reader["ltcfin"]?.ToString(),
                    Ltcusu = reader["ltcusu"] != DBNull.Value ? Convert.ToInt32(reader["ltcusu"]) : 0,
                    Ltcfal = reader["ltcfal"] != DBNull.Value ? Convert.ToDateTime(reader["ltcfal"]) : null,
                    Ltcfmd = reader["ltcfmd"] != DBNull.Value ? Convert.ToDateTime(reader["ltcfmd"]) : null,
                    Ltcpes = reader["ltcpes"] != DBNull.Value ? Convert.ToDouble(reader["ltcpes"]) : 0,
                    Ltcvol = reader["ltcvol"] != DBNull.Value ? Convert.ToDouble(reader["ltcvol"]) : 0,
                    Ltcide = reader["ltcide"] != DBNull.Value ? Convert.ToInt64(reader["ltcide"]) : 0,
                    Artnom = reader["artnom"]?.ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// DameTiposCajasActivas - Obtiene los tipos de cajas activas
        /// </summary>
        public async Task<List<TipoCaja>> DameTiposCajasActivas()
        {
            var result = new List<TipoCaja>();
            using var cmd = new SqlCommand("dbo.DameTiposCajasActivas", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new TipoCaja
                {
                    Tipcod = reader["TIPCOD"] != DBNull.Value ? Convert.ToInt32(reader["TIPCOD"]) : 0,
                    Tipdes = reader["TIPDES"]?.ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// CambiaEstadoBACdePTL - Cambia el estado de un BAC
        /// </summary>
        public async Task<ResultadoOperacion> CambiaEstadoBACdePTL(string bac, int estado, int usuario)
        {
            using var cmd = new SqlCommand("dbo.CambiaEstadoBACdePTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BAC", bac);
            cmd.Parameters.AddWithValue("@Estado", estado);
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            
            var retornoParam = new SqlParameter("@Retorno", SqlDbType.SmallInt) { Direction = ParameterDirection.Output };
            var msgSalidaParam = new SqlParameter("@msgSalida", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(retornoParam);
            cmd.Parameters.Add(msgSalidaParam);

            await cmd.ExecuteNonQueryAsync();

            return new ResultadoOperacion
            {
                Retorno = retornoParam.Value != DBNull.Value ? Convert.ToInt32(retornoParam.Value) : -1,
                MsgSalida = msgSalidaParam.Value?.ToString() ?? string.Empty
            };
        }

        /// <summary>
        /// RetirarBACdePTL - Retira un BAC de PTL
        /// </summary>
        public async Task<ResultadoOperacion> RetirarBACdePTL(string bac, int usuario)
        {
            using var cmd = new SqlCommand("dbo.RetirarBACdePTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BAC", bac);
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            
            var retornoParam = new SqlParameter("@Retorno", SqlDbType.SmallInt) { Direction = ParameterDirection.Output };
            var msgSalidaParam = new SqlParameter("@msgSalida", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(retornoParam);
            cmd.Parameters.Add(msgSalidaParam);

            await cmd.ExecuteNonQueryAsync();

            return new ResultadoOperacion
            {
                Retorno = retornoParam.Value != DBNull.Value ? Convert.ToInt32(retornoParam.Value) : -1,
                MsgSalida = msgSalidaParam.Value?.ToString() ?? string.Empty
            };
        }

        /// <summary>
        /// UbicarBACenPTL - Ubica un BAC en PTL
        /// </summary>
        public async Task<ResultadoOperacion> UbicarBACenPTL(string bac, int ubicacion, int usuario)
        {
            using var cmd = new SqlCommand("dbo.UbicarBACenPTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BAC", bac);
            cmd.Parameters.AddWithValue("@Ubicacion", ubicacion);
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            
            var retornoParam = new SqlParameter("@Retorno", SqlDbType.SmallInt) { Direction = ParameterDirection.Output };
            var msgSalidaParam = new SqlParameter("@msgSalida", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(retornoParam);
            cmd.Parameters.Add(msgSalidaParam);

            await cmd.ExecuteNonQueryAsync();

            return new ResultadoOperacion
            {
                Retorno = retornoParam.Value != DBNull.Value ? Convert.ToInt32(retornoParam.Value) : -1,
                MsgSalida = msgSalidaParam.Value?.ToString() ?? string.Empty
            };
        }

        /// <summary>
        /// TraspasaBACaCAJAdePTL - Traspasa BAC a CAJA
        /// </summary>
        public async Task<(ResultadoOperacion resultado, string sscc)> TraspasaBACaCAJAdePTL(string bac, int usuario, string ssccBase)
        {
            using var cmd = new SqlCommand("dbo.TraspasaBACaCAJAdePTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BAC", bac);
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            
            var ssccParam = new SqlParameter("@SSCC", SqlDbType.VarChar, 50) 
            { 
                Direction = ParameterDirection.InputOutput,
                Value = ssccBase
            };
            var retornoParam = new SqlParameter("@Retorno", SqlDbType.SmallInt) { Direction = ParameterDirection.Output };
            var msgSalidaParam = new SqlParameter("@msgSalida", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(ssccParam);
            cmd.Parameters.Add(retornoParam);
            cmd.Parameters.Add(msgSalidaParam);

            await cmd.ExecuteNonQueryAsync();

            return (new ResultadoOperacion
            {
                Retorno = retornoParam.Value != DBNull.Value ? Convert.ToInt32(retornoParam.Value) : -1,
                MsgSalida = msgSalidaParam.Value?.ToString() ?? string.Empty
            }, ssccParam.Value?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// DameArticuloConsulta - Obtiene datos de un artículo
        /// </summary>
        public async Task<Articulo?> DameArticuloConsulta(int articulo)
        {
            using var cmd = new SqlCommand("dbo.DameArticuloConsulta", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Articulo", articulo);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Articulo
                {
                    Artcod = reader["artcod"] != DBNull.Value ? Convert.ToInt32(reader["artcod"]) : 0,
                    Artnom = reader["artnom"]?.ToString(),
                    Artref = reader["artref"]?.ToString(),
                    Artean = reader["artean"]?.ToString(),
                    Artcj1 = reader["artcj1"] != DBNull.Value ? Convert.ToInt32(reader["artcj1"]) : 0,
                    Artcj2 = reader["artcj2"] != DBNull.Value ? Convert.ToInt32(reader["artcj2"]) : 0,
                    Artcj3 = reader["artcj3"] != DBNull.Value ? Convert.ToInt32(reader["artcj3"]) : 0,
                    Artpea = reader["artpea"] != DBNull.Value ? Convert.ToDouble(reader["artpea"]) : 0,
                    Artcua = reader["artcua"] != DBNull.Value ? Convert.ToDouble(reader["artcua"]) : 0
                };
            }
            return null;
        }

        /// <summary>
        /// DameArticuloEAN13 - Obtiene datos de un artículo por EAN13
        /// </summary>
        public async Task<Articulo?> DameArticuloEAN13(string ean13)
        {
            using var cmd = new SqlCommand("dbo.DameArticuloEAN13", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EAN13", ean13);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Articulo
                {
                    Artcod = reader["artcod"] != DBNull.Value ? Convert.ToInt32(reader["artcod"]) : 0,
                    Artnom = reader["artnom"]?.ToString(),
                    Artref = reader["artref"]?.ToString(),
                    Artean = reader["artean"]?.ToString(),
                    Artcj1 = reader["artcj1"] != DBNull.Value ? Convert.ToInt32(reader["artcj1"]) : 0,
                    Artcj2 = reader["artcj2"] != DBNull.Value ? Convert.ToInt32(reader["artcj2"]) : 0,
                    Artcj3 = reader["artcj3"] != DBNull.Value ? Convert.ToInt32(reader["artcj3"]) : 0,
                    Artpea = reader["artpea"] != DBNull.Value ? Convert.ToDouble(reader["artpea"]) : 0,
                    Artcua = reader["artcua"] != DBNull.Value ? Convert.ToDouble(reader["artcua"]) : 0
                };
            }
            return null;
        }

        /// <summary>
        /// DamePuestosTrabajoPTL - Obtiene puestos de trabajo PTL
        /// </summary>
        public async Task<List<PuestoTrabajoPTL>> DamePuestosTrabajoPTL()
        {
            var result = new List<PuestoTrabajoPTL>();
            using var cmd = new SqlCommand("dbo.DamePuestosTrabajoPTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new PuestoTrabajoPTL
                {
                    Puecod = reader["puecod"] != DBNull.Value ? Convert.ToInt32(reader["puecod"]) : 0,
                    Puedes = reader["puedes"]?.ToString(),
                    Puecor = reader["puecor"]?.ToString(),
                    Puetip = reader["puetip"] != DBNull.Value ? Convert.ToInt32(reader["puetip"]) : 0,
                    Pueusu = reader["pueusu"] != DBNull.Value ? Convert.ToInt32(reader["pueusu"]) : 0,
                    Pueimp = reader["pueimp"] != DBNull.Value ? Convert.ToInt32(reader["pueimp"]) : 0,
                    Puecol = reader["puecol"] != DBNull.Value ? Convert.ToInt32(reader["puecol"]) : 0,
                    Puegru = reader["puegru"] != DBNull.Value ? Convert.ToInt32(reader["puegru"]) : 0,
                    Puemac = reader["puemac"]?.ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// ReservaBACdePTL - Reserva un BAC para un artículo
        /// </summary>
        public async Task<ResultadoOperacion> ReservaBACdePTL(int articulo, int usuario)
        {
            using var cmd = new SqlCommand("dbo.ReservaBACdePTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Articulo", articulo);
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            
            var retornoParam = new SqlParameter("@Retorno", SqlDbType.SmallInt) { Direction = ParameterDirection.Output };
            var msgSalidaParam = new SqlParameter("@msgSalida", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(retornoParam);
            cmd.Parameters.Add(msgSalidaParam);

            await cmd.ExecuteNonQueryAsync();

            return new ResultadoOperacion
            {
                Retorno = retornoParam.Value != DBNull.Value ? Convert.ToInt32(retornoParam.Value) : -1,
                MsgSalida = msgSalidaParam.Value?.ToString() ?? string.Empty
            };
        }

        /// <summary>
        /// DameCajasGrupoTablillaPTL - Obtiene cajas de un grupo y tablilla
        /// </summary>
        public async Task<List<CajaGrupoTablillaPTL>> DameCajasGrupoTablillaPTL(int grupo, int tablilla)
        {
            var result = new List<CajaGrupoTablillaPTL>();
            using var cmd = new SqlCommand("dbo.DameCajasGrupoTablillaPTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Grupo", grupo);
            cmd.Parameters.AddWithValue("@Tablilla", tablilla);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new CajaGrupoTablillaPTL
                {
                    Ltcgru = reader["ltcgru"] != DBNull.Value ? Convert.ToInt32(reader["ltcgru"]) : 0,
                    Ltctab = reader["ltctab"] != DBNull.Value ? Convert.ToInt32(reader["ltctab"]) : 0,
                    Ltccaj = reader["ltccaj"]?.ToString(),
                    Ltctip = reader["ltctip"] != DBNull.Value ? Convert.ToInt32(reader["ltctip"]) : 0,
                    Ltcide = reader["ltcide"] != DBNull.Value ? Convert.ToInt64(reader["ltcide"]) : 0,
                    Ltcpes = reader["ltcpes"] != DBNull.Value ? Convert.ToDouble(reader["ltcpes"]) : 0,
                    Ltcssc = reader["ltcssc"]?.ToString(),
                    Ltcvol = reader["ltcvol"] != DBNull.Value ? Convert.ToDouble(reader["ltcvol"]) : 0,
                    Tipdes = reader["tipdes"]?.ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// CombinarCajasPTL - Combina dos cajas
        /// </summary>
        public async Task<ResultadoOperacion> CombinarCajasPTL(string sscc1, string sscc2, int usuario)
        {
            using var cmd = new SqlCommand("dbo.CombinarCajasPTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SSCC1", sscc1);
            cmd.Parameters.AddWithValue("@SSCC2", sscc2);
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            
            var retornoParam = new SqlParameter("@Retorno", SqlDbType.SmallInt) { Direction = ParameterDirection.Output };
            var msgSalidaParam = new SqlParameter("@msgSalida", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(retornoParam);
            cmd.Parameters.Add(msgSalidaParam);

            await cmd.ExecuteNonQueryAsync();

            return new ResultadoOperacion
            {
                Retorno = retornoParam.Value != DBNull.Value ? Convert.ToInt32(retornoParam.Value) : -1,
                MsgSalida = msgSalidaParam.Value?.ToString() ?? string.Empty
            };
        }

        /// <summary>
        /// CambiaTipoCajaPTL - Cambia el tipo de caja
        /// </summary>
        public async Task CambiaTipoCajaPTL(int tipoCaja, string bac, string sscc, int usuario)
        {
            using var cmd = new SqlCommand("dbo.CambiaTipoCajaPTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TipoCaja", tipoCaja);
            cmd.Parameters.AddWithValue("@BAC", bac);
            cmd.Parameters.AddWithValue("@SSCC", sscc);
            cmd.Parameters.AddWithValue("@Usuario", usuario);

            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// CambiaUnidadesArtCajaPTL - Cambia las unidades de un artículo en caja
        /// </summary>
        public async Task<ResultadoOperacion> CambiaUnidadesArtCajaPTL(string sscc, int articulo, int cantidad, int usuario)
        {
            using var cmd = new SqlCommand("dbo.CambiaUnidadesArtCajaPTL", GestionAlmacen);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SSCC", sscc);
            cmd.Parameters.AddWithValue("@Articulo", articulo);
            cmd.Parameters.AddWithValue("@Cantidad", cantidad);
            cmd.Parameters.AddWithValue("@Usuario", usuario);
            
            var retornoParam = new SqlParameter("@Retorno", SqlDbType.SmallInt) { Direction = ParameterDirection.Output };
            var msgSalidaParam = new SqlParameter("@msgSalida", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(retornoParam);
            cmd.Parameters.Add(msgSalidaParam);

            await cmd.ExecuteNonQueryAsync();

            return new ResultadoOperacion
            {
                Retorno = retornoParam.Value != DBNull.Value ? Convert.ToInt32(retornoParam.Value) : -1,
                MsgSalida = msgSalidaParam.Value?.ToString() ?? string.Empty
            };
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _gestionAlmacen?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
