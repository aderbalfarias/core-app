using CoreApp.Domain.Entities;

namespace CoreApp.Domain.Interfaces.Services
{
    public interface IOpenService
    {
        Task<IEnumerable<OpenEntity>> GetAll();
        Task<OpenEntity> GetById(int id);
    }
}
