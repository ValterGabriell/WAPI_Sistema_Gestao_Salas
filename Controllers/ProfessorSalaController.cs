using CS800_Model_iCorp;
using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.Interfaces;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/professorsala")]
    public class ProfessorSalaController(ICS_UnitOfWork uow) : ControllerBase
    {
        private readonly ICS_UnitOfWork _uow = uow;

        [HttpPost]
        public async Task<ActionResult<string>> Create(DtoCreateUserSala dto, [FromHeader] string requestKey)
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
                    dtoSendEmail.newUserId,
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
        public async Task<ActionResult<bool>> Accept([FromQuery] int salaId, [FromQuery] DateOnly dia, [FromQuery] int userId, [FromQuery] int newUserId,
            [FromQuery] int horaInit,
            [FromQuery] int horaFinal)
        {
            try
            {
                bool v = await _uow.UserSalaRepository.Accept(salaId, dia, userId, newUserId, horaInit, horaFinal);
                return Ok(v);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/notAccept")]
        public async Task<ActionResult<bool>> NotAccept([FromQuery] int salaId)
        {
            try
            {
                return Ok(await _uow.UserSalaRepository.NotAccept(salaId));
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

        public async Task<ActionResult<string>> Update(DtoUpdateSalaUser dto, [FromQuery] int salaId, [FromQuery] int oldUserId, [FromHeader] string requestKey)
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
