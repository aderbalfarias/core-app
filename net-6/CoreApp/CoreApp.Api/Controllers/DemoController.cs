using CoreApp.Api.Models;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CoreApp.Api.Controllers;

/// <summary>
/// Controller to allow demos management 
/// </summary>
//[Authorize]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//[Authorize(AuthenticationSchemes = "ADFS, Bearer", Roles = "Master, Admin")]
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

    /// <summary>
    /// Get all demo records from database
    /// </summary>
    /// <remarks>
    /// Sample Request: 
    /// 
    ///     GET api/v2/demos
    /// 
    /// Sample Response:
    /// 
    ///     [
    ///         {
    ///             Id: 1,
    ///             Text: "x1",
    ///             Description: "x2",
    ///             Presenter: "x3",
    ///             Date: "2020-01-01"
    ///         },
    ///         {
    ///             Id: 2,
    ///             Text: "x x1",
    ///             Description: "x x2",
    ///             Presenter: "x x3",
    ///             Date: "2020-01-01"
    ///         }
    ///     ]
    ///     
    /// </remarks>
    /// <response code="200">Returns a collection of demos entity</response>
    /// <response code="401">Returns Not Authorized</response>
    /// <returns>Returns a collection of demos entity</returns>
    /// <param></param>
    [HttpGet]
    [EnableCors]
    [ApiVersion("2.0")]
    [Route("demos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        _logger.LogInformation("GetAllAsync called");

        var result = await _demoService.GetAll();

        return Ok(result);
    }

    /// <summary>
    /// Get a specific demo data based on its id
    /// </summary>
    /// <remarks>
    /// Sample Request: 
    /// 
    ///     GET api/v2/demos/{id}
    /// 
    /// Sample Response:
    /// 
    ///     [
    ///         {
    ///             Id: 1,
    ///             Text: "x1",
    ///             Description: "x2",
    ///             Presenter: "x3",
    ///             Date: "2020-01-01"
    ///         }
    ///     ]
    ///     
    /// </remarks>
    /// <response code="200">Returns a demo json object</response>
    /// <response code="401">Returns Not Authorized</response>
    /// <returns>Returns a demo json object</returns>
    /// <param name="id">Demo Identifier</param>
    [HttpGet]
    [EnableCors]
    [Route("demos/{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _demoService.GetById(id));

    /// <summary>
    /// Get a specific demo data based on its id and modelId
    /// </summary>
    /// <remarks>
    /// Sample Request: 
    /// 
    ///     GET api/v1/demos/{id}/{modelId}
    /// 
    /// Sample Response:
    /// 
    ///     [
    ///         {
    ///             Id: 1,
    ///             Text: "x1",
    ///             Description: "x2",
    ///             Presenter: "x3",
    ///             Date: "2020-01-01"
    ///         }
    ///     ]
    ///     
    /// </remarks>
    /// <response code="200">Returns a demo json object</response>
    /// <response code="401">Returns Not Authorized</response>
    /// <returns>Returns a demo json object</returns>
    /// <param name="id">Demo Identifier</param>
    /// <param name="modelId">Model Identifier</param>
    [HttpGet]
    [EnableCors]
    [Route("demos/{id:int:min(1)}/{modelId:int:min(1)}")]
    public async Task<IActionResult> GetDetails(int id, int modelId)
        => Ok(await _demoService.GetDetails(id, modelId));

    /// <summary>
    /// Create a new demo object
    /// </summary>
    /// <remarks>
    /// Sample Request: 
    /// 
    ///     POST api/v1/demos
    ///     
    /// Sample Model:
    /// 
    ///     [
    ///         {
    ///             Text: "x1",
    ///             Description: "x2",
    ///             Presenter: "x3",
    ///             Date: "2020-01-01"
    ///         }
    ///     ]
    ///     
    /// </remarks>
    /// <response code="200">Returns Ok</response>
    /// <response code="204">Returns NoContent</response>
    /// <response code="401">Returns Not Authorized</response>
    /// <response code="500">Returns Internal Error</response>
    /// <returns>Returns Ok, NoContent or Internal Error</returns>
    /// <param name="model">Demo Model</param>
    [HttpPost]
    [EnableCors]
    [Route("demos")]
    public async Task<IActionResult> Save(DemoModel model)
    {
        var controller = typeof(DemoController).Name;

        _logger.LogInformation($"Save method on {controller} started");

        if (model == null)
        {
            _logger.LogWarning($"Save method on {controller} did not find any value in the request");

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

    /// <summary>
    /// Update demo object
    /// </summary>
    /// <remarks>
    /// Sample Request: 
    /// 
    ///     PUT api/v1/demos/{id}
    ///     
    /// Sample Model:
    /// 
    ///     [
    ///         {
    ///             Id: 1,
    ///             Text: "x1",
    ///             Description: "x2",
    ///             Presenter: "x3",
    ///             Date: "2020-01-01"
    ///         }
    ///     ]
    ///     
    /// </remarks>
    /// <response code="200">Returns Ok</response>
    /// <response code="204">Returns NoContent</response>
    /// <response code="401">Returns Not Authorized</response>
    /// <response code="500">Returns Internal Error</response>
    /// <returns>Returns Ok, NoContent or Internal Error</returns>
    /// <param name="id">Demo Identifier</param>
    /// <param name="model">Demo Model</param>
    [HttpPut]
    [EnableCors]
    [Route("demos/{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, DemoModel model)
    {
        var controller = typeof(DemoController).Name;

        _logger.LogInformation($"Update method on {controller} started");

        if (model == null)
        {
            _logger.LogWarning($"Update method on {controller} did not find any value in the request");

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

    /// <summary>
    /// Remove demo object
    /// </summary>
    /// <remarks>
    /// Sample Request: 
    /// 
    ///     DELETE api/v1/demos/{id}
    ///     
    /// </remarks>
    /// <response code="200">Returns Ok</response>
    /// <response code="401">Returns Not Authorized</response>
    /// <response code="500">Returns Internal Error</response>
    /// <returns>Returns Ok or Internal Error</returns>
    /// <param name="id">Demo Identifier</param>
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
