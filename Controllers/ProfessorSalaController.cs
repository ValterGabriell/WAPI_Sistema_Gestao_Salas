using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.Interfaces;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/professorsala")]
    public class ProfessorSalaController(IUnitOfWork uow) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;

        [HttpPost]
        public async Task<ActionResult<string>> AtribuirProfessorASala(DtoAtribuirProfessorASala dto)
        {
            try
            {
                var result = await _uow.AtribuicaoService.AtribuirProfessorASala(dto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        public async Task RemoverAtribuicaoProfessorSala([FromQuery] int userId, [FromQuery] int salaId, [FromQuery] string turmaID, [FromQuery] DateOnly dia)
        {
            await _uow.AtribuicaoService.RemoverAtribuicaoProfessorSala(userId, salaId, turmaID, dia);
        }

        [HttpDelete("todos")]
        public async Task RemoverTodasAtribuicaoProfessorSala([FromQuery] int userId, [FromQuery] int salaId, [FromQuery] string turmaID)
        {
            await _uow.AtribuicaoService.RemoverTodasAtribuicaoProfessorSala(userId, salaId, turmaID);
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetUserSala>>> GetList()
        {
            try
            {
                var result = await _uow.AtribuicaoService.GetList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]

        public async Task<ActionResult<string>> AtualizarAtribuicaoProfessorASala(DtoAtualizarAtribuicaoProfessorSala dto, [FromQuery] int salaId, [FromQuery] int oldUserId)
        {
            try
            {
                var result = await _uow.AtribuicaoService.AtualizarAtribuicaoProfessorASala(dto, oldUserId, salaId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
