using EmanuelCegidTest.Context;

namespace APICatalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private SalesItemsRepository _SalesItemsRepo;
        private CostomerRepository _CostomerRepo;
        public AppDbContext _context;
        public UnitOfWork(AppDbContext contexto)
        {
            _context = contexto;
        }

        public ISalesItemsRepository SalesItemsRepository
        {
            get
            {
                return _SalesItemsRepo = _SalesItemsRepo ?? new SalesItemsRepository(_context);
            }
        }

        public ICostomerRepository CostomerRepository
        {
            get
            {
                return _CostomerRepo = _CostomerRepo ?? new CostomerRepository(_context);
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
