using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Repositories;
using CoreApp.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp.Domain.Services
{
    public class SampleService : ITestService
    {
        private readonly IBaseRepository _baseRepository;

        public SampleService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public Task<IEnumerable<SampleEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SampleEntity> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SampleEntity> GetDetails(int id, int entityId)
        {
            throw new NotImplementedException();
        }

        public void Save(SampleEntity model)
        {
            throw new NotImplementedException();
        }
    }
}
