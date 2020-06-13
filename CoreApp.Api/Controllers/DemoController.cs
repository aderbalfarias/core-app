using CoreApp.Api.Models;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        public async Task<IActionResult> GetAllAsync()
        {
            _logger.LogInformation("GetAll called");

            var result = await _demoService.GetAll();

            return Ok(result);
        }

        // GET api/demos/{id}
        [HttpGet]
        [EnableCors]
        [Route("demos/{id:int:min(1)}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _demoService.GetById(id));

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
        public async Task<IActionResult> Save(DemoModel model)
        {
            var controller = typeof(DemoController).Name;

            _logger.LogInformation($"Save method on {controller} started");

            if (model == null)
            {
                _logger.LogWarning($"Save method on {controller} did not find any value in the model of the request");

                return NoContent();
            }

            await _demoService.Save(new DemoEntity
            {
                Id = model.Id,
                Description = model.Description,
                Text = model.Text,
                Date = DateTime.UtcNow
            });

            _logger.LogInformation($"Save method on {controller} finished successfully");

            return Ok();
        }

        // PUT api/demos
        [HttpPut]
        [EnableCors]
        [Route("demos/{id:int:min(1)}")]
        public async Task<IActionResult> Update(int id, DemoModel model)
        {
            var controller = typeof(DemoController).Name;

            _logger.LogInformation($"Update method on {controller} started");

            if (model == null)
            {
                _logger.LogWarning($"Update method on {controller} did not find any value in the model of the request");

                return NoContent();
            }

            await _demoService.Save(new DemoEntity
            {
                Id = model.Id,
                Description = model.Description,
                Text = model.Text
            }, id);

            _logger.LogInformation($"Update method on {controller} finished successfully");

            return Ok();
        }

        // DELETE api/demos
        [HttpDelete]
        [EnableCors]
        [Route("demos/{id:int:min(1)}")]
        public async Task<IActionResult> Delete(int id)
        {
            var controller = typeof(DemoController).Name;

            _logger.LogInformation($"Delete method on {controller} started");

            await _demoService.Delete(id);

            _logger.LogInformation($"Delete method on {controller} finished successfully");

            return Ok();
        }
    }
}
