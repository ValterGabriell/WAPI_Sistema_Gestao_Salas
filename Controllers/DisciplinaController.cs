using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto.Disciplina;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/disciplina")]
    public class DisciplinaController(ICS_UnitOfWork uow) : ControllerBase
    {
        private readonly ICS_UnitOfWork _uow = uow;


        [HttpPost]

        public async Task<ActionResult<string>> Create(DtoCreateDisciplina dto, string requestKey)
        {
            try
            {
                var result = await _uow.cS_Disciplina.Create(dto, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
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
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<TblDisciplina>>> GetList(string requestKey)
        {
            try
            {
                var result = await _uow.cS_Disciplina.GetList(requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }
    }
}
