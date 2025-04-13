using Microsoft.EntityFrameworkCore;
using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Repositorios.Turma
{
    public class TurmaRepositoryImpl : ITurmaRepository
    {
        private readonly AppDbContext _appDbContext;

        public TurmaRepositoryImpl(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<string> CreateAsync(TblTurma tblTurma)
        {
            _appDbContext.Add(tblTurma);
            await _appDbContext.SaveChangesAsync();
            return tblTurma.Id;
        }

        public async Task<TblTurma> GetByIdAsync(string id)
        {
            return await _appDbContext.TblTurma
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id) ?? throw new KeyNotFoundException("Turma não encontrada");
        }

        public async Task<List<TblTurma>> GetListAsync()
        {
            return await _appDbContext.TblTurma
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(TblTurma tblTurma)
        {
            var existingEntity = await _appDbContext.TblTurma.FindAsync(tblTurma.Id);
            if (existingEntity == null)
            {
                return false;
            }
            _appDbContext.Entry(existingEntity).CurrentValues.SetValues(tblTurma);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _appDbContext.TblTurma.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            _appDbContext.TblTurma.Remove(entity);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<TblTurma?> GetForCreate(EnumTurnoTurma enumTurnoTurma, int bloco, string nome)
        {
            TblTurma? turmaEncontrada = await (from turma in _appDbContext.TblTurma
                                               where turma.Turno == enumTurnoTurma.ToString()
                                               where turma.Bloco == bloco
                                               where turma.Nome == nome
                                               select turma).FirstOrDefaultAsync();
            return turmaEncontrada;
        }
    }
}
