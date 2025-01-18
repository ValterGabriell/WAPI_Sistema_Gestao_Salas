using CS800_Model_iCorp;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<string> Create(DtoCreateUserSala dto)
        {
            try
            {
                var result = _uow.UserSalaRepository.Create(dto);
                _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public void Delete([FromQuery] int userId, [FromQuery] int salaId)
        {
            _uow.UserSalaRepository.Delete(userId, salaId);
            _uow.Commit();
        }


        [HttpGet("sala/{salaNome}")]
        public async Task<ActionResult<PagedList<DtoGetUserSala>>> GetBySalaId(string salaNome)
        {
            try
            {
                var result = await _uow.UserSalaRepository.GetBySalaNome(salaNome);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<PagedList<DtoGetUserSala>>> GetByUserId(int userId)
        {
            try
            {
                var result = await _uow.UserSalaRepository.GetByUserId(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetUserSala>>> GetList([FromQuery] int? salaId, [FromQuery] int? profId)
        {
            try
            {
                var result = await _uow.UserSalaRepository.GetList(salaId, profId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut]

        public async Task<ActionResult<string>> Update(DtoCreateUserSala dto, [FromQuery] int salaId, [FromQuery] int userId)
        {
            try
            {
                var result = await _uow.UserSalaRepository.Update(dto, userId, salaId);
                await _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
