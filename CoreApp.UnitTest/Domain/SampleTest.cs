using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Repositories;
using CoreApp.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace CoreApp.UnitTest.Domain
{
    public class DemoTest
    {
        #region Fields 

        private readonly Mock<IBaseRepository> _mockBaseRepository;
        private readonly Mock<ILogger<DemoService>> _mockLogger;
        private readonly DemoService _demoServcice;

        #endregion End Fields 

        #region Constructor

        public DemoTest()
        {
            _mockBaseRepository = new Mock<IBaseRepository>();
            _mockLogger = new Mock<ILogger<DemoService>>();

            _demoServcice = new DemoService(_mockBaseRepository.Object, _mockLogger.Object);
        }

        #endregion End Constructor

        #region Setups 

        private Task RepositorySetup()
        {
            _mockBaseRepository.Setup(s => s.Add(It.IsAny<DemoEntity>()))
                .Returns(Task.FromResult(1));

            IQueryable<DemoEntity> mocks = MockDemoEntity.AsQueryable();

            _mockBaseRepository.Setup(s
                    => s.Get(It.IsAny<Expression<Func<DemoEntity, bool>>>(), It.IsAny<string>()))
                .Returns<Expression<Func<DemoEntity, bool>>, string>((predicate, include)
                        => mocks.Where(predicate));

            _mockBaseRepository.Setup(s
                    => s.GetAsync(It.IsAny<Expression<Func<DemoEntity, bool>>>()))
                .Returns<Expression<Func<DemoEntity, bool>>>(async (predicate)
                        => await Task.FromResult(mocks.Where(predicate)));

            _mockBaseRepository.Setup(s
                    => s.GetAsync(It.IsAny<Expression<Func<DemoEntity, bool>>>(), It.IsAny<string>()))
                .Returns<Expression<Func<DemoEntity, bool>>, string>(async (predicate, include)
                        => await Task.FromResult(mocks.Where(predicate)));

            _mockBaseRepository.Setup(s
                    => s.GetObjectAsync(It.IsAny<Expression<Func<DemoEntity, bool>>>()))
                .Returns<Expression<Func<DemoEntity, bool>>>(predicate
                        => Task.FromResult(mocks.FirstOrDefault(predicate)));

            return Task.CompletedTask;
        }

        #endregion End Setups 

        #region Mocks

        private IEnumerable<DemoEntity> MockDemoEntity
            => new List<DemoEntity>
            {
                new DemoEntity
                {
                    Id = 1,
                    Description = "Test 1"
                },
                new DemoEntity
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

        [Fact]
        public async Task GetAll_Should_Log_Info()
        {
            await RepositorySetup();

            await _demoServcice.GetAll();

            var getAllCalled = "Method get all was called";

            _mockLogger.Verify(x =>
                x.Log(LogLevel.Information, It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals(getAllCalled, o.ToString())),
                    It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task Save_Should_Log_Exception()
        {
            await RepositorySetup();
            var entity = MockDemoEntity.First();

            var logException = await Record
                .ExceptionAsync(async () => await _demoServcice.Save(entity));

            Assert.Contains("Method not Implemented", logException.Message);
        }

        [Fact]
        public async Task Save_Should_Throw_Exception()
        {
            await RepositorySetup();
            var entity = MockDemoEntity.First();

            await Assert.ThrowsAsync<NotImplementedException>(async ()
                => await _demoServcice.Save(entity));
        }

        [Fact]
        public async Task Save_Should_Throw_And_Check_Exception()
        {
            await RepositorySetup();
            var entity = MockDemoEntity.First();

            var exception = await Assert
                .ThrowsAsync<NotImplementedException>(async ()
                    => await _demoServcice.Save(entity));

            Assert.NotNull(exception);
        }

        [Fact]
        public async Task Save_Should_Check_Exception()
        {
            await RepositorySetup();
            var entity = MockDemoEntity.First();

            await Assert.ThrowsAsync<NotImplementedException>(async ()
                => await _demoServcice.Save(entity));
        }

        #endregion End Tests
    }
}