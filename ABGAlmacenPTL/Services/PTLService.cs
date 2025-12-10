using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABGAlmacenPTL.Data.Repositories;
using ABGAlmacenPTL.Models;
using Microsoft.EntityFrameworkCore;

namespace ABGAlmacenPTL.Services
{
    /// <summary>
    /// Service for PTL (Pick-to-Light) operations
    /// </summary>
    public class PTLService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PTLService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Article operations
        public async Task<Articulo?> GetArticuloByCodigoAsync(string codigo)
        {
            return await _unitOfWork.Articulos.FindFirstAsync(a => a.CodigoArticulo == codigo);
        }

        public async Task<Articulo?> GetArticuloByEAN13Async(string ean13)
        {
            return await _unitOfWork.Articulos.FindFirstAsync(a => a.EAN13 == ean13);
        }

        // BAC operations
        public async Task<BAC?> GetBACByCodigoAsync(string codigoBAC)
        {
            return await _unitOfWork.BACs.FindFirstAsync(b => b.CodigoBAC == codigoBAC);
        }

        public async Task<IEnumerable<Articulo>> GetArticulosEnBACAsync(string codigoBAC)
        {
            var bacArticulos = await _unitOfWork.BACArticulos.FindAsync(ba => ba.CodigoBAC == codigoBAC);
            var codigosArticulos = bacArticulos.Select(ba => ba.CodigoArticulo).ToList();
            
            var articulos = new List<Articulo>();
            foreach (var codigo in codigosArticulos)
            {
                var articulo = await _unitOfWork.Articulos.FindFirstAsync(a => a.CodigoArticulo == codigo);
                if (articulo != null)
                {
                    articulos.Add(articulo);
                }
            }
            return articulos;
        }

        public async Task<bool> AsignarBACaUbicacionAsync(string codigoBAC, string codigoUbicacion, EstadoBAC estado)
        {
            try
            {
                var bac = await GetBACByCodigoAsync(codigoBAC);
                if (bac == null) return false;

                var ubicacion = await _unitOfWork.Ubicaciones.FindFirstAsync(u => u.CodigoUbicacion == codigoUbicacion);
                if (ubicacion == null) return false;

                bac.CodigoUbicacion = codigoUbicacion;
                bac.Estado = estado;
                bac.FechaModificacion = DateTime.Now;

                _unitOfWork.BACs.Update(bac);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ExtraerBACDeUbicacionAsync(string codigoUbicacion, EstadoBAC nuevoEstado)
        {
            try
            {
                var bac = await _unitOfWork.BACs.FindFirstAsync(b => b.CodigoUbicacion == codigoUbicacion);
                if (bac == null) return false;

                bac.CodigoUbicacion = null;
                bac.Estado = nuevoEstado;
                bac.FechaModificacion = DateTime.Now;

                _unitOfWork.BACs.Update(bac);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Location operations
        public async Task<Ubicacion?> GetUbicacionByCodigoAsync(string codigoUbicacion)
        {
            return await _unitOfWork.Ubicaciones.FindFirstAsync(u => u.CodigoUbicacion == codigoUbicacion);
        }

        public async Task<BAC?> GetBACEnUbicacionAsync(string codigoUbicacion)
        {
            return await _unitOfWork.BACs.FindFirstAsync(b => b.CodigoUbicacion == codigoUbicacion);
        }

        // Box/Caja operations
        public async Task<Caja?> GetCajaBySSCCAsync(string sscc)
        {
            return await _unitOfWork.Cajas.FindFirstAsync(c => c.SSCC == sscc);
        }

        public async Task<IEnumerable<Articulo>> GetArticulosEnCajaAsync(string sscc)
        {
            var cajaArticulos = await _unitOfWork.CajaArticulos.FindAsync(ca => ca.SSCC == sscc);
            var codigosArticulos = cajaArticulos.Select(ca => ca.CodigoArticulo).ToList();
            
            var articulos = new List<Articulo>();
            foreach (var codigo in codigosArticulos)
            {
                var articulo = await _unitOfWork.Articulos.FindFirstAsync(a => a.CodigoArticulo == codigo);
                if (articulo != null)
                {
                    articulos.Add(articulo);
                }
            }
            return articulos;
        }

        public async Task<string> CrearNuevaCajaAsync(int tipoId)
        {
            // Generate SSCC (18 digits)
            string sscc = GenerarSSCC();
            
            var caja = new Caja
            {
                SSCC = sscc,
                TipoId = tipoId,
                Estado = EstadoCaja.Abierta,
                Unidades = 0,
                Peso = 0,
                Volumen = 0,
                FechaCreacion = DateTime.Now
            };

            await _unitOfWork.Cajas.AddAsync(caja);
            await _unitOfWork.SaveChangesAsync();
            
            return sscc;
        }

        public async Task<bool> EmpaquetarBACEnCajaAsync(string codigoBAC, string sscc)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var bac = await GetBACByCodigoAsync(codigoBAC);
                var caja = await GetCajaBySSCCAsync(sscc);

                if (bac == null || caja == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                // Transfer articles from BAC to Caja
                var bacArticulos = await _unitOfWork.BACArticulos.FindAsync(ba => ba.CodigoBAC == codigoBAC);
                foreach (var bacArticulo in bacArticulos)
                {
                    var cajaArticulo = new CajaArticulo
                    {
                        SSCC = sscc,
                        CodigoArticulo = bacArticulo.CodigoArticulo,
                        Cantidad = bacArticulo.Cantidad
                    };
                    await _unitOfWork.CajaArticulos.AddAsync(cajaArticulo);

                    // Update caja totals
                    caja.Unidades += bacArticulo.Cantidad;
                }

                // Remove BAC articles
                _unitOfWork.BACArticulos.RemoveRange(bacArticulos);

                // Update BAC state
                bac.Estado = EstadoBAC.Cerrado;
                bac.Unidades = 0;
                bac.FechaModificacion = DateTime.Now;
                _unitOfWork.BACs.Update(bac);

                // Update Caja
                _unitOfWork.Cajas.Update(caja);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }

        public async Task<bool> CerrarCajaAsync(string sscc)
        {
            try
            {
                var caja = await GetCajaBySSCCAsync(sscc);
                if (caja == null) return false;

                caja.Estado = EstadoCaja.Cerrada;
                caja.FechaCierre = DateTime.Now;

                _unitOfWork.Cajas.Update(caja);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Workstation operations
        public async Task<IEnumerable<Puesto>> GetPuestosActivosAsync()
        {
            return await _unitOfWork.Puestos.FindAsync(p => p.Activo);
        }

        public async Task<Puesto?> GetPuestoByNumeroAsync(int numero)
        {
            return await _unitOfWork.Puestos.FindFirstAsync(p => p.Numero == numero);
        }

        // Helper methods
        private string GenerarSSCC()
        {
            const string SPAIN_PREFIX = "384"; // GS1 Spain
            const string COMPANY_CODE = "12345";
            
            // Generate 9-digit serial number
            Random random = new Random();
            int serial = random.Next(100000000, 999999999);
            
            // Build 17-digit code (prefix + company + serial)
            string sscc17 = $"{SPAIN_PREFIX}{COMPANY_CODE}{serial}";
            
            // Calculate check digit (mod 10)
            int checkDigit = CalcularDigitoControl(sscc17);
            
            return $"{sscc17}{checkDigit}";
        }

        private int CalcularDigitoControl(string codigo)
        {
            int suma = 0;
            for (int i = codigo.Length - 1; i >= 0; i--)
            {
                int digito = int.Parse(codigo[i].ToString());
                suma += ((codigo.Length - i) % 2 == 0) ? digito * 3 : digito;
            }
            int modulo = suma % 10;
            return (modulo == 0) ? 0 : (10 - modulo);
        }
    }
}
