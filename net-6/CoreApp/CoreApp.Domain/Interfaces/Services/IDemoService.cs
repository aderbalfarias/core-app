using CoreApp.Domain.Entities;

namespace CoreApp.Domain.Interfaces.Services;

public interface IDemoService
{
    Task<IEnumerable<DemoEntity>> GetAll();
    Task<DemoEntity> GetById(int id);
    Task<DemoEntity> GetDetails(int id, int entityId);
    Task Save(DemoEntity entity, int id = 0);
    Task Test(DemoEntity entity);
    Task Delete(int id);
}

