using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto.Sala;
using WAPI_GS.Interfaces;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/sala")]
    public class SalaController(IUnitOfWork uow) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;

        [HttpPost()]
        public async Task<ActionResult<string>> Create(DtoCreateSala dto, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.SalaRepository.CreateAsync(dto, requestKey);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DtoGetSala>> GetById(int id, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.SalaRepository.GetByIdAsync(id, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetSala>>> GetList([FromQuery] FiltersParameter filtersParameter, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.SalaRepository.GetListAsync(filtersParameter, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<string>> Update(DtoCreateSala dto, int id, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.SalaRepository.UpdateAsync(dto, id, requestKey);
                await _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<string>> Delete(int id, [FromHeader] string requestKey)
        {
            try
            {
                await _uow.SalaRepository.DeleteAsync(id, requestKey);
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
