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
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ApplicationDatabaseContext _db;
        private readonly EntitysModel _model;

        public ProductController(ILogger<ProductController> logger, ApplicationDatabaseContext db, EntitysModel model)
        {
            this._logger = logger;
            this._db = db;
            this._model = model;
        }


        [HttpGet("/product")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetAllProduct()
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            IEnumerable<Product> list = await _model.product.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("/product/{id}")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            IEnumerable<Product> list = await _model.product.GetOneByIdAsync(id);
            return Ok(list);
        }

        [HttpPut("/product/{id}")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> ProductCangePriceById([FromRoute] int id, [FromQuery] double price)
        {
            if(!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.product.ChangePriceById(id, price);
            return Ok();
        }

        [HttpPut("/product/add")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddProduct([FromQuery] string name, [FromQuery] double price)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.product.AddAsync(name, price);
            return Ok();
        }

        [HttpDelete("/product/delete")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteProduct([FromQuery] string name)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.product.DeleteAsync(name);
            return Ok();
        }

    }
}
