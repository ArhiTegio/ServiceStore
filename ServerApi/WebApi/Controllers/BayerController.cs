﻿using Microsoft.AspNetCore.Mvc;
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
    public class BayerController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;
        private readonly ApplicationDatabaseContext _db;
        private readonly EntitysModel _model;

        public BayerController(ILogger<SalesController> logger, ApplicationDatabaseContext db, EntitysModel model)
        {
            this._logger = logger;
            this._db = db;
            this._model = model;
        }

        [HttpGet("/bayer")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetAllBayer()
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            IEnumerable<Bayer> list = await _model.bayer.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("/bayer/{id}")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetBayerById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            IEnumerable<Bayer> list = await _model.bayer.GetOneByIdAsync(id);
            return Ok(list);
        }

        [HttpPut("/bayer/add")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddBayer([FromQuery] string name)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.bayer.AddAsync(name);
            return Ok();
        }

        [HttpDelete("/bayer/delete")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteBayer([FromQuery] string name)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            await _model.bayer.DeleteAsync(name);
            return Ok();
        }
    }
}
