using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ABGAlmacenPTL.Data.Repositories;
using ABGAlmacenPTL.Models;
using Microsoft.EntityFrameworkCore;

namespace ABGAlmacenPTL.Services
{
    /// <summary>
    /// Enhanced PTL Service that combines EF Core with dynamic stored procedures
    /// This shows how to gradually migrate to using stored procedures where appropriate
    /// while keeping EF Core for simple CRUD operations
    /// </summary>
    public class PTLServiceEnhanced
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PTLStoredProcedureService _ptlSPService;
        private readonly IDynamicDatabaseService _dbService;

        public PTLServiceEnhanced(
            IUnitOfWork unitOfWork,
            PTLStoredProcedureService ptlSPService,
            IDynamicDatabaseService dbService)
        {
            _unitOfWork = unitOfWork;
            _ptlSPService = ptlSPService;
            _dbService = dbService;
        }

        #region Article Operations

        /// <summary>
        /// Get article by code - Using EF Core for simple query
        /// </summary>
        public async Task<Articulo?> GetArticuloByCodigoAsync(string codigo)
        {
            return await _unitOfWork.Articulos.FindFirstAsync(a => a.CodigoArticulo == codigo);
        }

        /// <summary>
        /// Get article by EAN13 - Using stored procedure for VB6 compatibility
        /// </summary>
        public async Task<Articulo?> GetArticuloByEAN13Async(string ean13)
        {
            try
            {
                // Call stored procedure from SELENE
                var result = await _ptlSPService.DameArticuloEAN13Async(ean13);
                
                if (result.Rows.Count == 0)
                    return null;

                var row = result.Rows[0];
                
                // Map DataRow to Articulo model
                return new Articulo
                {
                    CodigoArticulo = row["CodigoArticulo"]?.ToString() ?? "",
                    Nombre = row["Descripcion"]?.ToString() ?? row["Nombre"]?.ToString() ?? "",
                    EAN13 = row["EAN13"]?.ToString(),
                    CodigoSTD = row["CodigoSTD"]?.ToString(),
                    Peso = row.Table.Columns.Contains("Peso") ? Convert.ToDecimal(row["Peso"] ?? 0) : null,
                    Volumen = row.Table.Columns.Contains("Volumen") ? Convert.ToDecimal(row["Volumen"] ?? 0) : null,
                    Activo = row.Table.Columns.Contains("Activo") && Convert.ToBoolean(row["Activo"] ?? true),
                    FechaCreacion = row.Table.Columns.Contains("FechaCreacion") ? Convert.ToDateTime(row["FechaCreacion"] ?? DateTime.Now) : DateTime.Now,
                    FechaModificacion = row.Table.Columns.Contains("FechaModificacion") ? (DateTime?)Convert.ToDateTime(row["FechaModificacion"] ?? DateTime.Now) : null
                };
            }
            catch
            {
                // Fallback to EF Core if stored procedure fails
                return await _unitOfWork.Articulos.FindFirstAsync(a => a.EAN13 == ean13);
            }
        }

        #endregion

        #region BAC Operations

        /// <summary>
        /// Get BAC by code - Using EF Core for simple query
        /// </summary>
        public async Task<BAC?> GetBACByCodigoAsync(string codigoBAC)
        {
            return await _unitOfWork.BACs.FindFirstAsync(b => b.CodigoBAC == codigoBAC);
        }

        /// <summary>
        /// Get BAC data - Using stored procedure for complete data
        /// </summary>
        public async Task<DataTable> GetBACDataAsync(string codigoBAC)
        {
            return await _ptlSPService.DameDatosBACdePTLAsync(codigoBAC);
        }

        /// <summary>
        /// Assign BAC to location - Using stored procedure (VB6-style)
        /// </summary>
        public async Task<bool> AsignarBACaUbicacionAsync(string codigoBAC, string codigoUbicacion, int idPuesto)
        {
            try
            {
                // Use stored procedure for business logic
                var rowsAffected = await _ptlSPService.UbicarBACenPTLAsync(codigoBAC, codigoUbicacion, idPuesto);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al ubicar BAC: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Extract BAC from location - Using stored procedure (VB6-style)
        /// </summary>
        public async Task<bool> ExtraerBACAsync(string codigoBAC, int idPuesto)
        {
            try
            {
                // Use stored procedure for business logic
                var rowsAffected = await _ptlSPService.ExtraerBACdePTLAsync(codigoBAC, idPuesto);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al extraer BAC: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get all located BACs - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetBACsUbicadosAsync()
        {
            return await _ptlSPService.DameBACsUbicadosPTLAsync();
        }

        /// <summary>
        /// Get BACs for a group - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetBACsGrupoAsync(int idGrupo)
        {
            return await _ptlSPService.DameBACsGrupoPTLAsync(idGrupo);
        }

        /// <summary>
        /// Change BAC state - Using stored procedure
        /// </summary>
        public async Task<bool> CambiarEstadoBACAsync(string codigoBAC, string nuevoEstado)
        {
            try
            {
                var rowsAffected = await _ptlSPService.CambiaEstadoBACdePTLAsync(codigoBAC, nuevoEstado);
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Box Operations

        /// <summary>
        /// Transfer BAC to box - Using stored procedure
        /// </summary>
        public async Task<bool> TraspasarBACaCajaAsync(string codigoBAC, string codigoSSCC)
        {
            try
            {
                var rowsAffected = await _ptlSPService.TraspasaBACaCAJAdePTLAsync(codigoBAC, codigoSSCC);
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get box data - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetCajaDataAsync(string codigoSSCC)
        {
            return await _ptlSPService.DameDatosCAJAdePTLAsync(codigoSSCC);
        }

        /// <summary>
        /// Create box for group/board - Using stored procedure
        /// </summary>
        public async Task<DataTable> CrearCajaGrupoTablillaAsync(int idGrupo, int idTablilla)
        {
            return await _ptlSPService.CrearCajaGrupoTablillaPTLAsync(idGrupo, idTablilla);
        }

        /// <summary>
        /// Get boxes for group/board - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetCajasGrupoTablillaAsync(int idGrupo, int idTablilla)
        {
            return await _ptlSPService.DameCajasGrupoTablillaPTLAsync(idGrupo, idTablilla);
        }

        /// <summary>
        /// Change box type - Using stored procedure
        /// </summary>
        public async Task<bool> CambiarTipoCajaAsync(string codigoSSCC, int nuevoTipoCaja)
        {
            try
            {
                var rowsAffected = await _ptlSPService.CambiaTipoCajaPTLAsync(codigoSSCC, nuevoTipoCaja);
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Combine boxes - Using stored procedure
        /// </summary>
        public async Task<bool> CombinarCajasAsync(string codigoSSCCOrigen, string codigoSSCCDestino)
        {
            try
            {
                var rowsAffected = await _ptlSPService.CombinarCajasPTLAsync(codigoSSCCOrigen, codigoSSCCDestino);
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Group Operations

        /// <summary>
        /// Get all groups - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetGruposAsync()
        {
            return await _ptlSPService.DameGruposAsync();
        }

        /// <summary>
        /// Get filtered PTL groups - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetGruposFiltroPTLAsync()
        {
            return await _ptlSPService.DameGruposFiltroPTLAsync();
        }

        /// <summary>
        /// Initialize PTL group - Using stored procedure
        /// </summary>
        public async Task<bool> InicializarGrupoPTLAsync(int idGrupo)
        {
            try
            {
                var rowsAffected = await _ptlSPService.InicializaGrupoPTLAsync(idGrupo);
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Workstation Operations

        /// <summary>
        /// Get all PTL workstations - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetPuestosTrabajoPTLAsync()
        {
            return await _ptlSPService.DamePuestosTrabajoPTLAsync();
        }

        /// <summary>
        /// Get workstations for group - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetPuestosTrabajoGrupoAsync(int idGrupo)
        {
            return await _ptlSPService.DamePuestosTrabajoGrupoPTLAsync(idGrupo);
        }

        /// <summary>
        /// Get workstation by code - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetPuestoTrabajoAsync(string codigoPuesto)
        {
            return await _ptlSPService.DamePuestoTrabajoCodigoAsync(codigoPuesto);
        }

        #endregion

        #region Location Operations

        /// <summary>
        /// Get location data - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetUbicacionDataAsync(string codigoUbicacion)
        {
            return await _ptlSPService.DameDatosUbicacionPTLAsync(codigoUbicacion);
        }

        /// <summary>
        /// Get location by code - Using EF Core for simple query
        /// </summary>
        public async Task<Ubicacion?> GetUbicacionByCodigoAsync(string codigoUbicacion)
        {
            return await _unitOfWork.Ubicaciones.FindFirstAsync(u => u.CodigoUbicacion == codigoUbicacion);
        }

        #endregion

        #region Statistics Operations

        /// <summary>
        /// Get PTL statistics - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetEstadisticaPTLAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _ptlSPService.DameEstadisticaPTLAsync(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Get user distribution statistics - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetEstadisticaUsuarioRepartoAsync(int idUsuario, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _ptlSPService.DameEstadisticaUsuarioRepPTLAsync(idUsuario, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Get user packing statistics - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetEstadisticaUsuarioEmpaqueAsync(int idUsuario, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _ptlSPService.DameEstadisticaUsuarioEmpPTLAsync(idUsuario, fechaInicio, fechaFin);
        }

        #endregion

        #region Log Operations

        /// <summary>
        /// Get distribution log - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetLogRepartoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _ptlSPService.DameLogRepartoPTLAsync(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Get packing log - Using stored procedure
        /// </summary>
        public async Task<DataTable> GetLogEmpaquetadoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _ptlSPService.DameLogEmpaquetadoPTLAsync(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Insert packing log - Using stored procedure
        /// </summary>
        public async Task<bool> InsertarLogEmpaquetadoAsync(Dictionary<string, object> campos)
        {
            try
            {
                var rowsAffected = await _ptlSPService.InsertaLogEmpaquetadoAsync(campos);
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Liberation Operations

        /// <summary>
        /// Free erroneous BACs - Using stored procedure
        /// </summary>
        public async Task<bool> LiberarBacsErroneosAsync()
        {
            try
            {
                var rowsAffected = await _ptlSPService.LiberarBacsErroneosAsync();
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Free BACs for a group - Using stored procedure
        /// </summary>
        public async Task<bool> LiberarBacsGrupoAsync(int idGrupo)
        {
            try
            {
                var rowsAffected = await _ptlSPService.LiberarBacsGrupoAsync(idGrupo);
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Custom Dynamic Queries

        /// <summary>
        /// Execute any stored procedure dynamically
        /// Use this for stored procedures not yet wrapped in PTLStoredProcedureService
        /// </summary>
        public async Task<DataTable> ExecuteStoredProcedureAsync(
            string procedureName,
            Dictionary<string, object>? parameters = null,
            string database = "GestionAlmacen")
        {
            return await _dbService.ExecuteStoredProcedureAsync(procedureName, parameters, database);
        }

        /// <summary>
        /// Execute any SQL query dynamically
        /// Use with caution - prefer stored procedures for better performance and security
        /// </summary>
        public async Task<DataTable> ExecuteQueryAsync(
            string query,
            Dictionary<string, object>? parameters = null,
            string database = "GestionAlmacen")
        {
            return await _dbService.ExecuteQueryAsync(query, parameters, database);
        }

        #endregion
    }
}
