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
                var result = _uow.UserRepository.Create(dto);
                _uow.Commit();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DtoGetUser>> GetById(int id)
        {
            try
            {
                var result = await _uow.UserRepository.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetUser>>> GetList([FromQuery] FiltersParameter filtersParameter)
        {
            try
            {
                var result = await _uow.UserRepository.GetList(filtersParameter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<string>> Update(DtoCreateUser dto, int id)
        {
            try
            {
                var result = await _uow.UserRepository.Update(dto, id);
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


