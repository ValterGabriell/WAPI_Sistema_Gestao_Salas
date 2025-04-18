using WAPI_GS.Dto;
using WAPI_GS.Dto.Disciplina;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Repositorios.Disciplina;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Service
{
    public class DisciplinaService(IDisciplinaRepository repository) : IDisciplinaService
    {
        private readonly IDisciplinaRepository _repository = repository;
        public async Task<string> Create(DtoCreateDisciplina dto)
        {
            try
            {
                TblDisciplina newDisciplina = dto.ToEntity();
                string message = await _repository.Create(newDisciplina);
                return message;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<string> UpdateAsync(DtoCreateDisciplina dto, int id)
        {
            try
            {
                TblDisciplina? tblDisciplina = await _repository.RecuperaDisciplinaPorIDELancaExcecaoSeNaoAchar(id);
                tblDisciplina = dto.ToEntityForUpdate(id);

                string updateResult = await _repository.Update(tblDisciplina);
                return updateResult;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<List<TblDisciplina>> GetList()
        {
            try
            {
                List<TblDisciplina> tblDisciplinas = await _repository.GetListAsync();
                return tblDisciplinas;
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
                var lista = await _repository.GetListAsync();
                return lista.Select(e => new DtoGetCombo(e.Id.ToString(), e.Nome)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

    }
}
