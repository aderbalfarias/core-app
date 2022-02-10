using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Repositories;
using CoreApp.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace CoreApp.Domain.Services;

public class OpenService : IOpenService
{
    private readonly IBaseRepository _baseRepository;
    private readonly ILogger _logger;

    public OpenService
    (
        IBaseRepository baseRepository,
        ILogger<DemoService> logger
    )
    {
        _baseRepository = baseRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<OpenEntity>> GetAll()
    {
        return await Task.FromResult(new List<OpenEntity>
            {
                new OpenEntity
                {
                    Id = 1,
                    Value = "Open check",
                    Date = DateTime.Now
                },
                new OpenEntity
                {
                    Id = 2,
                    Value = "Open now check",
                    Date = DateTime.Now
                }
            });

        //return await _baseRepository.GetAsync<OpenEntity>();
    }

    public async Task<OpenEntity> GetById(int id)
    {
        return await Task.FromResult(new OpenEntity
        {
            Id = id,
            Value = "Open check",
            Date = DateTime.Now
        });

        //return await _baseRepository
        //    .GetObjectAsync<OpenEntity>(p => p.Id == id);
    }
}

