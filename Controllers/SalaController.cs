using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto;
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
        public async Task<ActionResult<string>> Create(DtoCreateSala dto)
        {
            try
            {
                var result = await _uow.SalaService.Create(dto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DtoGetSala>> GetById(int id)
        {
            try
            {
                var result = await _uow.SalaService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetSala>>> GetList([FromQuery] FiltersParameter filtersParameter)
        {
            try
            {
                var result = await _uow.SalaService.GetListAsync(filtersParameter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        [HttpGet("combo")]
        public async Task<ActionResult<List<DtoGetCombo>>> GetListCombo()
        {
            try
            {
                var result = await _uow.SalaService.GetListCombo(new FiltersParameter());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<string>> Update(DtoCreateSala dto, int id)
        {
            try
            {
                var result = await _uow.SalaService.UpdateAsync(dto, id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<string>> Delete(int id)
        {
            try
            {
                await _uow.SalaService.DeleteAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }
    }
}
