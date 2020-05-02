using CoreApp.Api.Models;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CoreApp.Api.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}")]
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

        // GET api/demos
        [HttpGet]
        [EnableCors]
        [ApiVersion("2.0")]
        [Route("demos")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("GetAll called");

            var result = new { Teste = "Test", Code = 2, Message = "Success" };

            return Ok(result);
        }

        // GET api/demos/{id}
        [HttpGet]
        [EnableCors]
        [Route("demos/{id:int:min(1)}")]
        public IActionResult GetById(int id) => Ok(_demoService.GetById(id));

        // GET api/demos/{id}/{modelId}
        [HttpGet]
        [EnableCors]
        [Route("demos/{id:int:min(1)}/{modelId:int:min(1)}")]
        public async Task<IActionResult> GetDetails(int id, int modelId)
            => Ok(await _demoService.GetDetails(id, modelId));

        // POST api/demos
        [HttpPost]
        [EnableCors]
        [Route("demos")]
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
