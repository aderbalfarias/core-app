using CoreApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp.Domain.Interfaces.Services
{
    public interface ITestService
    {
        Task<IEnumerable<TestEntity>> GetAll();
        Task<TestEntity> GetById(int id);
        Task<TestEntity> GetDetails(int id, int entityId);
        void Save(TestEntity model);
    }
}
