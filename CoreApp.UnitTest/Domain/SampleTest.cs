using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Repositories;
using CoreApp.Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreApp.UnitTest.Domain
{
    public class SampleTest
    {
        #region Fields 

        private readonly Mock<IBaseRepository> _mockBaseRepository;
        private readonly SampleService _sampleServcice;

        #endregion End Fields 

        #region Constructor

        public SampleTest()
        {
            _mockBaseRepository = new Mock<IBaseRepository>();

            _sampleServcice = new SampleService(_mockBaseRepository.Object);
        }

        #endregion End Constructor

        #region Setups 

        private Task RepositorySetup()
        {
            _mockBaseRepository.Setup(s => s.Add(It.IsAny<SampleEntity>()))
                .Returns(Task.FromResult(1));

            IQueryable<SampleEntity> mocks = MockSampleEntity.AsQueryable();

            //_mockBaseRepository.Setup(s
            //        => s.GetObjectWithInclude(It.IsAny<Expression<Func<SampleEntity, bool>>>(), It.IsAny<string>()))
            //    .Returns<Expression<Func<SampleEntity, bool>>, string>((predicate, include)
            //            => Task.FromResult(mocks.FirstOrDefault(predicate)));

            _mockBaseRepository.Setup(s
                    => s.Get(It.IsAny<Expression<Func<SampleEntity, bool>>>(), It.IsAny<string>()))
                .Returns<Expression<Func<SampleEntity, bool>>, string>((predicate, include)
                        => mocks.Where(predicate));

            _mockBaseRepository.Setup(s
                    => s.GetObjectAsync(It.IsAny<Expression<Func<SampleEntity, bool>>>()))
                .Returns<Expression<Func<SampleEntity, bool>>>(predicate
                        => Task.FromResult(mocks.FirstOrDefault(predicate)));

            return Task.CompletedTask;
        }

        #endregion End Setups 

        #region Mocks

        private IEnumerable<SampleEntity> MockSampleEntity
            => new List<SampleEntity>
            {
                new SampleEntity
                {
                    Id = 1,
                    Description = "Test 1"
                },
                new SampleEntity
                {
                    Id = 2,
                    Description = "Test 2",
                    Text = "test"
                },
            };

        private Task MockAppSettings()
        {
            // Properties need to be virtual because we are mocking a concrete class
            //_appSettings.SetupGet(s => s.Folder).Returns("Test");

            // IOptions interface
            //_appSettings.SetupGet(s => s.Value).Returns(new AppSettings { Id = 3 });

            return Task.CompletedTask;
        }

        #endregion End Mocks

        #region Tests

        #endregion End Tests
    }
}