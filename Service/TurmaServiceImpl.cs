using WAPI_GS.Dto;
using WAPI_GS.Dto.Turma;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Repositorios.Turma;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Service
{
    public class TurmaServiceImpl : ITurmaService
    {
        private readonly ITurmaRepository _turmaRepository;

        public TurmaServiceImpl(ITurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository;
        }

        public async Task<string> CreateAsync(DtoCreateTurma dto)
        {
            try
            {
                var turmaEncontrada = await _turmaRepository.GetForCreate(dto.Turno, dto.Bloco, dto.Nome);
                if (turmaEncontrada != null) throw new ArgumentException("Já existe uma turma para esse turno/bloco com o mesmo nome");

                var id = Guid.NewGuid().ToString();
                var entity = dto.ToEntity(id);
                await _turmaRepository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }

            return "Entidade gerada!";
        }



        public async Task<TblTurma> GetByIdAsync(string id)
        {
            try
            {
                TblTurma tblTurma = await _turmaRepository.GetByIdAsync(id);
                return tblTurma;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        public async Task<List<TblTurma>> GetListAsync()
        {
            try
            {
                List<TblTurma> tblTurmas = await _turmaRepository.GetListAsync();

                return tblTurmas;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<List<DtoGetCombo>> GetListCombo()
        {
            try
            {
                var lista = await _turmaRepository.GetListAsync();
                return lista.Select(e => new DtoGetCombo(e.Id.ToString(), e.Nome ?? "-")).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }



        public async Task<string> Update(DtoCreateTurma dto, string id)
        {
            try
            {
                TblTurma entity = dto.ToEntity(id);
                bool ok = await _turmaRepository.UpdateAsync(entity);
                return ok ? "Atualizado" : "Erro ao atualizar";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<string> Delete(string id)
        {
            try
            {
                var ok = await _turmaRepository.DeleteAsync(id);
                return ok ? "Deletado" : "Erro ao deletar";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
