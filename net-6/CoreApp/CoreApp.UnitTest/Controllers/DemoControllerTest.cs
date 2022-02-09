using CoreApp.Api.Controllers;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp.UnitTest.Controllers
{
    public class DemoControllerTest
    {
        #region Fields 

        private readonly Mock<IDemoService> _mockDemoService;
        private readonly Mock<ILogger<DemoController>> _mockLogger;
        private readonly DemoController _demoController;

        #endregion End Fields 

        #region Constructor

        public DemoControllerTest()
        {
            _mockDemoService = new Mock<IDemoService>();
            _mockLogger = new Mock<ILogger<DemoController>>();

            _demoController = new DemoController(_mockDemoService.Object, _mockLogger.Object);
        }

        #endregion End Constructor

        #region Setups 

        private Task ServiceSetup()
        {
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
                    Text = "test controller"
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