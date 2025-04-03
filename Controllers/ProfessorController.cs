using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto.User;
using WAPI_GS.Interfaces;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Controllers
{
    [ApiController()]
    [Route("api/v1/professor")]
    public class ProfessorController(ICS_UnitOfWork uow) : ControllerBase
    {
        private readonly ICS_UnitOfWork _uow = uow;

        [HttpPost]
        public ActionResult<string> Create(DtoCreateUpdateUser dto)
        {
            try
            {
                var result = _uow.UserRepository.CreateAsync(dto, "");
                _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DtoGetProfessor>> GetById(int id, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.UserRepository.GetByIdAsync(id, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetProfessor>>> GetList([FromQuery] FiltersParameter filtersParameter, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.UserRepository.GetListAsync(filtersParameter, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<string>> Update(DtoCreateUpdateUser dto, int id, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.UserRepository.UpdateAsync(dto, id, requestKey);
                await _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id, [FromHeader] string requestKey)
        {
            try
            {
                await _uow.UserRepository.DeleteAsync(id, requestKey);
                await _uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }
    }
}


