using System.Data;

namespace ABGAlmacenPTL.Services
{
    /// <summary>
    /// Servicio que encapsula los stored procedures más comunes del sistema PTL.
    /// Basado en los procedimientos encontrados en SELENE.txt para las bases de datos
    /// GAATFRA, GAKIOKIDS, etc.
    /// </summary>
    public class PTLStoredProcedureService
    {
        private readonly IDynamicDatabaseService _dbService;

        public PTLStoredProcedureService(IDynamicDatabaseService dbService)
        {
            _dbService = dbService;
        }

        #region Procedimientos de BAC

        /// <summary>
        /// Ubica un BAC en una ubicación del PTL
        /// </summary>
        public async Task<int> UbicarBACenPTLAsync(string codigoBAC, string codigoUbicacion, int idPuesto)
        {
            if (string.IsNullOrWhiteSpace(codigoBAC))
                throw new ArgumentException("El código BAC no puede estar vacío", nameof(codigoBAC));
            
            if (string.IsNullOrWhiteSpace(codigoUbicacion))
                throw new ArgumentException("El código de ubicación no puede estar vacío", nameof(codigoUbicacion));
            
            var parameters = new Dictionary<string, object>
            {
                { "CodigoBAC", codigoBAC },
                { "CodigoUbicacion", codigoUbicacion },
                { "IdPuesto", idPuesto }
            };

            return await _dbService.ExecuteNonQueryAsync("UbicarBACenPTL", parameters);
        }

        /// <summary>
        /// Extrae un BAC del PTL
        /// </summary>
        public async Task<int> ExtraerBACdePTLAsync(string codigoBAC, int idPuesto)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoBAC", codigoBAC },
                { "IdPuesto", idPuesto }
            };

            return await _dbService.ExecuteNonQueryAsync("ExtraerBACdePTL", parameters);
        }

        /// <summary>
        /// Retira un BAC del PTL
        /// </summary>
        public async Task<int> RetirarBACdePTLAsync(string codigoBAC, int idPuesto)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoBAC", codigoBAC },
                { "IdPuesto", idPuesto }
            };

            return await _dbService.ExecuteNonQueryAsync("RetirarBACdePTL", parameters);
        }

        /// <summary>
        /// Vacía un BAC del PTL
        /// </summary>
        public async Task<int> VaciarBACdePTLAsync(string codigoBAC, int idPuesto)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoBAC", codigoBAC },
                { "IdPuesto", idPuesto }
            };

            return await _dbService.ExecuteNonQueryAsync("VaciarBACdePTL", parameters);
        }

        /// <summary>
        /// Consulta un BAC del PTL
        /// </summary>
        public async Task<DataTable> ConsultaBACdePTLAsync(string codigoBAC)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoBAC", codigoBAC }
            };

            return await _dbService.ExecuteStoredProcedureAsync("ConsultaBACdePTL", parameters);
        }

        /// <summary>
        /// Obtiene los datos de un BAC del PTL
        /// </summary>
        public async Task<DataTable> DameDatosBACdePTLAsync(string codigoBAC)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoBAC", codigoBAC }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameDatosBACdePTL", parameters);
        }

        /// <summary>
        /// Obtiene la ubicación de un BAC en el PTL
        /// </summary>
        public async Task<DataTable> DameBACUbicacionPTLAsync(string codigoBAC)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoBAC", codigoBAC }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameBACUbicacionPTL", parameters);
        }

        /// <summary>
        /// Obtiene los BACs ubicados en el PTL
        /// </summary>
        public async Task<DataTable> DameBACsUbicadosPTLAsync()
        {
            return await _dbService.ExecuteStoredProcedureAsync("DameBACsUbicadosPTL");
        }

        /// <summary>
        /// Obtiene los BACs de un grupo PTL
        /// </summary>
        public async Task<DataTable> DameBACsGrupoPTLAsync(int idGrupo)
        {
            var parameters = new Dictionary<string, object>
            {
                { "IdGrupo", idGrupo }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameBACsGrupoPTL", parameters);
        }

        /// <summary>
        /// Cambia el estado de un BAC en el PTL
        /// </summary>
        public async Task<int> CambiaEstadoBACdePTLAsync(string codigoBAC, string nuevoEstado)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoBAC", codigoBAC },
                { "NuevoEstado", nuevoEstado }
            };

            return await _dbService.ExecuteNonQueryAsync("CambiaEstadoBACdePTL", parameters);
        }

        /// <summary>
        /// Actualiza el estado de un BAC en el PTL
        /// </summary>
        public async Task<int> ActualizaEstadoBACPTLAsync(string codigoBAC, string estado)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoBAC", codigoBAC },
                { "Estado", estado }
            };

            return await _dbService.ExecuteNonQueryAsync("ActualizaEstadoBACPTL", parameters);
        }

        #endregion

        #region Procedimientos de Cajas

        /// <summary>
        /// Traspasa un BAC a caja del PTL
        /// </summary>
        public async Task<int> TraspasaBACaCAJAdePTLAsync(string codigoBAC, string codigoSSCC)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoBAC", codigoBAC },
                { "CodigoSSCC", codigoSSCC }
            };

            return await _dbService.ExecuteNonQueryAsync("TraspasaBACaCAJAdePTL", parameters);
        }

        /// <summary>
        /// Obtiene los datos de una caja del PTL
        /// </summary>
        public async Task<DataTable> DameDatosCAJAdePTLAsync(string codigoSSCC)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoSSCC", codigoSSCC }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameDatosCAJAdePTL", parameters);
        }

        /// <summary>
        /// Actualiza una caja BAC del PTL
        /// </summary>
        public async Task<int> ActualizaCajaBACPTLAsync(string codigoSSCC, Dictionary<string, object> campos)
        {
            var parameters = new Dictionary<string, object> { { "CodigoSSCC", codigoSSCC } };
            foreach (var campo in campos)
            {
                parameters[campo.Key] = campo.Value;
            }

            return await _dbService.ExecuteNonQueryAsync("ActualizaCajaBACPTL", parameters);
        }

        /// <summary>
        /// Cambia el tipo de caja en el PTL
        /// </summary>
        public async Task<int> CambiaTipoCajaPTLAsync(string codigoSSCC, int nuevoTipoCaja)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoSSCC", codigoSSCC },
                { "NuevoTipoCaja", nuevoTipoCaja }
            };

            return await _dbService.ExecuteNonQueryAsync("CambiaTipoCajaPTL", parameters);
        }

        /// <summary>
        /// Crea una caja de grupo tablilla PTL
        /// </summary>
        public async Task<DataTable> CrearCajaGrupoTablillaPTLAsync(int idGrupo, int idTablilla)
        {
            var parameters = new Dictionary<string, object>
            {
                { "IdGrupo", idGrupo },
                { "IdTablilla", idTablilla }
            };

            return await _dbService.ExecuteStoredProcedureAsync("CrearCajaGrupoTablillaPTL", parameters);
        }

        /// <summary>
        /// Obtiene las cajas de un grupo tablilla PTL
        /// </summary>
        public async Task<DataTable> DameCajasGrupoTablillaPTLAsync(int idGrupo, int idTablilla)
        {
            var parameters = new Dictionary<string, object>
            {
                { "IdGrupo", idGrupo },
                { "IdTablilla", idTablilla }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameCajasGrupoTablillaPTL", parameters);
        }

        /// <summary>
        /// Obtiene una caja de grupo tablilla PTL
        /// </summary>
        public async Task<DataTable> DameCajaGrupoTablillaPTLAsync(int idGrupo, int idTablilla, string codigoSSCC)
        {
            var parameters = new Dictionary<string, object>
            {
                { "IdGrupo", idGrupo },
                { "IdTablilla", idTablilla },
                { "CodigoSSCC", codigoSSCC }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameCajaGrupoTablillaPTL", parameters);
        }

        #endregion

        #region Procedimientos de Grupos y Puestos

        /// <summary>
        /// Obtiene los grupos
        /// </summary>
        public async Task<DataTable> DameGruposAsync()
        {
            return await _dbService.ExecuteStoredProcedureAsync("DameGrupos");
        }

        /// <summary>
        /// Obtiene los grupos filtro PTL
        /// </summary>
        public async Task<DataTable> DameGruposFiltroPTLAsync()
        {
            return await _dbService.ExecuteStoredProcedureAsync("DameGruposFiltroPTL");
        }

        /// <summary>
        /// Inicializa un grupo PTL
        /// </summary>
        public async Task<int> InicializaGrupoPTLAsync(int idGrupo)
        {
            var parameters = new Dictionary<string, object>
            {
                { "IdGrupo", idGrupo }
            };

            return await _dbService.ExecuteNonQueryAsync("InicializaGrupoPTL", parameters);
        }

        /// <summary>
        /// Actualiza un grupo de puestos PTL
        /// </summary>
        public async Task<int> ActualizaGrupoPuestosPTLAsync(int idGrupo, string puestos)
        {
            var parameters = new Dictionary<string, object>
            {
                { "IdGrupo", idGrupo },
                { "Puestos", puestos }
            };

            return await _dbService.ExecuteNonQueryAsync("ActualizaGrupoPuestosPTL", parameters);
        }

        /// <summary>
        /// Actualiza un grupo de puesto PTL
        /// </summary>
        public async Task<int> ActualizaGrupoPuestoPTLAsync(int idGrupo, int idPuesto, Dictionary<string, object> campos)
        {
            var parameters = new Dictionary<string, object>
            {
                { "IdGrupo", idGrupo },
                { "IdPuesto", idPuesto }
            };
            foreach (var campo in campos)
            {
                parameters[campo.Key] = campo.Value;
            }

            return await _dbService.ExecuteNonQueryAsync("ActualizaGrupoPuestoPTL", parameters);
        }

        /// <summary>
        /// Obtiene los puestos de trabajo PTL
        /// </summary>
        public async Task<DataTable> DamePuestosTrabajoPTLAsync()
        {
            return await _dbService.ExecuteStoredProcedureAsync("DamePuestosTrabajoPTL");
        }

        /// <summary>
        /// Obtiene los puestos de trabajo de un grupo PTL
        /// </summary>
        public async Task<DataTable> DamePuestosTrabajoGrupoPTLAsync(int idGrupo)
        {
            var parameters = new Dictionary<string, object>
            {
                { "IdGrupo", idGrupo }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DamePuestosTrabajoGrupoPTL", parameters);
        }

        /// <summary>
        /// Obtiene un puesto de trabajo por código
        /// </summary>
        public async Task<DataTable> DamePuestoTrabajoCodigoAsync(string codigoPuesto)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoPuesto", codigoPuesto }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DamePuestoTrabajoCodigo", parameters);
        }

        #endregion

        #region Procedimientos de Artículos

        /// <summary>
        /// Obtiene un artículo por código EAN13
        /// </summary>
        public async Task<DataTable> DameArticuloEAN13Async(string ean13)
        {
            var parameters = new Dictionary<string, object>
            {
                { "EAN13", ean13 }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameArticuloEAN13", parameters);
        }

        /// <summary>
        /// Cambia unidades de artículo en caja PTL
        /// </summary>
        public async Task<int> CambiaUnidadesArtCajaPTLAsync(string codigoSSCC, string codigoArticulo, int nuevasUnidades)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoSSCC", codigoSSCC },
                { "CodigoArticulo", codigoArticulo },
                { "NuevasUnidades", nuevasUnidades }
            };

            return await _dbService.ExecuteNonQueryAsync("CambiaUnidadesArtCajaPTL", parameters);
        }

        #endregion

        #region Procedimientos de Ubicaciones

        /// <summary>
        /// Obtiene los datos de una ubicación PTL
        /// </summary>
        public async Task<DataTable> DameDatosUbicacionPTLAsync(string codigoUbicacion)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoUbicacion", codigoUbicacion }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameDatosUbicacionPTL", parameters);
        }

        #endregion

        #region Procedimientos de Empaquetado

        /// <summary>
        /// Combina cajas en el PTL
        /// </summary>
        public async Task<int> CombinarCajasPTLAsync(string codigoSSCCOrigen, string codigoSSCCDestino)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CodigoSSCCOrigen", codigoSSCCOrigen },
                { "CodigoSSCCDestino", codigoSSCCDestino }
            };

            return await _dbService.ExecuteNonQueryAsync("CombinarCajasPTL", parameters);
        }

        /// <summary>
        /// Inserta log de empaquetado
        /// </summary>
        public async Task<int> InsertaLogEmpaquetadoAsync(Dictionary<string, object> campos)
        {
            return await _dbService.ExecuteNonQueryAsync("InsertaLogEmpaquetado", campos);
        }

        /// <summary>
        /// Obtiene el log de empaquetado PTL
        /// </summary>
        public async Task<DataTable> DameLogEmpaquetadoPTLAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var parameters = new Dictionary<string, object>
            {
                { "FechaInicio", fechaInicio },
                { "FechaFin", fechaFin }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameLogEmpaquetadoPTL", parameters);
        }

        #endregion

        #region Procedimientos de Reparto

        /// <summary>
        /// Obtiene el log de reparto PTL
        /// </summary>
        public async Task<DataTable> DameLogRepartoPTLAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var parameters = new Dictionary<string, object>
            {
                { "FechaInicio", fechaInicio },
                { "FechaFin", fechaFin }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameLogRepartoPTL", parameters);
        }

        #endregion

        #region Procedimientos de Estadísticas

        /// <summary>
        /// Obtiene estadística PTL
        /// </summary>
        public async Task<DataTable> DameEstadisticaPTLAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var parameters = new Dictionary<string, object>
            {
                { "FechaInicio", fechaInicio },
                { "FechaFin", fechaFin }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameEstadisticaPTL", parameters);
        }

        /// <summary>
        /// Obtiene estadística de usuario de reparto PTL
        /// </summary>
        public async Task<DataTable> DameEstadisticaUsuarioRepPTLAsync(int idUsuario, DateTime fechaInicio, DateTime fechaFin)
        {
            var parameters = new Dictionary<string, object>
            {
                { "IdUsuario", idUsuario },
                { "FechaInicio", fechaInicio },
                { "FechaFin", fechaFin }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameEstadisticaUsuarioRepPTL", parameters);
        }

        /// <summary>
        /// Obtiene estadística de usuario de empaquetado PTL
        /// </summary>
        public async Task<DataTable> DameEstadisticaUsuarioEmpPTLAsync(int idUsuario, DateTime fechaInicio, DateTime fechaFin)
        {
            var parameters = new Dictionary<string, object>
            {
                { "IdUsuario", idUsuario },
                { "FechaInicio", fechaInicio },
                { "FechaFin", fechaFin }
            };

            return await _dbService.ExecuteStoredProcedureAsync("DameEstadisticaUsuarioEmpPTL", parameters);
        }

        #endregion

        #region Procedimientos de Liberación

        /// <summary>
        /// Libera BACs erróneos
        /// </summary>
        public async Task<int> LiberarBacsErroneosAsync()
        {
            return await _dbService.ExecuteNonQueryAsync("LiberarBacsErroneos");
        }

        /// <summary>
        /// Libera BACs de un grupo
        /// </summary>
        public async Task<int> LiberarBacsGrupoAsync(int idGrupo)
        {
            var parameters = new Dictionary<string, object>
            {
                { "IdGrupo", idGrupo }
            };

            return await _dbService.ExecuteNonQueryAsync("LiberarBacsGrupo", parameters);
        }

        #endregion
    }
}
