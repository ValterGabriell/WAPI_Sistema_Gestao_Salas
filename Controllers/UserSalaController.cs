using CS800_Model_iCorp;
using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto.User;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.Interfaces;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/UserController")]
    public class UserSalaController(ICS_UnitOfWork uow) : ControllerBase
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


        [HttpGet("sala/{salaId}")]
        public async Task<ActionResult<DtoGetUserSala>> GetBySalaId(int salaId)
        {
            try
            {
                var result = await _uow.UserSalaRepository.GetBySalaId(salaId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<DtoGetUserSala>> GetByUserId(int userId)
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
        public async Task<ActionResult<PagedList<DtoGetUserSala>>> GetList()
        {
            try
            {
                var result = await _uow.UserSalaRepository.GetList();
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
