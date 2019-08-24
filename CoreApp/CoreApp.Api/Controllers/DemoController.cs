using CoreApp.Api.Models;
using CoreApp.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CoreApp.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/demo")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        //[Route("api/[controller]")]
        
        public class TestController : ControllerBase
        {
            private readonly ITestService _testService;
            private readonly ILogger _logger;

            public TestController(ITestService testService, ILogger<TestController> logger)
            {
                _testService = testService;
                _logger = logger;
            }

            // GET api/demo/getall
            [HttpGet]
            [EnableCors]
            [Route("getall")]
            public IActionResult GetAll() => Ok(_testService.GetAll());

            // GET api/demo/getbyid/{id}
            [HttpGet]
            [EnableCors]
            [Route("getbyid/{id:int:min(1)}")]
            public IActionResult GetById(int id) => Ok(_testService.GetById(id));

            // GET api/demo/getdetails/{id}
            [HttpGet]
            [EnableCors]
            [Route("getdetails/{id:int:min(1)}/{id:int:min(1)}")]
            public async Task<IActionResult> GetDetails(int id, int modelId) 
                => Ok(await _testService.GetDetails(id, modelId));

            // POST api/demo/save
            [HttpPost]
            [EnableCors]
            [Route("save)}")]
            public IActionResult Save(DemoModel model) => Ok(_testService.Save(model));
        }
    }
}
