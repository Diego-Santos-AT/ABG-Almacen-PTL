using System;
using System.Threading.Tasks;
using ABGAlmacenPTL.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace ABGAlmacenPTL.Data.Repositories
{
    /// <summary>
    /// Unit of Work implementation for managing database operations
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ABGAlmacenContext _context;
        private IDbContextTransaction? _transaction;

        // Repository instances
        private IRepository<Articulo>? _articulos;
        private IRepository<BAC>? _bacs;
        private IRepository<Ubicacion>? _ubicaciones;
        private IRepository<Caja>? _cajas;
        private IRepository<TipoCaja>? _tiposCaja;
        private IRepository<Puesto>? _puestos;
        private IRepository<Usuario>? _usuarios;
        private IRepository<BACArticulo>? _bacArticulos;
        private IRepository<CajaArticulo>? _cajaArticulos;

        public UnitOfWork(ABGAlmacenContext context)
        {
            _context = context;
        }

        // Lazy initialization of repositories
        public IRepository<Articulo> Articulos =>
            _articulos ??= new Repository<Articulo>(_context);

        public IRepository<BAC> BACs =>
            _bacs ??= new Repository<BAC>(_context);

        public IRepository<Ubicacion> Ubicaciones =>
            _ubicaciones ??= new Repository<Ubicacion>(_context);

        public IRepository<Caja> Cajas =>
            _cajas ??= new Repository<Caja>(_context);

        public IRepository<TipoCaja> TiposCaja =>
            _tiposCaja ??= new Repository<TipoCaja>(_context);

        public IRepository<Puesto> Puestos =>
            _puestos ??= new Repository<Puesto>(_context);

        public IRepository<Usuario> Usuarios =>
            _usuarios ??= new Repository<Usuario>(_context);

        public IRepository<BACArticulo> BACArticulos =>
            _bacArticulos ??= new Repository<BACArticulo>(_context);

        public IRepository<CajaArticulo> CajaArticulos =>
            _cajaArticulos ??= new Repository<CajaArticulo>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
