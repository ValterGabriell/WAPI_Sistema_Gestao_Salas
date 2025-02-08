using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WAPI_GS.Modelos;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/turma")]
    public class TurmaController(AppDbContext appDbContext) : ControllerBase
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        [HttpPost()]
        public async Task<ActionResult<string>> Create([FromBody] TblTurma entity, [FromHeader] string requestKey)
        {
            try
            {

                var id = Guid.NewGuid().ToString();
                entity.Id = id;
                _appDbContext.Add(entity);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            return "Entidade gerada!";
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TblTurma>> GetById(string id, [FromHeader] string requestKey)
        {
            try
            {
                TblTurma tblTurma = await _appDbContext.TblTurma
                     .AsNoTracking().FirstOrDefaultAsync(e => e.Id == id) ?? throw new KeyNotFoundException("Key Not FOund");
                return Ok(tblTurma);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<TblTurma>>> GetList([FromQuery] FiltersParameter filtersParameter, [FromHeader] string requestKey)
        {
            try
            {
                List<TblTurma> tblTurmas = await _appDbContext.TblTurma
                    .AsNoTracking().ToListAsync();

                return Ok(tblTurmas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<string>> Update([FromBody] TblTurma newEntity, string id, [FromHeader] string requestKey)
        {
            try
            {
                TblTurma _ = await _appDbContext.TblTurma
                    .AsNoTracking().FirstOrDefaultAsync(e => e.Id == id) ?? throw new KeyNotFoundException("Key Not Fund");


                newEntity.Id = id;
                _appDbContext.Update(newEntity);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return id.ToString();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<string>> Delete(string id, [FromHeader] string requestKey)
        {
            try
            {
                var entity = await _appDbContext.TblTurma.Where(e => e.Id == id).FirstAsync();
                _appDbContext.Remove(entity);
                return Ok("Deletado");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
