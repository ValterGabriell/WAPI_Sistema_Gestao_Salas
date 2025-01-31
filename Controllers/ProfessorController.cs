using CS800_Model_iCorp;
using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto.User;
using WAPI_GS.Interfaces;

namespace WAPI_GS.Controllers
{
    [ApiController()]
    [Route("api/v1/professor")]
    public class ProfessorController(ICS_UnitOfWork uow) : ControllerBase
    {
        private readonly ICS_UnitOfWork _uow = uow;

        [HttpPost]
        public ActionResult<string> Create(DtoCreateUser dto)
        {
            try
            {
                var result = _uow.UserRepository.Create(dto, "");
                _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DtoGetUser>> GetById(int id, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.UserRepository.GetById(id, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetUser>>> GetList([FromQuery] FiltersParameter filtersParameter, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.UserRepository.GetList(filtersParameter, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<string>> Update(DtoCreateUser dto, int id, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.UserRepository.Update(dto, id, requestKey);
                await _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id, [FromHeader] string requestKey)
        {
            try
            {
                await _uow.UserRepository.Delete(id, requestKey);
                await _uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }
    }
}


