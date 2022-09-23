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
    [Route("api/[controller]")]
    [ApiController]
    public class SalesDataController : ControllerBase
    {
        private readonly ILogger<SalesPointController> _logger;
        private readonly ApplicationDatabaseContext _db;
        private readonly EntitysModel _model;

        public SalesDataController(ILogger<SalesPointController> logger, ApplicationDatabaseContext db, EntitysModel model)
        {
            this._logger = logger;
            this._db = db;
            this._model = model;
        }

        
        [HttpGet("/sales_data")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetAllSalesPoint()
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var t1 = this.ControllerContext;
            var t2 = this.HttpContext;

            IEnumerable<SalesData> list = await _model.salesDataModel.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("/sales_data/{id}")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetSalesPointById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            
            IEnumerable<SalesData> list = await _model.salesDataModel.GetOneByIdAsync(id);
            return Ok(list);
        }

        [HttpPut("/sales_data/{id}")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> SalesPointChangeById([FromRoute] int id, [FromQuery] int? productQuantity, [FromQuery] int? productIdAmount)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.salesDataModel.ChangeById(id, productQuantity, productIdAmount);
            return Ok();
        }

        [HttpPut("/sales_data/add")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddSalesPoint([FromQuery] int products_id, [FromQuery] int sale_id, [FromQuery] int productQuantity, [FromQuery] int productIdAmount)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            var id_provided_products = await _model.salesDataModel.AddAsync(products_id, sale_id, productQuantity, productIdAmount);
            return Ok(id_provided_products);
        }

        [HttpDelete("/sales_data/delete")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteSalesPoint([FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.salesDataModel.DeleteAsync(id);
            return Ok();
        }
    }
}
