using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto.User;
using WAPI_GS.Interfaces;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Controllers
{
    [ApiController()]
    [Route("api/v1/professor")]
    public class ProfessorController(IUnitOfWork uow) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;

        [HttpPost]
        public async Task<ActionResult<string>> Create(DtoCreateUpdateUser dto)
        {
            try
            {
                var result = await _uow.ProfessorService.CreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DtoGetProfessor>> GetById(int id)
        {
            try
            {
                var result = await _uow.ProfessorService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetProfessor>>> GetList([FromQuery] FiltersParameter filtersParameter)
        {
            try
            {
                var result = await _uow.ProfessorService.GetListAsync(filtersParameter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<string>> Update(DtoCreateUpdateUser dto, int id)
        {
            try
            {
                var result = await _uow.ProfessorService.UpdateAsync(dto, id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _uow.ProfessorService.DeleteAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }
    }
}


