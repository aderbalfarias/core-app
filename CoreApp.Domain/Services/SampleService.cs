using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Repositories;
using CoreApp.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp.Domain.Services
{
    public class SampleService : ISampleService
    {
        private readonly IBaseRepository _baseRepository;
        private readonly ILogger _logger;

        public SampleService(
            IBaseRepository baseRepository, 
            ILogger<SampleService> logger
        )
        {
            _baseRepository = baseRepository;
            _logger = logger;
        }

        public Task<List<SampleEntity>> GetAll()
        {
            _logger.LogInformation("Method get all was called");

            var list = new List<SampleEntity>
                {
                    new SampleEntity 
                    {
                        Id = 1,
                        Description = "Test"
                    }
                };

            return Task.FromResult(list);
        }

        public Task<SampleEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SampleEntity> GetDetails(int id, int entityId)
        {
            throw new NotImplementedException();
        }

        public Task Save(SampleEntity model)
        {
            throw new NotImplementedException("Method not Implemented");

            //Task.CompletedTask;
        }
    }
}
