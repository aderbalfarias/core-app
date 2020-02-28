using CoreApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp.Domain.Interfaces.Services
{
    public interface ISampleService
    {
        Task<List<SampleEntity>> GetAll();
        Task<SampleEntity> GetById(int id);
        Task<SampleEntity> GetDetails(int id, int entityId);
        Task Save(SampleEntity model);
    }
}
