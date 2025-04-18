using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto;
using WAPI_GS.Dto.Disciplina;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/disciplina")]
    public class DisciplinaController(IUnitOfWork uow) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;


        [HttpPost]

        public async Task<ActionResult<string>> Create(DtoCreateDisciplina dto)
        {
            try
            {
                var result = await _uow.DisciplinaService.Create(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        [HttpPut]
        public async Task<ActionResult<string>> Update(DtoCreateDisciplina dto, int id)
        {
            try
            {
                var result = await _uow.DisciplinaService.UpdateAsync(dto, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<TblDisciplina>>> GetList()
        {
            try
            {
                var result = await _uow.DisciplinaService.GetList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpGet("combo")]
        public async Task<ActionResult<List<DtoGetCombo>>> GetListCombo()
        {
            try
            {
                var result = await _uow.DisciplinaService.GetListCombo();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }
    }
}
