using CoreApp.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.Data.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly DbContext _context = new PrimaryContext();

        public BaseRepository()
        {
        }

    }
}
