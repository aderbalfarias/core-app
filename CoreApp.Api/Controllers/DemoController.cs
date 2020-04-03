﻿using CoreApp.Api.Models;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CoreApp.Api.Controllers
{
    //[Authorize]
    [Route("api/demo")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly ISampleService _sampleService;
        private readonly ILogger _logger;

        public DemoController
        (
            ISampleService sampleService,
            ILogger<DemoController> logger
        )
        {
            _sampleService = sampleService;
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
        public IActionResult GetById(int id) => Ok(_sampleService.GetById(id));

        // GET api/demo/getdetails/{id}/{modelId}
        [HttpGet]
        [EnableCors]
        [Route("getdetails/{id:int:min(1)}/{modelId:int:min(1)}")]
        public async Task<IActionResult> GetDetails(int id, int modelId)
            => Ok(await _sampleService.GetDetails(id, modelId));

        // POST api/demo/save
        [HttpPost]
        [EnableCors]
        [Route("save")]
        public IActionResult Save(DemoModel model)
        {
            _logger.LogInformation("Save method started");

            if (model == null)
                return NoContent();

            _sampleService.Save(new SampleEntity
            {
                Id = model.Id,
                Description = model.Description,
                Text = model.Text
            });

            _logger.LogInformation("Save method finished");

            return Ok();
        }
    }
}
