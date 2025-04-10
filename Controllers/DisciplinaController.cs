﻿using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto.Disciplina;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/disciplina")]
    public class DisciplinaController(IUnitOfWork uow) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;


        [HttpPost]

        public async Task<ActionResult<string>> Create(DtoCreateDisciplina dto, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.cS_Disciplina.CreateAsync(dto, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        [HttpPut]
        public async Task<ActionResult<string>> Update(DtoCreateDisciplina dto, int id, string requestKey)
        {
            try
            {
                var result = await _uow.cS_Disciplina.Update(dto, id, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<TblDisciplina>>> GetList([FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.cS_Disciplina.GetList(requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }
    }
}
