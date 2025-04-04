using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto;
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
        public async Task<ActionResult<string>> Create(DtoAtribuirProfessorASala dto, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.UserSalaRepository.Create(dto, requestKey);
                await _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("/sendEmail")]
        public async Task<ActionResult<bool>> SendEmail([FromBody] DtoSendEmail dtoSendEmail, [FromHeader] string requestKey)
        {
            try
            {
                var scheme = HttpContext.Request.Scheme; // "http" ou "https"
                var host = HttpContext.Request.Host.Value; // Exemplo: localhost:5000 ou meu-app.onrender.com
                var fullUrl = $"{scheme}://{host}"; // Construindo a URL completa
                var title = "Solicitação de substituição de horário " + dtoSendEmail.salaNome;

                var result = await _uow.UserSalaRepository.SendEmailSolicitacao(dtoSendEmail.destEmail, dtoSendEmail.body, title, fullUrl,
                    dtoSendEmail.salaId,
                    dtoSendEmail.dia,
                    dtoSendEmail.currentUserId,
                    dtoSendEmail.currentUsername,
                    dtoSendEmail.horaInit,
                    dtoSendEmail.horaFinal,
                    requestKey
                    );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/accept")]
        public async Task<ActionResult<bool>> Accept([FromQuery] int salaId, [FromQuery] string dia, [FromQuery] int userId, [FromQuery] string currentUsername,
            [FromQuery] int horaInit,
            [FromQuery] int horaFinal)
        {
            try
            {
                await _uow.UserSalaRepository.Accept(salaId, DateOnly.Parse(dia), userId, currentUsername, horaInit, horaFinal);
                return Ok("Um email foi enviado ao professor com sua resposta, pode fechar essa aba");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/notAccept")]
        public async Task<ActionResult<string>> NotAccept([FromQuery] int salaId)
        {
            try
            {
                await _uow.UserSalaRepository.NotAccept(salaId);
                return Ok("Um email foi enviado ao professor com sua resposta, pode fechar essa aba");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public void Delete([FromQuery] int userId, [FromQuery] int salaId, [FromHeader] string requestKey)
        {
            _uow.UserSalaRepository.Delete(userId, salaId, requestKey);
            _uow.Commit();
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetUserSala>>> GetList([FromQuery] int? salaId, [FromQuery] int? profId, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.UserSalaRepository.GetList(salaId, profId, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]

        public async Task<ActionResult<string>> Update(DtoAtualizarAtribuicaoProfessorSala dto, [FromQuery] int salaId, [FromQuery] int oldUserId, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.UserSalaRepository.Update(dto, oldUserId, salaId, requestKey);
                await _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
