using CoreApp.Api.Models;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CoreApp.Api.Controllers
{
    //[Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IDemoService _demoService;
        private readonly ILogger _logger;

        public DemoController
        (
            IDemoService DemoService,
            ILogger<DemoController> logger
        )
        {
            _demoService = DemoService;
            _logger = logger;
        }

        // GET api/demo/getall
        [HttpGet]
        [EnableCors]
        [Route("getall")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("GetAll called");
            return Ok(new { Teste = "Test", Code = 2, Message = "Success" });
        }

        // GET api/demo/getbyid/{id}
        [HttpGet]
        [EnableCors]
        [Route("getbyid/{id:int:min(1)}")]
        public IActionResult GetById(int id) => Ok(_demoService.GetById(id));

        // GET api/demo/getdetails/{id}/{modelId}
        [HttpGet]
        [EnableCors]
        [Route("getdetails/{id:int:min(1)}/{modelId:int:min(1)}")]
        public async Task<IActionResult> GetDetails(int id, int modelId)
            => Ok(await _demoService.GetDetails(id, modelId));

        // POST api/demo/save
        [HttpPost]
        [EnableCors]
        [Route("save")]
        public IActionResult Save(DemoModel model)
        {
            _logger.LogInformation("Save method started");

            if (model == null)
            {
                _logger.LogWarning("Save method did not find any value in the model of the request");

                return NoContent();
            }

            _demoService.Save(new DemoEntity
            {
                Id = model.Id,
                Description = model.Description,
                Text = model.Text
            });

            _logger.LogInformation("Save method finished successfully");

            return Ok();
        }
    }
}
