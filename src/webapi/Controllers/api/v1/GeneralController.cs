using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace VVJ.AppWithDataDog.WebApi.Controllers.api.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GeneralController : ControllerBase
    {
        private ILogger<GeneralController> _logger;
        public GeneralController(ILogger<GeneralController> logger)
        {
            _logger = logger;
        }
        [HttpGet("time")]
        public async Task<ActionResult<DateTime>> GetTime()
        {
            return await Task.FromResult<ActionResult<DateTime>>(Ok(DateTime.Now));
        }

        [HttpGet("error")]
        public async Task<ActionResult<string>> GetError()
        {
            throw new Exception("general.error");
        }

        [HttpGet("handled-error")]
        public async Task<ActionResult<string>> GetHandledError()
        {
            try
            {
                throw new Exception("general.error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return await Task.FromResult<ActionResult<string>>(StatusCode(StatusCodes.Status500InternalServerError, "handled error"));
            }
        }

    }
}
