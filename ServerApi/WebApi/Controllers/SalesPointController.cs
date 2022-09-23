using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server_Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server_Model.Base;
using Server_Data.EntityDb;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SalesPointController : ControllerBase
    {
        private readonly ILogger<SalesPointController> _logger;
        private readonly ApplicationDatabaseContext _db;
        private readonly EntitysModel _model;

        public SalesPointController(ILogger<SalesPointController> logger, ApplicationDatabaseContext db, EntitysModel model)
        {
            this._logger = logger;
            this._db = db;
            this._model = model;
        }

        [HttpGet("/sales_point")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetAllSalesPoint()
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            IEnumerable<SalesPoint> list = await _model.salesPointModel.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("/sales_point/{id}")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetSalesPointById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            IEnumerable<SalesPoint> list = await _model.salesPointModel.GetOneByIdAsync(id);
            return Ok(list);
        }

        [HttpPut("/sales_point/{id}")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> SalesPointChangeTotalAmountById([FromRoute] int id, [FromQuery] string name)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.salesPointModel.ChangeNameById(id, name);
            return Ok();
        }

        [HttpPut("/sales_point/add")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddSalesPoint([FromQuery] string name, [FromQuery] int provided_products_id, [FromQuery] int sale_id)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            var id_provided_products = await _model.salesPointModel.AddAsync(name, provided_products_id, sale_id);
            return Ok(id_provided_products);
        }

        [HttpDelete("/sales_point/delete")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteSalesPoint([FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.salesPointModel.DeleteAsync(id);
            return Ok();
        }
    }
}
