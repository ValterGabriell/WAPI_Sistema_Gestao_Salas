using CS800_Model_iCorp;
using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto.Sala;
using WAPI_GS.Interfaces;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/sala")]
    public class SalaController(ICS_UnitOfWork uow) : ControllerBase
    {
        private readonly ICS_UnitOfWork _uow = uow;

        [HttpPost]
        public ActionResult<string> Create(DtoCreateSala dto, [FromHeader] string requestKey)
        {
            try
            {
                var result = _uow.SalaRepository.Create(dto, requestKey);
                _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
               return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpDelete]
        public void Delete(int id, [FromHeader] string requestKey)
        {
            _uow.SalaRepository.Delete(id, requestKey);
            _uow.Commit();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DtoGetSala>> GetById(int id, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.SalaRepository.GetById(id, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetSala>>> GetList([FromQuery]FiltersParameter filtersParameter, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.SalaRepository.GetList(filtersParameter, requestKey);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<string>> Update(DtoCreateSala dto, int id, [FromHeader] string requestKey)
        {
            try
            {
                var result = await _uow.SalaRepository.Update(dto, id, requestKey);
               await _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
               return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }
    }
}
