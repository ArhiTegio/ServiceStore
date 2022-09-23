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
    public class ProvidedProductsController : ControllerBase
    {
        private readonly ILogger<ProvidedProductsController> _logger;
        private readonly ApplicationDatabaseContext _db;
        private readonly EntitysModel _model;

        public ProvidedProductsController(ILogger<ProvidedProductsController> logger, ApplicationDatabaseContext db, EntitysModel model)
        {
            this._logger = logger;
            this._db = db;
            this._model = model;
        }

        [HttpGet("/provided_products")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetAllProvidedProducts()
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            IEnumerable<ProvidedProducts> list = await _model.provided_products.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("/provided_products/{id}")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetProvidedProductsById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            IEnumerable<ProvidedProducts> list = await _model.provided_products.GetOneByIdAsync(id);
            return Ok(list);
        }

        [HttpPut("/provided_products/{id}")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> ProvidedProductsCangeCountById([FromRoute] int id, [FromQuery] int count)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.provided_products.ChangePriceById(id, count);
            return Ok();
        }

        [HttpGet("/provided_products/add")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddProvidedProducts([FromQuery] int id_product, [FromQuery] int count)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            var id_provided_products = await _model.provided_products.AddAsync(id_product, count);
            return Ok(id_provided_products);
        }

        [HttpDelete("/provided_products/delete")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteProvidedProducts([FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.provided_products.DeleteAsync(id);
            return Ok();
        }

    }
}
