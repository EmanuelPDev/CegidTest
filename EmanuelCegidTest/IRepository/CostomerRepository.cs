using EmanuelCegidTest.Context;
using EmanuelCegidTest.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CostomerRepository : Repository<Customers>, ICostomerRepository
    {
        public CostomerRepository(AppDbContext contexto) : base(contexto)
        {
        }
    }
}
