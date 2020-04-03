using CoreApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp.Domain.Interfaces.Services
{
    public interface IDemoService
    {
        Task<List<DemoEntity>> GetAll();
        Task<DemoEntity> GetById(int id);
        Task<DemoEntity> GetDetails(int id, int entityId);
        Task Save(DemoEntity model);
    }
}
