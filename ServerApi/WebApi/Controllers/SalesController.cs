using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server_Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server_Model.Base;
using Server_Data.EntityDb;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;
        private readonly ApplicationDatabaseContext _db;
        private readonly EntitysModel _model;

        public SalesController(ILogger<SalesController> logger, ApplicationDatabaseContext db, EntitysModel model)
        {
            this._logger = logger;
            this._db = db;
            this._model = model;
        }

        [HttpGet("/testlogger")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> TestLogger()
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            _logger.LogInformation("TestLogInformation");
            _logger.LogWarning("TestLogWarning");
            _logger.LogError("TestLogError");
            _logger.LogCritical("TestLogCritical");
            return Ok();
        }

        [HttpGet("/sale")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetAllSaler()
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            IEnumerable<Sale> list = await _model.sale.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("/sale/{id}")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetSalerById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            IEnumerable<Sale> list = await _model.sale.GetOneByIdAsync(id);
            return Ok(list);
        }

        [HttpPut("/sale/{id}")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> SalerChangeTotalAmountById([FromRoute] int id, [FromQuery] int total_amount)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.sale.ChangeTotalAmountById(id, total_amount);
            return Ok();
        }

        [HttpPut("/sale/add")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddSaler([FromQuery] int total_amount, [FromQuery] int? id_bayer)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            var user = HttpContext.User.Identities.Where(x=> x.AuthenticationType == "Ruth" && x.NameClaimType == "Auth").Count() > 0 ? true : false;
            var id_provided_products = await _model.sale.AddAsync(total_amount, id_bayer, user);
            return Ok(id_provided_products);
        }

        [HttpDelete("/sale/delete")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteSaler([FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.sale.DeleteAsync(id);
            return Ok();
        }
    }
}
