using CoreApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp.Domain.Interfaces.Services
{
    public interface IOpenService
    {
        Task<IEnumerable<OpenEntity>> GetAll();
        Task<OpenEntity> GetById(int id);
    }
}
