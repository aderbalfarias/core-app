using CoreApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp.Domain.Interfaces.Services
{
    public interface IDemoService
    {
        Task<IEnumerable<DemoEntity>> GetAll();
        Task<DemoEntity> GetById(int id);
        Task<DemoEntity> GetDetails(int id, int entityId);
        Task Save(DemoEntity entity);
        Task Update(DemoEntity entity);
    }
}
