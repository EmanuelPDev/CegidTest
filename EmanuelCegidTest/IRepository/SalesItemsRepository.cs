using EmanuelCegidTest.Context;
using EmanuelCegidTest.Models;

namespace APICatalogo.Repository
{
    public class SalesItemsRepository : Repository<SalesItems>, ISalesItemsRepository
    {
        public SalesItemsRepository(AppDbContext contexto) : base(contexto)
        {
        }

     
    }
}
