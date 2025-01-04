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
        public ActionResult<string> Create(DtoCreateSala dto)
        {
            try
            {
                var result = _uow.SalaRepository.Create(dto);
                _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _uow.SalaRepository.Delete(id);
            _uow.Commit();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DtoGetSala>> GetById(int id)
        {
            try
            {
                var result = await _uow.SalaRepository.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetSala>>> GetList([FromQuery]FiltersParameter filtersParameter)
        {
            try
            {
                var result = await _uow.SalaRepository.GetList(filtersParameter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<string>> Update(DtoCreateSala dto, int id)
        {
            try
            {
                var result = await _uow.SalaRepository.Update(dto, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
