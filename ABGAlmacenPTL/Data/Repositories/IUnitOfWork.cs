using System;
using System.Threading.Tasks;
using ABGAlmacenPTL.Models;

namespace ABGAlmacenPTL.Data.Repositories
{
    /// <summary>
    /// Unit of Work pattern interface for coordinating repository operations
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Repository properties
        IRepository<Articulo> Articulos { get; }
        IRepository<BAC> BACs { get; }
        IRepository<Ubicacion> Ubicaciones { get; }
        IRepository<Caja> Cajas { get; }
        IRepository<TipoCaja> TiposCaja { get; }
        IRepository<Puesto> Puestos { get; }
        IRepository<Usuario> Usuarios { get; }
        IRepository<BACArticulo> BACArticulos { get; }
        IRepository<CajaArticulo> CajaArticulos { get; }

        // Transaction management
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
