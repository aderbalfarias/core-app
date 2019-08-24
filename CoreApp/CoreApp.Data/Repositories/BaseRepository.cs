using CoreApp.Data.Mappings;
using CoreApp.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.Data.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly DbContext _context;

        public BaseRepository(PrimaryContext context)
        {
            _context = context;
        }
    }
}
