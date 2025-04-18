using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto;
using WAPI_GS.Dto.Turma;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/turma")]
    public class TurmaController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public TurmaController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost()]
        public async Task<ActionResult<string>> Create([FromBody] DtoCreateTurma dto)
        {
            string id = await _uow.TurmaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TblTurma>> GetById(string id)
        {
            TblTurma tblTurma = await _uow.TurmaService.GetByIdAsync(id);
            return Ok(tblTurma);
        }

        [HttpGet]
        public async Task<ActionResult<List<TblTurma>>> GetList()
        {
            List<TblTurma> tblTurmas = await _uow.TurmaService.GetListAsync();
            return Ok(tblTurmas);
        }


        [HttpGet("combo")]
        public async Task<ActionResult<List<DtoGetCombo>>> GetListCombo()
        {
            try
            {
                var result = await _uow.TurmaService.GetListCombo();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<string>> Update([FromBody] DtoCreateTurma dto, string id)
        {
            string _id = await _uow.TurmaService.Update(dto, id);
            return Ok(_id);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<string>> Delete(string id)
        {
            string _id = await _uow.TurmaService.Delete(id);
            return Ok(_id);
        }
    }
}
