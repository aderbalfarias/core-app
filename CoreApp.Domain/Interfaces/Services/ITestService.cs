using CoreApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp.Domain.Interfaces.Services
{
    public interface ITestService
    {
        Task<IEnumerable<SampleEntity>> GetAll();
        Task<SampleEntity> GetById(int id);
        Task<SampleEntity> GetDetails(int id, int entityId);
        void Save(SampleEntity model);
    }
}
