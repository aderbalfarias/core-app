using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Repositories;
using CoreApp.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp.Domain.Services
{
    public class DemoService : IDemoService
    {
        private readonly IBaseRepository _baseRepository;
        private readonly ILogger _logger;

        public DemoService
        (
            IBaseRepository baseRepository,
            ILogger<DemoService> logger
        )
        {
            _baseRepository = baseRepository;
            _logger = logger;
        }

        public Task<List<DemoEntity>> GetAll()
        {
            _logger.LogInformation("Method get all was called");

            var list = new List<DemoEntity>
                {
                    new DemoEntity
                    {
                        Id = 1,
                        Description = "Test"
                    }
                };

            return Task.FromResult(list);
        }

        public async Task<DemoEntity> GetById(int id)
        {
            return await Task.FromResult(new DemoEntity());
        }

        public async Task<DemoEntity> GetDetails(int id, int entityId)
        {
            return await Task.FromResult(new DemoEntity());
        }

        public Task Save(DemoEntity model)
        {
            return Task.CompletedTask;
        }
    }
}
